using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Services
{
    /// <summary>
    /// 物品验证器
    /// 实现物品转换和验证的核心逻辑
    /// </summary>
    public class ItemValidator : IItemValidator
    {
        /// <summary>
        /// 有效的形状转换字典
        /// 定义单向转换规则：Whole -> Chunk -> Slice -> Julienne -> Crumbled
        /// </summary>
        private static readonly Dictionary<Shape, List<Shape>> _validShapeTransitions = new Dictionary<Shape, List<Shape>>
        {
            { Shape.Whole, new List<Shape> { Shape.Chunk } },
            { Shape.Chunk, new List<Shape> { Shape.Slice } },
            { Shape.Slice, new List<Shape> { Shape.Julienne } },
            { Shape.Julienne, new List<Shape> { Shape.Crumbled } },
            { Shape.Crumbled, new List<Shape>() }
        };

        /// <summary>
        /// 有效的熟度转换字典
        /// 定义单向转换规则：Raw -> Medium -> WellDone -> Burnt
        /// </summary>
        private static readonly Dictionary<CookingStage, List<CookingStage>> _validCookingStageTransitions = new Dictionary<CookingStage, List<CookingStage>>
        {
            { CookingStage.Raw, new List<CookingStage> { CookingStage.Medium, CookingStage.WellDone, CookingStage.Burnt } },
            { CookingStage.Medium, new List<CookingStage> { CookingStage.WellDone, CookingStage.Burnt } },
            { CookingStage.WellDone, new List<CookingStage> { CookingStage.Burnt } },
            { CookingStage.Burnt, new List<CookingStage>() }
        };

        /// <summary>
        /// 验证物品转换
        /// 检查物品是否可以转换为指定形状
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="targetShape">目标形状</param>
        /// <returns>是否可以转换</returns>
        public bool ValidateShapeConversion(Item item, Shape targetShape)
        {
            if (item == null) return false;
            return ValidateShapeTransition(item.Shape, targetShape);
        }

        /// <summary>
        /// 验证物品熟度
        /// 检查物品是否达到指定的熟度要求
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="requiredStage">要求的熟度阶段</param>
        /// <returns>是否符合要求</returns>
        public bool ValidateCookingStage(Item item, CookingStage requiredStage)
        {
            if (item == null) return false;
            return item.CookingStage == requiredStage;
        }

        /// <summary>
        /// 验证复合食材
        /// 检查复合食材中的所有组件食材是否仍然存在
        /// </summary>
        /// <param name="composite">复合食材</param>
        /// <returns>是否有效</returns>
        public bool ValidateCompositeIngredient(CompositeIngredient composite)
        {
            if (composite == null) return false;
            return composite.IsValid;
        }

        /// <summary>
        /// 验证形状转换
        /// 检查从一个形状转换到另一个形状是否合法
        /// </summary>
        /// <param name="fromShape">原始形状</param>
        /// <param name="toShape">目标形状</param>
        /// <returns>转换是否合法</returns>
        public bool ValidateShapeTransition(Shape fromShape, Shape toShape)
        {
            if (_validShapeTransitions.TryGetValue(fromShape, out var validTransitions))
            {
                return validTransitions.Contains(toShape);
            }
            return false;
        }

        /// <summary>
        /// 验证熟度转换
        /// 检查从一个熟度转换到另一个熟度是否合法
        /// </summary>
        /// <param name="fromStage">原始熟度</param>
        /// <param name="toStage">目标熟度</param>
        /// <returns>转换是否合法</returns>
        public bool ValidateCookingStageTransition(CookingStage fromStage, CookingStage toStage)
        {
            if (_validCookingStageTransitions.TryGetValue(fromStage, out var validTransitions))
            {
                return validTransitions.Contains(toStage);
            }
            return false;
        }

        /// <summary>
        /// 验证物品是否适合特定工具
        /// 检查物品是否可以放入指定类型的烹饪工具中
        /// </summary>
        /// <param name="item">待验证的物品</param>
        /// <param name="toolType">工具类型</param>
        /// <returns>是否适合</returns>
        public bool ValidateItemForTool(Item item, CookingToolType toolType)
        {
            switch (toolType)
            {
                case CookingToolType.Knife:
                    return item.Category == ItemType.RawIngredient || item.Category == ItemType.ProcessedIngredient;
                case CookingToolType.Pan:
                case CookingToolType.Pot:
                case CookingToolType.Oven:
                    return item.Category == ItemType.RawIngredient || item.Category == ItemType.ProcessedIngredient;
                case CookingToolType.Mixer:
                    return item.Category == ItemType.RawIngredient || item.Category == ItemType.ProcessedIngredient || item.Category == ItemType.FinishedDish;
                default:
                    return false;
            }
        }
    }
}
