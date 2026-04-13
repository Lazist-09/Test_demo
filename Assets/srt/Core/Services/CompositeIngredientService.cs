using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Services
{
    /// <summary>
    /// 复合食材服务
    /// 实现复合食材的创建、更新和验证逻辑
    /// </summary>
    public class CompositeIngredientService : ICompositeIngredientService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompositeIngredientService()
        {
        }

        /// <summary>
        /// 创建复合食材
        /// 在烹饪开始时创建，用于动态匹配配方
        /// </summary>
        /// <param name="id">复合食材ID</param>
        /// <param name="items">组成复合食材的食材列表</param>
        /// <returns>创建的复合食材</returns>
        public CompositeIngredient CreateCompositeIngredient(string id, List<Item> items)
        {
            var composite = new CompositeIngredient(id);

            foreach (var item in items)
            {
                composite.AddComponent(item.Id);
            }

            composite.SetCookingStage(CookingStage.Raw);
            return composite;
        }

        /// <summary>
        /// 更新复合食材
        /// 在烹饪过程中更新复合食材的进度
        /// </summary>
        /// <param name="composite">复合食材</param>
        /// <param name="progress">增加的进度</param>
        public void UpdateCompositeIngredient(CompositeIngredient composite, float progress)
        {
            composite.AddCookingProgress(progress);

            if (composite.CookingProgress >= 1.0f)
            {
                composite.SetCookingStage(CalculateFinalCookingStage(composite));
            }
        }

        /// <summary>
        /// 计算最终熟度
        /// 根据复合食材的组成计算最终熟度
        /// </summary>
        /// <param name="composite">复合食材</param>
        /// <returns>最终熟度</returns>
        private CookingStage CalculateFinalCookingStage(CompositeIngredient composite)
        {
            // 根据复合食材的组成计算最终熟度
            // 这里可以添加更复杂的逻辑，例如：
            // - 取所有组件食材的平均熟度
            // - 取最生的食材的熟度
            // - 根据烹饪时间动态计算
            
            // 简单实现：默认为WellDone
            return CookingStage.WellDone;
        }

        /// <summary>
        /// 验证复合食材
        /// 检查复合食材是否有效（有组件且未烧焦）
        /// </summary>
        /// <param name="composite">复合食材</param>
        /// <returns>是否有效</returns>
        public bool ValidateCompositeIngredient(CompositeIngredient composite)
        {
            return composite.IsValid;
        }
    }
}
