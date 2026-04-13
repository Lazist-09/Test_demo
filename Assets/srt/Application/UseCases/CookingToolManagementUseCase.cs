using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Services;
using CookingGame.Core.Models;
using CookingGame.Application.DTOs;
using System.Linq;
using CookingGame.Core.Events;
using CookingGame.Core.Logging;

namespace CookingGame.Application.UseCases
{
    /// <summary>
    /// 烹饪工具管理用例
    /// 处理烹饪工具相关的业务逻辑
    /// </summary>
    public class CookingToolManagementUseCase
    {
        /// <summary>
        /// 烹饪工具仓储
        /// </summary>
        private readonly ICookingToolRepository _toolRepository;
        
        /// <summary>
        /// 物品仓储
        /// </summary>
        private readonly IItemRepository _itemRepository;
        
        /// <summary>
        /// 配方匹配器
        /// </summary>
        private readonly IRecipeMatcher _recipeMatcher;
        
        /// <summary>
        /// 复合食材服务
        /// </summary>
        private readonly ICompositeIngredientService _compositeService;
        
        /// <summary>
        /// 日志服务
        /// </summary>
        private readonly CookingGame.Core.Logging.ILogger _logger;

        /// <summary>
        /// 构造函数
        /// 通过依赖注入获取所需的服务
        /// </summary>
        /// <param name="toolRepository">烹饪工具仓储</param>
        /// <param name="itemRepository">物品仓储</param>
        /// <param name="recipeMatcher">配方匹配器</param>
        /// <param name="compositeService">复合食材服务</param>
        /// <param name="logger">日志服务</param>
        public CookingToolManagementUseCase(
            ICookingToolRepository toolRepository,
            IItemRepository itemRepository,
            IRecipeMatcher recipeMatcher,
            ICompositeIngredientService compositeService,
            CookingGame.Core.Logging.ILogger logger)
        {
            _toolRepository = toolRepository;
            _itemRepository = itemRepository;
            _recipeMatcher = recipeMatcher;
            _compositeService = compositeService;
            _logger = logger;
        }

        /// <summary>
        /// 创建烹饪工具
        /// </summary>
        /// <param name="type">工具类型</param>
        /// <returns>创建的工具DTO</returns>
        public CookingToolDto CreateTool(CookingToolType type)
        {
            _logger.Info("CookingToolManagementUseCase.CreateTool: type={ToolType}", type);
            var tool = new CookingTool(_toolRepository.GenerateId(), type);
            _toolRepository.Save(tool);
            _logger.Info("Tool created: {ToolId}, Type: {ToolType}", tool.Id, tool.Type);
            return CookingToolDto.FromTool(tool);
        }

        /// <summary>
        /// 获取单个烹饪工具
        /// </summary>
        /// <param name="id">工具ID</param>
        /// <returns>工具DTO，如果不存在则返回null</returns>
        public CookingToolDto GetTool(string id)
        {
            _logger.Debug("CookingToolManagementUseCase.GetTool: {ToolId}", id);
            var tool = _toolRepository.GetById(id);
            if (tool == null)
            {
                _logger.Warning("Tool {ToolId} not found!", id);
                return null;
            }
            return CookingToolDto.FromTool(tool);
        }

        /// <summary>
        /// 获取所有烹饪工具
        /// </summary>
        /// <returns>工具DTO列表</returns>
        public List<CookingToolDto> GetAllTools()
        {
            var tools = _toolRepository.GetAll();
            return tools.ConvertAll(CookingToolDto.FromTool);
        }

        /// <summary>
        /// 检查是否可以向工具添加食材
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <param name="itemId">食材ID</param>
        /// <returns>是否可以添加</returns>
        public bool CanAddItemToTool(string toolId, string itemId)
        {
            _logger.Debug("CanAddItemToTool: toolId={ToolId}, itemId={ItemId}", toolId, itemId);
            var tool = _toolRepository.GetById(toolId);
            var item = _itemRepository.GetById(itemId);
            if (tool == null || item == null) return false;
            var canAdd = tool.CanAddItem();
            _logger.Debug("Can add: {CanAdd}", canAdd);
            return canAdd;
        }

