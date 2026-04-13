using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Services;
using CookingGame.Application.UseCases;
using CookingGame.Infrastructure.Repositories;
using CookingGame.Infrastructure.Logging;

namespace CookingGame.Infrastructure
{
    /// <summary>
    /// 依赖注入容器
    /// 管理应用程序的所有依赖项
    /// 实现控制反转(IoC)模式
    /// </summary>
    public static class DependencyContainer
    {
        /// <summary>
        /// 物品仓储实例
        /// </summary>
        private static IItemRepository _itemRepository;
        
        /// <summary>
        /// 食谱仓储实例
        /// </summary>
        private static IRecipeRepository _recipeRepository;
        
        /// <summary>
        /// 订单仓储实例
        /// </summary>
        private static IOrderRepository _orderRepository;
        
        /// <summary>
        /// 烹饪工具仓储实例
        /// </summary>
        private static ICookingToolRepository _toolRepository;
        
        /// <summary>
        /// 日志器实例
        /// </summary>
        private static Core.Logging.ILogger _logger;

        /// <summary>
        /// 初始化依赖注入容器
        /// 创建所有仓储和服务的实例
        /// </summary>
        public static void Initialize()
        {
            Debug.Log("DependencyContainer.Initialize() called");
            
            _itemRepository = new InMemoryItemRepository();
            _recipeRepository = new InMemoryRecipeRepository();
            _orderRepository = new InMemoryOrderRepository();
            _toolRepository = new InMemoryCookingToolRepository();
            _logger = new DebugLogger();
            
            // 初始化默认数据
            InitializeDefaultData();
            
            Debug.Log("DependencyContainer.Initialize() completed");
        }

        /// <summary>
        /// 初始化默认数据
        /// </summary>
        private static void InitializeDefaultData()
        {
            var initializer = new DatabaseInitializer(_recipeRepository, _itemRepository);
            initializer.InitializeDefaultData();
        }

        /// <summary>
        /// 获取物品仓储
        /// </summary>
        /// <returns>物品仓储实例</returns>
        public static IItemRepository GetItemRepository()
        {
            var result = _itemRepository;
            return result;
        }

        /// <summary>
        /// 获取食谱仓储
        /// </summary>
        /// <returns>食谱仓储实例</returns>
        public static IRecipeRepository GetRecipeRepository()
        {
            return _recipeRepository;
        }

        /// <summary>
        /// 获取订单仓储
        /// </summary>
        /// <returns>订单仓储实例</returns>
        public static IOrderRepository GetOrderRepository()
        {
            return _orderRepository;
        }

        /// <summary>
        /// 获取烹饪工具仓储
        /// </summary>
        /// <returns>烹饪工具仓储实例</returns>
        public static ICookingToolRepository GetToolRepository()
        {
            return _toolRepository;
        }

        /// <summary>
        /// 获取配方匹配器
        /// </summary>
        /// <returns>配方匹配器实例</returns>
        public static IRecipeMatcher GetRecipeMatcher()
        {
            return new RecipeMatcher(GetRecipeRepository());
        }

        /// <summary>
        /// 获取物品验证器
        /// </summary>
        /// <returns>物品验证器实例</returns>
        public static IItemValidator GetItemValidator()
        {
            return new ItemValidator();
        }

        /// <summary>
        /// 获取复合食材服务
        /// </summary>
        /// <returns>复合食材服务实例</returns>
        public static ICompositeIngredientService GetCompositeIngredientService()
        {
            return new CompositeIngredientService();
        }

        /// <summary>
        /// 获取物品管理用例
        /// </summary>
        /// <returns>物品管理用例实例</returns>
        public static ItemManagementUseCase GetItemManagementUseCase()
        {
            return new ItemManagementUseCase(
                GetItemRepository(),
                GetItemValidator());
        }

        /// <summary>
        /// 获取食谱管理用例
        /// </summary>
        /// <returns>食谱管理用例实例</returns>
        public static RecipeManagementUseCase GetRecipeManagementUseCase()
        {
            return new RecipeManagementUseCase(
                GetRecipeRepository(),
                GetRecipeMatcher());
        }

        /// <summary>
        /// 获取订单管理用例
        /// </summary>
        /// <returns>订单管理用例实例</returns>
        public static OrderManagementUseCase GetOrderManagementUseCase()
        {
            return new OrderManagementUseCase(
                GetOrderRepository(),
                GetRecipeRepository());
        }

        /// <summary>
        /// 获取订单提交用例
        /// </summary>
        /// <returns>订单提交用例实例</returns>
        public static OrderSubmissionUseCase GetOrderSubmissionUseCase()
        {
            return new OrderSubmissionUseCase(
                GetOrderRepository(),
                GetRecipeRepository(),
                GetItemRepository(),
                GetRecipeMatcher());
        }

        /// <summary>
        /// 获取烹饪工具管理用例
        /// </summary>
        /// <returns>烹饪工具管理用例实例</returns>
        public static CookingToolManagementUseCase GetCookingToolManagementUseCase()
        {
            return new CookingToolManagementUseCase(
                GetToolRepository(),
                GetItemRepository(),
                GetRecipeMatcher(),
                GetCompositeIngredientService(),
                GetLogger());
        }

        /// <summary>
        /// 获取日志器
        /// </summary>
        /// <returns>日志器实例</returns>
        public static Core.Logging.ILogger GetLogger()
        {
            return _logger;
        }
    }
}
