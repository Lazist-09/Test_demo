using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Services
{
    /// <summary>
    /// 复合食材服务接口
    /// 定义复合食材的创建、更新和验证方法
    /// </summary>
    public interface ICompositeIngredientService
    {
        /// <summary>
        /// 创建复合食材
        /// 在烹饪开始时创建，用于动态匹配配方
        /// </summary>
        /// <param name="id">复合食材ID</param>
        /// <param name="items">组成复合食材的食材列表</param>
        /// <returns>创建的复合食材</returns>
        CompositeIngredient CreateCompositeIngredient(string id, List<Item> items);

        /// <summary>
        /// 更新复合食材
        /// 在烹饪过程中更新复合食材的进度
        /// </summary>
        /// <param name="composite">复合食材</param>
        /// <param name="progress">增加的进度</param>
        void UpdateCompositeIngredient(CompositeIngredient composite, float progress);

        /// <summary>
        /// 验证复合食材
        /// 检查复合食材中的所有组件食材是否仍然存在
        /// </summary>
        /// <param name="composite">复合食材</param>
        /// <returns>是否有效</returns>
        bool ValidateCompositeIngredient(CompositeIngredient composite);
    }
}
