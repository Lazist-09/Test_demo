using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Services
{
    /// <summary>
    /// 配方匹配服务接口
    /// 定义动态匹配配方的核心方法
    /// </summary>
    public interface IRecipeMatcher
    {
        /// <summary>
        /// 匹配方
        /// 根据提供的食材匹配合适的食谱
        /// </summary>
        /// <param name="items">提供的食材列表</param>
        /// <returns>匹配的食谱，如果未匹配则返回null</returns>
        Recipe MatchRecipe(List<Item> items);

        /// <summary>
        /// 计算菜品得分
        /// 根据食谱要求和实际菜品计算得分
        /// </summary>
        /// <param name="recipe">食谱</param>
        /// <param name="items">菜品食材列表</param>
        /// <returns>得分（0-100）</returns>
        int CalculateScore(Recipe recipe, List<Item> items);
    }
}
