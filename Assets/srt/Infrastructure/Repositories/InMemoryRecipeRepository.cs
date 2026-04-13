using System.Collections.Generic;
using System.Linq;
using CookingGame.Core.Repositories;
using CookingGame.Core.Models;

namespace CookingGame.Infrastructure.Repositories
{
    /// <summary>
    /// 内存食谱仓储
    /// 使用内存字典存储食谱数据，用于开发和测试
    /// </summary>
    public class InMemoryRecipeRepository : IRecipeRepository
    {
        /// <summary>
        /// 食谱存储字典
        /// </summary>
        private readonly Dictionary<string, Recipe> _recipes = new Dictionary<string, Recipe>();
        
        /// <summary>
        /// ID计数器
        /// 用于生成唯一ID
        /// </summary>
        private int _idCounter = 0;

        /// <summary>
        /// 保存食谱
        /// 如果食谱没有ID则生成新的ID
        /// </summary>
        /// <param name="recipe">要保存的食谱</param>
        public void Save(Recipe recipe)
        {
            if (string.IsNullOrEmpty(recipe.Id))
            {
                recipe.Id = GenerateId();
            }
            _recipes[recipe.Id] = recipe;
        }

        /// <summary>
        /// 根据ID获取食谱
        /// </summary>
        /// <param name="id">食谱ID</param>
        /// <returns>找到的食谱，如果不存在则返回null</returns>
        public Recipe GetById(string id)
        {
            return _recipes.TryGetValue(id, out var recipe) ? recipe : null;
        }

        /// <summary>
        /// 获取所有食谱
        /// </summary>
        /// <returns>所有食谱的列表</returns>
        public List<Recipe> GetAll()
        {
            return new List<Recipe>(_recipes.Values);
        }

        /// <summary>
        /// 根据食材模板ID获取食谱
        /// 查找包含指定食材的所有食谱
        /// </summary>
        /// <param name="templateId">食材模板ID</param>
        /// <returns>包含该食材的食谱列表</returns>
        public List<Recipe> GetByIngredient(string templateId)
        {
            var result = new List<Recipe>();
            foreach (var recipe in _recipes.Values)
            {
                if (recipe.Ingredients.Any(i => i.TemplateId == templateId))
                {
                    result.Add(recipe);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据ID删除食谱
        /// </summary>
        /// <param name="id">食谱ID</param>
        public void Remove(string id)
        {
            _recipes.Remove(id);
        }

        /// <summary>
        /// 更新食谱
        /// </summary>
        /// <param name="recipe">要更新的食谱</param>
        public void Update(Recipe recipe)
        {
            if (_recipes.ContainsKey(recipe.Id))
            {
                _recipes[recipe.Id] = recipe;
            }
        }

        /// <summary>
        /// 生成唯一ID
        /// </summary>
        /// <returns>生成的ID</returns>
        private string GenerateId()
        {
            return $"recipe_{++_idCounter}";
        }
    }
}
