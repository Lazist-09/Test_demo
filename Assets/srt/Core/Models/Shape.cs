namespace CookingGame.Core.Models
{
    /// <summary>
    /// 形状枚举
    /// 定义食材的切割形状，支持单向转换
    /// 转换顺序：Whole -> Chunk -> Slice -> Julienne -> Crumbled
    /// </summary>
    public enum Shape
    {
        Whole,       // 整体
        Chunk,       // 块状
        Slice,       // 片状
        Julienne,    // 细丝状
        Crumbled     // 碎状
    }
}
