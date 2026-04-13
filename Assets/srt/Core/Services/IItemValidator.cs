using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Services
{
    /// <summary>
    /// 物品验证服务接口
    /// 定义物品转换和验证的核心方法
    /// </summary>
    public interface IItemValidator
    {
        /// <summary>
        /// 验证物品转换
        /// 检查物品是否可以转换为指定形状
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="targetShape">目标形状</param>
        /// <returns>是否可以转换</returns>
        bool ValidateShapeConversion(Item item, Shape targetShape);

        /// <summary>
        /// 验证物品熟度
        /// 检查物品是否达到指定的熟度要求
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="requiredStage">要求的熟度阶段</param>
        /// <returns>是否符合要求</returns>
        bool ValidateCookingStage(Item item, CookingStage requiredStage);

        /// <summary>
        /// 验证复合食材
        /// 检查复合食材中的所有组件食材是否仍然存在
        /// </summary>
        /// <param name="composite">复合食材</param>
        /// <returns>是否有效</returns>
        bool ValidateCompositeIngredient(CompositeIngredient composite);

        /// <summary>
        /// 验证形状转换
        /// 检查形状转换是否合法（单向转换规则）
        /// </summary>
        /// <param name="currentShape">当前形状</param>
        /// <param name="newShape">新形状</param>
        /// <returns>转换是否合法</returns>
        bool ValidateShapeTransition(Shape currentShape, Shape newShape);

        /// <summary>
        /// 验证熟度转换
        /// 检查熟度转换是否合法
        /// </summary>
        /// <param name="currentStage">当前熟度</param>
        /// <param name="newStage">新熟度</param>
        /// <returns>转换是否合法</returns>
        bool ValidateCookingStageTransition(CookingStage currentStage, CookingStage newStage);
    }
}
