using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Services;
using CookingGame.Core.Models;
using CookingGame.Application.DTOs;
using CookingGame.Core.Events;
using CookingGame.Core.Logging;

namespace CookingGame.Application.UseCases
{
    /// <summary>
    /// 物品管理用例
    /// 处理物品的创建、查询和转换逻辑
    /// </summary>
    public class ItemManagementUseCase
    {
        /// <summary>
        /// 物品仓储
        /// </summary>
        private readonly IItemRepository _itemRepository;
        
        /// <summary>
        /// 物品验证器
        /// </summary>
        private readonly IItemValidator _itemValidator;
        
        /// <summary>
        /// 日志服务
        /// </summary>
        private readonly CookingGame.Core.Logging.ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemRepository">物品仓储</param>
        /// <param name="itemValidator">物品验证器</param>
        /// <param name="logger">日志服务</param>
        public ItemManagementUseCase(
            IItemRepository itemRepository, 
            IItemValidator itemValidator,
            CookingGame.Core.Logging.ILogger logger)
        {
            _itemRepository = itemRepository;
            _itemValidator = itemValidator;
            _logger = logger;
        }

        /// <summary>
        /// 创建物品
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="category">物品分类</param>
        /// <returns>创建的物品DTO</returns>
        public ItemDto CreateItem(string templateId, ItemType category)
        {
            _logger.Info("ItemManagementUseCase.CreateItem: templateId={TemplateId}, category={Category}", templateId, category);
            var item = new Item($"item_{System.Guid.NewGuid()}", templateId, category);
            _itemRepository.Save(item);
            _logger.Info("Item created: {ItemId}", item.Id);
            
            // 添加领域事件
            item.AddDomainEvent(new ItemCreatedDomainEvent(item));
            
            // 触发菜品创建事件
            if (category == ItemType.FinishedDish)
            {
                GameEvents.InvokeDishCreated(item);
            }
            
            return ItemDto.FromItem(item);
        }

        /// <summary>
        /// 获取单个物品
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>物品DTO，如果不存在则返回null</returns>
        public ItemDto GetItem(string id)
        {
            _logger.Debug("ItemManagementUseCase.GetItem: {ItemId}", id);
            var item = _itemRepository.GetById(id);
            if (item == null)
            {
                _logger.Warning("Item {ItemId} not found!", id);
                return null;
            }
            return ItemDto.FromItem(item);
        }

        /// <summary>
        /// 获取所有物品
        /// </summary>
        /// <returns>物品DTO列表</returns>
        public List<ItemDto> GetAllItems()
        {
            var items = _itemRepository.GetAll();
            return items.ConvertAll(ItemDto.FromItem);
        }

        /// <summary>
        /// 验证形状转换
        /// </summary>
        /// <param name="itemId">物品ID</param>
        /// <param name="newShape">新形状</param>
        /// <returns>转换是否合法</returns>
        public bool ValidateShapeTransition(string itemId, Shape newShape)
        {
            var item = _itemRepository.GetById(itemId);
            if (item == null) return false;
            return _itemValidator.ValidateShapeTransition(item.Shape, newShape);
        }

        /// <summary>
        /// 验证熟度转换
        /// </summary>
        /// <param name="itemId">物品ID</param>
        /// <param name="newStage">新熟度</param>
        /// <returns>转换是否合法</returns>
        public bool ValidateCookingStageTransition(string itemId, CookingStage newStage)
        {
            var item = _itemRepository.GetById(itemId);
            if (item == null) return false;
            return _itemValidator.ValidateCookingStageTransition(item.CookingStage, newStage);
        }

        /// <summary>
        /// 更新物品形状
        /// 只有在转换合法的情况下才会更新
        /// </summary>
        /// <param name="itemId">物品ID</param>
        /// <param name="newShape">新形状</param>
        public void UpdateItemShape(string itemId, Shape newShape)
        {
            var item = _itemRepository.GetById(itemId);
            if (item != null && _itemValidator.ValidateShapeTransition(item.Shape, newShape))
            {
                item.SetShape(newShape);
                _itemRepository.Update(item);
                
                // 添加领域事件
                item.AddDomainEvent(new ItemUpdatedDomainEvent(item));
                
                _logger.Info("Item {ItemId} shape updated to {Shape}", itemId, newShape);
            }
        }

        /// <summary>
        /// 更新物品熟度
        /// 只有在转换合法的情况下才会更新
        /// </summary>
        /// <param name="itemId">物品ID</param>
        /// <param name="newStage">新熟度</param>
        public void UpdateItemCookingStage(string itemId, CookingStage newStage)
        {
            var item = _itemRepository.GetById(itemId);
            if (item != null && _itemValidator.ValidateCookingStageTransition(item.CookingStage, newStage))
            {
                item.SetCookingStage(newStage);
                _itemRepository.Update(item);
                
                // 添加领域事件
                item.AddDomainEvent(new ItemUpdatedDomainEvent(item));
                
                _logger.Info("Item {ItemId} cooking stage updated to {Stage}", itemId, newStage);
            }
        }

        /// <summary>
        /// 更新物品
        /// </summary>
        /// <param name="item">要更新的物品</param>
        public void UpdateItem(Item item)
        {
            if (item != null)
            {
                _itemRepository.Update(item);
                
                // 添加领域事件
                item.AddDomainEvent(new ItemUpdatedDomainEvent(item));
                
                _logger.Info("Item {ItemId} updated", item.Id);
            }
        }
    }
}
