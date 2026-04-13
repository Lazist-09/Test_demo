namespace CookingGame.Core.Models
{
    /// <summary>
    /// 物品状态枚举
    /// 定义物品在游戏中的当前状态
    /// </summary>
    public enum ItemStatus
    {
        Inactive,   // 未激活（工具未启动）
        Processing, // 处理中
        Ready,      // 就绪（可取出）
        Discarded   // 已丢弃
    }
}
