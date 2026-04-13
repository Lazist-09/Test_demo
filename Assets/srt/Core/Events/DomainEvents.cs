using System;
using CookingGame.Core.Models;

namespace CookingGame.Core.Events
{
    /// <summary>
    /// 领域事件中心
    /// 管理领域事件的发布和订阅
    /// </summary>
    public static class DomainEvents
    {
        #region 字段
        
        /// <summary>
        /// 事件总线实例
        /// </summary>
        private static IEventBus _eventBus;
        
        /// <summary>
        /// 事件总线实例
        /// </summary>
        public static IEventBus EventBus => _eventBus ??= new EventBus();
        
        #endregion
        
        #region 领域事件订阅方法
        
        /// <summary>
        /// 订阅领域事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="handler">事件处理器</param>
        public static void Subscribe<T>(Action<T> handler) where T : DomainEvent
        {
            EventBus.Subscribe(new DomainEventHandler<T>(handler));
        }
        
        /// <summary>
        /// 取消订阅领域事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="handler">事件处理器</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : DomainEvent
        {
            EventBus.Unsubscribe(new DomainEventHandler<T>(handler));
        }
        
        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="event">领域事件</param>
        public static void Publish<T>(T @event) where T : DomainEvent
        {
            EventBus.Publish(@event);
        }
        
        #endregion
        
        #region 内部类 - 领域事件处理器包装器
        
        /// <summary>
        /// 领域事件处理器包装器
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        private class DomainEventHandler<T> : IEventHandler<T> where T : DomainEvent
        {
            private readonly Action<T> _handler;
            
            public DomainEventHandler(Action<T> handler)
            {
                _handler = handler;
            }
            
            public void Handle(T eventData)
            {
                _handler(eventData);
            }
        }
        
        #endregion
    }
    
    /// <summary>
    /// 领域事件基类
    /// 所有领域事件都应该继承此类
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public Guid EventId { get; private set; }
        
        /// <summary>
        /// 事件时间戳
        /// </summary>
        public DateTime Timestamp { get; private set; }
        
        /// <summary>
        /// 事件发生的时间（Unix时间戳）
        /// </summary>
        public long TimestampTicks { get; private set; }
        
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            Timestamp = DateTime.Now;
            TimestampTicks = DateTime.Now.Ticks;
        }
    }
    
    /// <summary>
    /// 物品创建领域事件
    /// </summary>
    public class ItemCreatedDomainEvent : DomainEvent
    {
        public Item Item { get; }
        
        public ItemCreatedDomainEvent(Item item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
    
    /// <summary>
    /// 物品更新领域事件
    /// </summary>
    public class ItemUpdatedDomainEvent : DomainEvent
    {
        public Item Item { get; }
        
        public ItemUpdatedDomainEvent(Item item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
    }
    
    /// <summary>
    /// 物品删除领域事件
    /// </summary>
    public class ItemDeletedDomainEvent : DomainEvent
    {
        public string ItemId { get; }
        public string ItemName { get; }
        
        public ItemDeletedDomainEvent(string itemId, string itemName)
        {
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
            ItemName = itemName ?? throw new ArgumentNullException(nameof(itemName));
        }
    }
    
    /// <summary>
    /// Trash创建领域事件
    /// </summary>
    public class TrashCreatedDomainEvent : DomainEvent
    {
        public Item TrashItem { get; }
        
        public TrashCreatedDomainEvent(Item trashItem)
        {
            TrashItem = trashItem ?? throw new ArgumentNullException(nameof(trashItem));
        }
    }
    
    /// <summary>
    /// Trash合并领域事件
    /// </summary>
    public class TrashMergedDomainEvent : DomainEvent
    {
        public string OriginalTrashId { get; }
        public string MergedTrashId { get; }
        public int NewCount { get; }
        
        public TrashMergedDomainEvent(string originalTrashId, string mergedTrashId, int newCount)
        {
            OriginalTrashId = originalTrashId ?? throw new ArgumentNullException(nameof(originalTrashId));
            MergedTrashId = mergedTrashId ?? throw new ArgumentNullException(nameof(mergedTrashId));
            NewCount = newCount;
        }
    }
    
    /// <summary>
    /// 烹饪失败领域事件
    /// </summary>
    public class CookingFailedDomainEvent : DomainEvent
    {
        public string ToolId { get; }
        public int Penalty { get; }
        
        public CookingFailedDomainEvent(string toolId, int penalty)
        {
            ToolId = toolId ?? throw new ArgumentNullException(nameof(toolId));
            Penalty = penalty;
        }
    }
    
    /// <summary>
    /// 烹饪成功领域事件
    /// </summary>
    public class CookingSuccessDomainEvent : DomainEvent
    {
        public string ToolId { get; }
        public string DishId { get; }
        public int Score { get; }
        
        public CookingSuccessDomainEvent(string toolId, string dishId, int score)
        {
            ToolId = toolId ?? throw new ArgumentNullException(nameof(toolId));
            DishId = dishId ?? throw new ArgumentNullException(nameof(dishId));
            Score = score;
        }
    }
}
