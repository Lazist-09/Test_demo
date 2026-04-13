using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;
using CookingGame.Infrastructure;
using CookingGame.Presentation.UI;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 厨房 UI 核心协调器
    /// 协调各个 UI 模块，处理全局状态
    /// </summary>
    public class KitchenUIController : MonoBehaviour
    {
        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            // 初始化依赖注入容器
            DependencyContainer.Initialize();
            
            // 确保ServiceLocator被初始化
            var _ = ServiceLocator.Instance;
            
            // 初始化各个管理器
            InitializeManagers();
        }

        /// <summary>
        /// 初始化管理器
        /// </summary>
        private void InitializeManagers()
        {
            // 从 ServiceLocator 获取用例
            var itemUseCase = ServiceLocator.Instance.ItemUseCase;
            var recipeUseCase = ServiceLocator.Instance.RecipeUseCase;
            var orderUseCase = ServiceLocator.Instance.OrderUseCase;
            var toolUseCase = ServiceLocator.Instance.ToolUseCase;

            // 初始化所有挂载在 Container 上的 Manager
            InitializeManagersWithUseCases(itemUseCase, recipeUseCase, orderUseCase, toolUseCase);
        }

        /// <summary>
        /// 使用用例初始化管理器
        /// </summary>
        private void InitializeManagersWithUseCases(
            ItemManagementUseCase itemUseCase,
            RecipeManagementUseCase recipeUseCase,
            OrderManagementUseCase orderUseCase,
            CookingToolManagementUseCase toolUseCase)
        {
            // 获取所有 Manager 组件
            var ingredientManagers = FindObjectsOfType<IngredientManager>();
            foreach (var manager in ingredientManagers)
            {
                manager.Initialize(itemUseCase, recipeUseCase);
            }

            var toolManagers = FindObjectsOfType<CookingToolManager>();
            foreach (var manager in toolManagers)
            {
                manager.Initialize(toolUseCase);
            }

            var orderManagers = FindObjectsOfType<OrderManager>();
            foreach (var manager in orderManagers)
            {
                manager.Initialize(orderUseCase, toolUseCase);
            }

            var dishOutputManagers = FindObjectsOfType<DishOutputManager>();
            foreach (var manager in dishOutputManagers)
            {
                manager.Initialize(toolUseCase);
            }
        }

        /// <summary>
        /// 更新所有管理器
        /// </summary>
        private void Update()
        {
            // 更新所有管理器
            var ingredientManagers = FindObjectsOfType<IngredientManager>();
            foreach (var manager in ingredientManagers)
            {
                manager.Update();
            }

            var toolManagers = FindObjectsOfType<CookingToolManager>();
            foreach (var manager in toolManagers)
            {
                manager.Update();
            }

            var orderManagers = FindObjectsOfType<OrderManager>();
            foreach (var manager in orderManagers)
            {
                manager.Update();
            }

            var dishOutputManagers = FindObjectsOfType<DishOutputManager>();
            foreach (var manager in dishOutputManagers)
            {
                manager.Update();
            }
        }

        /// <summary>
        /// 切换到主菜单
        /// </summary>
        public void GoToMenu()
        {
            UIManager.Instance?.ShowMenu();
        }

        /// <summary>
        /// 切换到厨房
        /// </summary>
        public void GoToKitchen()
        {
            UIManager.Instance?.ShowKitchen();
        }
    }
}
