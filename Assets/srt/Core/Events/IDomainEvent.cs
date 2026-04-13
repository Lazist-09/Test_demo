using System;

namespace CookingGame.Core.Events
{
    /// <summary>
    /// 领域事件接口
    /// 所有领域事件都应该实现此接口
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        Guid EventId { get; }
        
        /// <summary>
        /// 事件时间戳
        /// </summary>
        DateTime Timestamp { get; }
        
        /// <summary>
        /// 事件发生的时间（Unix时间戳）
        /// </summary>
        long TimestampTicks { get; }
    }
}
