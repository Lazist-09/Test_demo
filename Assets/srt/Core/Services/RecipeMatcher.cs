using System.Collections.Generic;
using System.Linq;
using CookingGame.Core.Repositories;
using CookingGame.Core.Models;

namespace CookingGame.Core.Services
{
    /// <summary>
    /// 配方匹配器
    /// 实现动态配方匹配算法
    /// 采用遍历查找，无序精准匹配
    /// </summary>
    public class RecipeMatcher : IRecipeMatcher
    {
        /// <summary>
        /// 食谱仓储
        /// </summary>
        private readonly IRecipeRepository _recipeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="recipeRepository">食谱仓储</param>
        public RecipeMatcher(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        /// <summary>
        /// 匹配配方
        /// 遍历所有配方，查找与给定食材匹配的配方
        /// </summary>
        /// <param name="items">待匹配的食材列表</param>
        /// <returns>匹配的配方，如果没有匹配则返回null</returns>
        public Recipe MatchRecipe(List<Item> items)
        {
            var allRecipes = _recipeRepository.GetAll();

            foreach (var recipe in allRecipes)
            {
                if (ValidateRecipe(recipe, items))
                {
                    return recipe;
                }
            }

            return null;
        }

        /// <summary>
        /// 验证配方
        /// 检查给定的食材是否符合配方的要求
        /// </summary>
        /// <param name="recipe">要验证的配方</param>
        /// <param name="items">待验证的食材列表</param>
        /// <returns>是否匹配</returns>
        public bool ValidateRecipe(Recipe recipe, List<Item> items)
        {
            // 数量必须匹配
            if (recipe.Ingredients.Count != items.Count)
            {
                return false;
            }

            var matchedItems = new bool[items.Count];
            var matchedIngredients = new bool[recipe.Ingredients.Count];

            // 遍历配方中的每个成分
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                // 遍历所有食材，寻找匹配项
                for (int j = 0; j < items.Count; j++)
                {
                    // 如果该食材或成分已经匹配，则跳过
                    if (matchedItems[j] || matchedIngredients[i])
                    {
                        continue;
                    }

                    var ingredient = recipe.Ingredients[i];
                    var item = items[j];

                    // 精准匹配：模板ID、形状、熟度都必须相同
                    if (ingredient.TemplateId == item.TemplateId &&
                        ingredient.RequiredShape == item.Shape &&
                        ingredient.RequiredStage == item.CookingStage)
                    {
                        matchedItems[j] = true;
                        matchedIngredients[i] = true;
                        break;
                    }
                }
            }

            // 所有成分都必须匹配
            return matchedIngredients.All(m => m);
        }

        /// <summary>
        /// 计算得分
        /// 根据食材与配方的匹配程度计算得分
        /// </summary>
        /// <param name="recipe">配方</param>
        /// <param name="items">食材列表</param>
        /// <returns>得分 (0-100分)</returns>
        public int CalculateScore(Recipe recipe, List<Item> items)
        {
            int totalScore = 0;
            int maxScore = recipe.MaxScore;

            // 计算每个成分的得分
            foreach (var ingredient in recipe.Ingredients)
            {
                var matchingItem = FindMatchingItem(ingredient, items);
                if (matchingItem != null)
                {
                    int itemScore = CalculateItemScore(ingredient, matchingItem);
                    totalScore += itemScore;
                }
            }

            return Mathf.Clamp(totalScore, recipe.MinScore, maxScore);
        }

        /// <summary>
        /// 查找匹配的食材
        /// </summary>
        /// <param name="ingredient">配方成分</param>
        /// <param name="items">食材列表</param>
        /// <returns>匹配的食材，如果不存在则返回null</returns>
        private Item FindMatchingItem(RecipeIngredient ingredient, List<Item> items)
        {
            foreach (var item in items)
            {
                if (ingredient.TemplateId == item.TemplateId &&
                    ingredient.RequiredShape == item.Shape &&
                    ingredient.RequiredStage == item.CookingStage)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 计算单个食材的得分
        /// </summary>
        /// <param name="ingredient">配方成分</param>
        /// <param name="item">食材</param>
        /// <returns>食材得分</returns>
        private int CalculateItemScore(RecipeIngredient ingredient, Item item)
        {
            int baseScore = 100;

            // 烧焦的食材得分为0
            if (item.CookingStage == CookingStage.Burnt)
            {
                return 0;
            }

            // 完美匹配：形状和熟度都符合要求
            if (item.Shape == ingredient.RequiredShape &&
                item.CookingStage == ingredient.RequiredStage)
            {
                return baseScore;
            }

            // 计算相似度
            float similarity = 0f;

            if (item.Shape == ingredient.RequiredShape)
            {
                similarity += 0.5f;  // 形状匹配
            }

            if (item.CookingStage == ingredient.RequiredStage)
            {
                similarity += 0.5f;  // 熟度匹配
            }

            return (int)(baseScore * similarity);
        }
    }
}