        /// <summary>
        /// 向工具添加食材
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <param name="itemId">食材ID</param>
        public void AddItemToTool(string toolId, string itemId)
        {
            _logger.Info("AddItemToTool: toolId={ToolId}, itemId={ItemId}", toolId, itemId);
            var tool = _toolRepository.GetById(toolId);
            var item = _itemRepository.GetById(itemId);
            if (tool != null && item != null && tool.CanAddItem())
            {
                _logger.Debug("Adding item to tool, current count: {ItemCount}", tool.InputItems.Count);
                tool.InputItems.Add(item);
                _toolRepository.Update(tool);
                _logger.Debug("Item added successfully, new count: {ItemCount}", tool.InputItems.Count);
            }
            else
            {
                _logger.Warning("Cannot add item: tool={ToolExists}, item={ItemExists}, canAdd={CanAdd}", 
                    tool != null, item != null, tool?.CanAddItem());
            }
        }

        /// <summary>
        /// 启动工具烹饪
        /// </summary>
        /// <param name="toolId">工具ID</param>
        public void StartCooking(string toolId)
        {
            _logger.Info("StartCooking: {ToolId}", toolId);
            var tool = _toolRepository.GetById(toolId);
            if (tool != null)
            {
                _logger.Debug("Starting tool: {ToolId}, InputItems: {ItemCount}", tool.Id, tool.InputItems.Count);
                tool.StartCooking();
                _toolRepository.Update(tool);
                _logger.Info("Tool started: {IsRunning}", tool.IsRunning);
                
                // 触发工具启动事件
                GameEvents.InvokeToolStarted(tool);
            }
            else
            {
                _logger.Error("Tool {ToolId} not found!", toolId);
            }
        }

        /// <summary>
        /// 停止工具烹饪
        /// </summary>
        /// <param name="toolId">工具ID</param>
        public void PauseCooking(string toolId)
        {
            _logger.Info("PauseCooking: {ToolId}", toolId);
            var tool = _toolRepository.GetById(toolId);
            if (tool != null)
            {
                tool.StopCooking();
                _toolRepository.Update(tool);
                
                // 触发工具暂停事件
                GameEvents.InvokeToolPaused(tool);
            }
        }

        /// <summary>
        /// 完成烹饪并输出菜品
        /// </summary>
        /// <param name="toolId">工具ID</param>
        public void CompleteCooking(string toolId)
        {
            _logger.Info("CookingToolManagementUseCase.CompleteCooking: ToolId={ToolId}", toolId);
            var tool = _toolRepository.GetById(toolId);
            if (tool != null)
            {
                tool.StopCooking();
                ProcessToolOutput(tool);
                _toolRepository.Update(tool);
                
                // 触发工具完成事件
                GameEvents.InvokeToolCompleted(tool);
            }
        }

        /// <summary>
        /// 增加工具处理进度
        /// 在烹饪过程中调用
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <param name="amount">增加的进度值</param>
        public void AddToolProgress(string toolId, float amount)
        {
            var tool = _toolRepository.GetById(toolId);
            if (tool != null)
            {
                tool.AddProgress(amount);
                _toolRepository.Update(tool);

                if (tool.CurrentProgress >= 1.0f)
                {
                    ProcessToolOutput(tool);
                }
            }
        }

        /// <summary>
        /// 处理工具输出
        /// 根据输入的食材匹配配方并生成输出菜品
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        private void ProcessToolOutput(CookingTool tool)
        {
            _logger.Info("ProcessToolOutput: {ToolId}, InputItems: {ItemCount}", tool.Id, tool.InputItems.Count);
            if (tool.InputItems.Count == 0)
            {
                _logger.Info("No input items, clearing tool");
                tool.ClearInput();
                return;
            }

            var matchedRecipe = _recipeMatcher.MatchRecipe(tool.InputItems);
            Item outputItem;
            if (matchedRecipe != null)
            {
                int score = _recipeMatcher.CalculateScore(matchedRecipe, tool.InputItems);
                outputItem = CreateOutputItem(matchedRecipe, score);
                
                // 将输出菜品添加到原料仓储中
                _itemRepository.Save(outputItem);
                
                // 添加领域事件
                outputItem.AddDomainEvent(new ItemCreatedDomainEvent(outputItem));
                
                // 添加烹饪成功领域事件
                outputItem.AddDomainEvent(new CookingSuccessDomainEvent(tool.Id, outputItem.Id, score));
                
                _logger.Info("Recipe matched: {RecipeId}, Output: {OutputId}, Score: {Score}", 
                    matchedRecipe.Id, outputItem.Id, score);
            }
            else
            {
                // 优化 Trash 处理：尝试合并现有的 Trash
                outputItem = TryMergeTrash(tool);
                
                // 添加领域事件
                outputItem.AddDomainEvent(new TrashCreatedDomainEvent(outputItem));
                
                // 添加烹饪失败领域事件
                outputItem.AddDomainEvent(new CookingFailedDomainEvent(tool.Id, 10));
                
                _logger.Info("No recipe matched, trash created/merged: {TrashId}", outputItem.Id);
            }

            tool.SetOutput(outputItem);
            tool.ClearInput();
            tool.CurrentProgress = 0f;
            tool.StopCooking();
            _toolRepository.Update(tool);
            _logger.Info("ProcessToolOutput completed: {ToolId}, OutputItem: {OutputId}", tool.Id, outputItem.Id);
        }

