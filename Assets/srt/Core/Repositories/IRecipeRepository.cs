using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Repositories
{
    /// <summary>
    /// 食谱仓储接口
    /// 定义食谱的持久化操作
    /// </summary>
    public interface IRecipeRepository
    {
        /// <summary>
        /// 保存食谱
        /// 如果食谱没有ID则生成新的ID
        /// </summary>
        /// <param name="recipe">要保存的食谱</param>
        void Save(Recipe recipe);

        /// <summary>
        /// 根据ID获取食谱
        /// </summary>
        /// <param name="id">食谱ID</param>
        /// <returns>找到的食谱，如果不存在则返回null</returns>
        Recipe GetById(string id);

        /// <summary>
        /// 获取所有食谱
        /// </summary>
        /// <returns>所有食谱的列表</returns>
        List<Recipe> GetAll();

        /// <summary>
        /// 根据食材模板ID获取食谱
        /// 查找包含指定食材的所有食谱
        /// </summary>
        /// <param name="templateId">食材模板ID</param>
        /// <returns>包含该食材的食谱列表</returns>
        List<Recipe> GetByIngredient(string templateId);

        /// <summary>
        /// 根据ID删除食谱
        /// </summary>
        /// <param name="id">食谱ID</param>
        void Remove(string id);

        /// <summary>
        /// 更新食谱
        /// </summary>
        /// <param name="recipe">要更新的食谱</param>
        void Update(Recipe recipe);
    }
}
