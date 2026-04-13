using System.Collections.Generic;

namespace CookingGame.Core.Models
{
    /// <summary>
    /// 订单状态枚举
    /// 定义订单的生命周期状态
    /// </summary>
    public enum OrderStatus
    {
        Pending,    // 待处理 (等待提交)
        Submitted,  // 已提交 (已提交但未完成)
        Completed   // 已完成 (已验收)
    }
}
