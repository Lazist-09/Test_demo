using System.Collections.Generic;
using CookingGame.Core.Repositories;
using CookingGame.Core.Models;

namespace CookingGame.Infrastructure.Repositories
{
    /// <summary>
    /// 数据库初始化器
    /// 初始化默认的食谱和物品数据
    /// </summary>
    public class DatabaseInitializer
    {
        /// <summary>
        /// 食谱仓储
        /// </summary>
        private readonly IRecipeRepository _recipeRepository;
        
        /// <summary>
        /// 物品仓储
        /// </summary>
        private readonly IItemRepository _itemRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="recipeRepository">食谱仓储</param>
        /// <param name="itemRepository">物品仓储</param>
        public DatabaseInitializer(IRecipeRepository recipeRepository, IItemRepository itemRepository)
        {
            _recipeRepository = recipeRepository;
            _itemRepository = itemRepository;
        }

        /// <summary>
        /// 初始化默认数据
        /// </summary>
        public void InitializeDefaultData()
        {
            InitializeRecipes();
            InitializeDefaultItems();
        }

        /// <summary>
        /// 初始化默认食谱
        /// </summary>
        private void InitializeRecipes()
        {
            var recipes = new List<Recipe>
            {
                new Recipe(
                    "recipe_1",
                    "Chopped Carrot",  // 切片胡萝卜
                    new List<RecipeIngredient>
                    {
                        new RecipeIngredient("carrot", Shape.Slice, CookingStage.Raw)
                    },
                    50,
                    100
                ),
                new Recipe(
                    "recipe_2",
                    "Cooked Carrot",  // 烹饪胡萝卜
                    new List<RecipeIngredient>
                    {
                        new RecipeIngredient("carrot", Shape.Slice, CookingStage.Medium)
                    },
                    60,
                    100
                ),
                new Recipe(
                    "recipe_3",
                    "Julienne Carrot",  // 细丝胡萝卜
                    new List<RecipeIngredient>
                    {
                        new RecipeIngredient("carrot", Shape.Julienne, CookingStage.Raw)
                    },
                    70,
                    100
                ),
                new Recipe(
                    "recipe_4",
                    "Carrot Salad",  // 胡萝卜沙拉
                    new List<RecipeIngredient>
                    {
                        new RecipeIngredient("carrot", Shape.Slice, CookingStage.Raw),
                        new RecipeIngredient("lettuce", Shape.Slice, CookingStage.Raw)
                    },
                    80,
                    100
                )
            };

            foreach (var recipe in recipes)
            {
                _recipeRepository.Save(recipe);
            }
        }

        /// <summary>
        /// 初始化默认物品
        /// </summary>
        private void InitializeDefaultItems()
        {
            var defaultItems = new List<Item>
            {
                new Item("item_1", "carrot", ItemType.RawIngredient),    // 胡萝卜
                new Item("item_2", "lettuce", ItemType.RawIngredient),   // 生菜
                new Item("item_3", "tomato", ItemType.RawIngredient)     // 番茄
            };

            foreach (var item in defaultItems)
            {
                _itemRepository.Save(item);
            }
        }
    }
}
