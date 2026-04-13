namespace CookingGame.Core.Models
{
    /// <summary>
    /// 烹饪工具状态枚举
    /// 定义烹饪工具的状态机
    /// </summary>
    public enum CookingToolState
    {
        Idle,       // 空闲 - 可以添加食材
        Preparing,  // 准备中 - 已添加食材，等待启动
        Cooking,    // 烹饪中 - 正在处理
        Paused,     // 暂停 - 进度暂停
        Finished,   // 完成 - 可以取出输出
        Discarded   // 丢弃 - 超时或错误
    }
}