        /// <summary>
        /// 尝试合并 Trash
        /// 如果已有相同属性的 Trash，则合并而不是创建新的
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>合并或新创建的 Trash</returns>
        private Item TryMergeTrash(CookingTool tool)
        {
            // 获取所有 Trash
            var allItems = _itemRepository.GetAll();
            var existingTrash = allItems
                .Where(i => i.TemplateId == "trash" 
                         && i.Shape == Shape.Whole 
                         && i.CookingStage == CookingStage.Burnt)
                .FirstOrDefault();

            if (existingTrash != null)
            {
                // 合并 Trash
                existingTrash.AddProgress(0.1f); // 每次合并增加 0.1 进度，表示数量增加
                _itemRepository.Update(existingTrash);
                
                // 添加合并领域事件
                existingTrash.AddDomainEvent(new TrashMergedDomainEvent(
                    existingTrash.Id, 
                    existingTrash.Id, 
                    (int)(existingTrash.Progress * 10)));
                
                _logger.Info("Trash merged: {TrashId}, NewCount: {Count}", 
                    existingTrash.Id, (int)(existingTrash.Progress * 10));
                
                return existingTrash;
            }
            
            // 没有现有 Trash，创建新的
            var newTrash = new Item($"trash_{System.Guid.NewGuid()}", "trash", ItemType.Trash, 
                Shape.Whole, CookingStage.Burnt, ItemStatus.Inactive);
            _itemRepository.Save(newTrash);
            
            // 添加领域事件
            newTrash.AddDomainEvent(new TrashCreatedDomainEvent(newTrash));
            
            _logger.Info("New trash created: {TrashId}", newTrash.Id);
            
            return newTrash;
        }

        /// <summary>
        /// 创建输出菜品
        /// 根据配方和得分创建菜品
        /// </summary>
        /// <param name="recipe">配方</param>
        /// <param name="score">得分</param>
        /// <returns>创建的菜品</returns>
        private Item CreateOutputItem(Recipe recipe, int score)
        {
            var outputItem = new Item($"dish_{System.Guid.NewGuid()}", recipe.Id, ItemType.FinishedDish)
            {
                CookingStage = score >= 80 ? CookingStage.WellDone :
                              score >= 50 ? CookingStage.Medium : CookingStage.Raw
            };
            return outputItem;
        }

        /// <summary>
        /// 获取工具输出
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <returns>输出的菜品DTO，如果不存在则返回null</returns>
        public ItemDto GetToolOutput(string toolId)
        {
            var tool = _toolRepository.GetById(toolId);
            var outputDto = tool?.OutputItem != null ? ItemDto.FromItem(tool.OutputItem) : null;
            _logger.Debug("GetToolOutput: {ToolId}, OutputItem={OutputId}", toolId, outputDto?.Id);
            return outputDto;
        }

        /// <summary>
        /// 清空工具输出
        /// </summary>
        /// <param name="toolId">工具ID</param>
        public void ClearToolOutput(string toolId)
        {
            var tool = _toolRepository.GetById(toolId);
            if (tool != null)
            {
                tool.ClearOutput();
                _toolRepository.Update(tool);
                _logger.Info("Tool output cleared: {ToolId}", toolId);
            }
        }

        /// <summary>
        /// 清空工具输入
        /// </summary>
        /// <param name="toolId">工具ID</param>
        public void ClearToolInput(string toolId)
        {
            var tool = _toolRepository.GetById(toolId);
            if (tool != null && !tool.IsRunning)
            {
                tool.ClearInput();
                _toolRepository.Update(tool);
                _logger.Info("Tool input cleared: {ToolId}", toolId);
            }
        }

        /// <summary>
        /// 更新烹饪工具
        /// </summary>
        /// <param name="tool">要更新的工具</param>
        public void UpdateTool(CookingTool tool)
        {
            if (tool != null)
            {
                _toolRepository.Update(tool);
                _logger.Info("Tool {ToolId} updated", tool.Id);
            }
        }
    }
}
