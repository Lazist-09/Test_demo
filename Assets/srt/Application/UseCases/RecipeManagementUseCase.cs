using System.Collections.Generic;
using System.Linq;
using CookingGame.Core.Repositories;
using CookingGame.Core.Services;
using CookingGame.Core.Models;
using CookingGame.Application.DTOs;

namespace CookingGame.Application.UseCases
{
    /// <summary>
    /// 食谱管理用例
    /// 处理食谱的创建、查询和匹配逻辑
    /// </summary>
    public class RecipeManagementUseCase
    {
        /// <summary>
        /// 食谱仓储
        /// </summary>
        private readonly IRecipeRepository _recipeRepository;
        
        /// <summary>
        /// 配方匹配器
        /// </summary>
        private readonly IRecipeMatcher _recipeMatcher;

        /// <summary>
        /// 构造函数
        /// 通过依赖注入获取所需的服务
        /// </summary>
        /// <param name="recipeRepository">食谱仓储</param>
        /// <param name="recipeMatcher">配方匹配器</param>
        public RecipeManagementUseCase(
            IRecipeRepository recipeRepository,
            IRecipeMatcher recipeMatcher)
        {
            _recipeRepository = recipeRepository;
            _recipeMatcher = recipeMatcher;
        }

        /// <summary>
        /// 创建食谱
        /// </summary>
        /// <param name="id">食谱ID</param>
        /// <param name="name">食谱名称</param>
        /// <param name="requiredItems">所需食材列表</param>
        /// <param name="reward">奖励分数</param>
        /// <returns>创建的食谱DTO</returns>
        public RecipeDto CreateRecipe(string id, string name, List<string> requiredItems, int reward)
        {
            var recipe = new Recipe(id, name, new List<RecipeIngredient>(), reward, reward * 2);
            
            foreach (var itemId in requiredItems)
            {
                recipe.AddRequiredItem(itemId);
            }
            
            _recipeRepository.Save(recipe);
            
            return RecipeDto.FromRecipe(recipe);
        }

        /// <summary>
        /// 获取所有食谱
        /// </summary>
        /// <returns>食谱DTO列表</returns>
        public List<RecipeDto> GetAllRecipes()
        {
            var recipes = _recipeRepository.GetAll();
            return recipes.Select(RecipeDto.FromRecipe).ToList();
        }

        /// <summary>
        /// 根据ID获取食谱
        /// </summary>
        /// <param name="id">食谱ID</param>
        /// <returns>食谱DTO，如果不存在则返回null</returns>
        public RecipeDto GetRecipeById(string id)
        {
            var recipe = _recipeRepository.GetById(id);
            return recipe != null ? RecipeDto.FromRecipe(recipe) : null;
        }

        /// <summary>
        /// 匹配方
        /// 根据提供的食材匹配合适的食谱
        /// </summary>
        /// <param name="items">提供的食材列表</param>
        /// <returns>匹配的食谱，如果未匹配则返回null</returns>
        public RecipeDto MatchRecipe(List<Item> items)
        {
            var matchedRecipe = _recipeMatcher.MatchRecipe(items);
            
            if (matchedRecipe != null)
            {
                return RecipeDto.FromRecipe(matchedRecipe);
            }
            
            return null;
        }

        /// <summary>
        /// 计算菜品得分
        /// 根据食谱要求和实际菜品计算得分
        /// </summary>
        /// <param name="recipe">食谱</param>
        /// <param name="items">菜品食材列表</param>
        /// <returns>得分（0-100）</returns>
        public int CalculateScore(Recipe recipe, List<Item> items)
        {
            return _recipeMatcher.CalculateScore(recipe, items);
        }

        /// <summary>
        /// 删除食谱
        /// </summary>
        /// <param name="id">食谱ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteRecipe(string id)
        {
            var recipe = _recipeRepository.GetById(id);
            if (recipe != null)
            {
                _recipeRepository.Remove(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新食谱
        /// </summary>
        /// <param name="recipe">要更新的食谱</param>
        public void UpdateRecipe(Recipe recipe)
        {
            if (recipe != null)
            {
                _recipeRepository.Update(recipe);
            }
        }
    }
}
