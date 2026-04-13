using System;
using CookingGame.Core.Models;

namespace CookingGame.Core.Events
{
    /// <summary>
    /// 事件数据基类
    /// 所有事件数据都应该继承此类
    /// </summary>
    public abstract class EventData
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType EventType { get; private set; }
        
        /// <summary>
        /// 事件时间戳
        /// </summary>
        public long Timestamp { get; protected set; }
        
        /// <summary>
        /// 事件ID
        /// </summary>
        public Guid EventId { get; protected set; }
        
        protected EventData(EventType eventType)
        {
            EventType = eventType;
            Timestamp = DateTime.Now.Ticks;
            EventId = Guid.NewGuid();
        }
    }
    
    /// <summary>
    /// 工具启动事件数据
    /// </summary>
    public class ToolStartedEventData : EventData
    {
        public CookingTool Tool { get; }
        
        public ToolStartedEventData(CookingTool tool)
            : base(EventType.ToolStarted)
        {
            Tool = tool ?? throw new ArgumentNullException(nameof(tool));
        }
    }
    
    /// <summary>
    /// 工具暂停事件数据
    /// </summary>
    public class ToolPausedEventData : EventData
    {
        public CookingTool Tool { get; }
        
        public ToolPausedEventData(CookingTool tool)
            : base(EventType.ToolPaused)
        {
            Tool = tool ?? throw new ArgumentNullException(nameof(tool));
        }
    }
    
    /// <summary>
    /// 工具完成事件数据
    /// </summary>
    public class ToolCompletedEventData : EventData
    {
        public CookingTool Tool { get; }
        
        public ToolCompletedEventData(CookingTool tool)
            : base(EventType.ToolCompleted)
        {
            Tool = tool ?? throw new ArgumentNullException(nameof(tool));
        }
    }
    
    /// <summary>
    /// 工具输出变化事件数据
    /// </summary>
    public class ToolOutputChangedEventData : EventData
    {
        public CookingTool Tool { get; }
        
        public ToolOutputChangedEventData(CookingTool tool)
            : base(EventType.ToolOutputChanged)
        {
            Tool = tool ?? throw new ArgumentNullException(nameof(tool));
        }
    }
    
    /// <summary>
    /// 工具输入变化事件数据
    /// </summary>
    public class ToolInputChangedEventData : EventData
    {
        public CookingTool Tool { get; }
        
        public ToolInputChangedEventData(CookingTool tool)
            : base(EventType.ToolInputChanged)
        {
            Tool = tool ?? throw new ArgumentNullException(nameof(tool));
        }
    }
    
    /// <summary>
    /// 菜品创建事件数据
    /// </summary>
    public class DishCreatedEventData : EventData
    {
        public Item Dish { get; }
        
        public DishCreatedEventData(Item dish)
            : base(EventType.DishCreated)
        {
            Dish = dish ?? throw new ArgumentNullException(nameof(dish));
        }
    }
    
    /// <summary>
    /// 菜品更新事件数据
    /// </summary>
    public class DishUpdatedEventData : EventData
    {
        public Item Dish { get; }
        
        public DishUpdatedEventData(Item dish)
            : base(EventType.DishUpdated)
        {
            Dish = dish ?? throw new ArgumentNullException(nameof(dish));
        }
    }
    
    /// <summary>
    /// 菜品删除事件数据
    /// </summary>
    public class DishDeletedEventData : EventData
    {
        public Item Dish { get; }
        
        public DishDeletedEventData(Item dish)
            : base(EventType.DishDeleted)
        {
            Dish = dish ?? throw new ArgumentNullException(nameof(dish));
        }
    }
    
    /// <summary>
    /// 订单创建事件数据
    /// </summary>
    public class OrderCreatedEventData : EventData
    {
        public Order Order { get; }
        
        public OrderCreatedEventData(Order order)
            : base(EventType.OrderCreated)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
        }
    }
    
    /// <summary>
    /// 订单提交事件数据
    /// </summary>
    public class OrderSubmittedEventData : EventData
    {
        public Order Order { get; }
        
        public OrderSubmittedEventData(Order order)
            : base(EventType.OrderSubmitted)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
        }
    }
    
    /// <summary>
    /// 订单完成事件数据
    /// </summary>
    public class OrderCompletedEventData : EventData
    {
        public Order Order { get; }
        
        public OrderCompletedEventData(Order order)
            : base(EventType.OrderCompleted)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
        }
    }
    
    /// <summary>
    /// 订单状态变化事件数据
    /// </summary>
    public class OrderStatusChangedEventData : EventData
    {
        public Order Order { get; }
        public OrderStatus OldStatus { get; }
        public OrderStatus NewStatus { get; }
        
        public OrderStatusChangedEventData(Order order, OrderStatus oldStatus, OrderStatus newStatus)
            : base(EventType.OrderStatusChanged)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
    
    /// <summary>
    /// 状态变化事件数据
    /// </summary>
    public class StateChangedEventData : EventData
    {
        public string StateName { get; }
        public object NewValue { get; }
        
        public StateChangedEventData(string stateName, object newValue)
            : base(EventType.StateChanged)
        {
            StateName = stateName ?? throw new ArgumentNullException(nameof(stateName));
            NewValue = newValue;
        }
    }
}
