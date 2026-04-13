using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Services;
using CookingGame.Core.Configuration;
using CookingGame.Infrastructure;
using CookingGame.Application.UseCases;

namespace CookingGame.Infrastructure
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        private readonly GameConfig _config;
        private readonly ItemManagementUseCase _itemUseCase;
        private readonly RecipeManagementUseCase _recipeUseCase;
        private readonly OrderManagementUseCase _orderUseCase;
        private readonly CookingToolManagementUseCase _toolUseCase;
        private readonly OrderSubmissionUseCase _orderSubmissionUseCase;

        private ServiceLocator()
        {
            Debug.Log("ServiceLocator constructor called");
            
            // 确保DependencyContainer已经初始化
            if (DependencyContainer.GetItemRepository() == null)
            {
                Debug.Log("DependencyContainer not initialized, calling Initialize()");
                DependencyContainer.Initialize();
            }
            else
            {
                Debug.Log("DependencyContainer already initialized");
            }
            
            _config = new GameConfig();
            
            var itemRepo = DependencyContainer.GetItemRepository();
            var recipeRepo = DependencyContainer.GetRecipeRepository();
            var orderRepo = DependencyContainer.GetOrderRepository();
            var toolRepo = DependencyContainer.GetToolRepository();
            var recipeMatcher = DependencyContainer.GetRecipeMatcher();
            var itemValidator = DependencyContainer.GetItemValidator();
            var compositeService = DependencyContainer.GetCompositeIngredientService();
            
            Debug.Log($"ServiceLocator: itemRepo={itemRepo != null}, toolRepo={toolRepo != null}, recipeMatcher={recipeMatcher != null}");

            _itemUseCase = new ItemManagementUseCase(itemRepo, itemValidator);
            _recipeUseCase = new RecipeManagementUseCase(recipeRepo, recipeMatcher);
            _orderUseCase = new OrderManagementUseCase(orderRepo, recipeRepo);
            _toolUseCase = new CookingToolManagementUseCase(toolRepo, itemRepo, recipeMatcher, compositeService);
            _orderSubmissionUseCase = new OrderSubmissionUseCase(orderRepo, recipeRepo, itemRepo, recipeMatcher);
            
            Debug.Log("ServiceLocator initialized");
        }

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }
                return _instance;
            }
        }

        public GameConfig Config => _config;
        public ItemManagementUseCase ItemUseCase => _itemUseCase;
        public RecipeManagementUseCase RecipeUseCase => _recipeUseCase;
        public OrderManagementUseCase OrderUseCase => _orderUseCase;
        public CookingToolManagementUseCase ToolUseCase => _toolUseCase;
        public OrderSubmissionUseCase OrderSubmissionUseCase => _orderSubmissionUseCase;
    }
}
