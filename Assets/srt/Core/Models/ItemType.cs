namespace CookingGame.Core.Models
{
    /// <summary>
    /// 物品分类枚举
    /// 定义游戏中物品的不同类型
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// 原始食材
        /// 尚未处理的食材
        /// </summary>
        RawIngredient,
        
        /// <summary>
        /// 处理后食材
        /// 已经经过切割等处理的食材
        /// </summary>
        ProcessedIngredient,
        
        /// <summary>
        /// 菜品
        /// 已经制作完成的菜品
        /// </summary>
        FinishedDish,
        
        /// <summary>
        /// 垃圾
        /// 制作失败产生的垃圾
        /// </summary>
        Trash
    }
}
