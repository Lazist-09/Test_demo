using System;
using CookingGame.Core.Models;
using CookingGame.Core.Events;

namespace CookingGame.Core.Events
{
    /// <summary>
    /// 游戏事件中心
    /// 使用事件总线管理游戏中的各种事件
    /// 提供向后兼容的静态事件接口
    /// </summary>
    public static class GameEvents
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
        
        #region 烹饪工具事件
        
        /// <summary>
        /// 工具启动事件
        /// 当工具开始烹饪时触发
        /// </summary>
        public static event Action<CookingTool> ToolStarted
        {
            add { EventBus.Subscribe(new ToolStartedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new ToolStartedEventHandler(value)); }
        }
        
        /// <summary>
        /// 工具暂停事件
        /// 当工具暂停烹饪时触发
        /// </summary>
        public static event Action<CookingTool> ToolPaused
        {
            add { EventBus.Subscribe(new ToolPausedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new ToolPausedEventHandler(value)); }
        }
        
        /// <summary>
        /// 工具完成事件
        /// 当工具完成烹饪时触发
        /// </summary>
        public static event Action<CookingTool> ToolCompleted
        {
            add { EventBus.Subscribe(new ToolCompletedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new ToolCompletedEventHandler(value)); }
        }
        
        /// <summary>
        /// 工具输出事件
        /// 当工具产生输出时触发
        /// </summary>
        public static event Action<CookingTool> ToolOutputChanged
        {
            add { EventBus.Subscribe(new ToolOutputChangedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new ToolOutputChangedEventHandler(value)); }
        }
        
        /// <summary>
        /// 工具输入变化事件
        /// 当工具的输入食材变化时触发
        /// </summary>
        public static event Action<CookingTool> ToolInputChanged
        {
            add { EventBus.Subscribe(new ToolInputChangedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new ToolInputChangedEventHandler(value)); }
        }
        
        #endregion
        
        #region 物品事件
        
        /// <summary>
        /// 菜品创建事件
        /// 当新菜品被创建时触发
        /// </summary>
        public static event Action<Item> DishCreated
        {
            add { EventBus.Subscribe(new DishCreatedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new DishCreatedEventHandler(value)); }
        }
        
        /// <summary>
        /// 菜品更新事件
        /// 当菜品被更新时触发
        /// </summary>
        public static event Action<Item> DishUpdated
        {
            add { EventBus.Subscribe(new DishUpdatedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new DishUpdatedEventHandler(value)); }
        }
        
        /// <summary>
        /// 菜品删除事件
        /// 当菜品被删除时触发
        /// </summary>
        public static event Action<Item> DishDeleted
        {
            add { EventBus.Subscribe(new DishDeletedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new DishDeletedEventHandler(value)); }
        }
        
        #endregion
        
        #region 订单事件
        
        /// <summary>
        /// 订单创建事件
        /// 当新订单被创建时触发
        /// </summary>
        public static event Action<Order> OrderCreated
        {
            add { EventBus.Subscribe(new OrderCreatedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new OrderCreatedEventHandler(value)); }
        }
        
        /// <summary>
        /// 订单提交事件
        /// 当订单被提交时触发
        /// </summary>
        public static event Action<Order> OrderSubmitted
        {
            add { EventBus.Subscribe(new OrderSubmittedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new OrderSubmittedEventHandler(value)); }
        }
        
        /// <summary>
        /// 订单完成事件
        /// 当订单被完成时触发
        /// </summary>
        public static event Action<Order> OrderCompleted
        {
            add { EventBus.Subscribe(new OrderCompletedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new OrderCompletedEventHandler(value)); }
        }
        
        /// <summary>
        /// 订单状态变化事件
        /// 当订单状态变化时触发
        /// </summary>
        public static event Action<Order> OrderStatusChanged
        {
            add { EventBus.Subscribe(new OrderStatusChangedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new OrderStatusChangedEventHandler(value)); }
        }
        
        #endregion
        
        #region 状态变化事件
        
        /// <summary>
        /// 状态变化通用事件
        /// </summary>
        public static event Action<string, object> StateChanged
        {
            add { EventBus.Subscribe(new StateChangedEventHandler(value)); }
            remove { EventBus.Unsubscribe(new StateChangedEventHandler(value)); }
        }
        
        #endregion
        
        #region 事件触发方法
        
        /// <summary>
        /// 触发工具启动事件
        /// </summary>
        public static void InvokeToolStarted(CookingTool tool)
        {
            EventBus.Publish(new ToolStartedEventData(tool));
        }
        
        /// <summary>
        /// 触发工具暂停事件
        /// </summary>
        public static void InvokeToolPaused(CookingTool tool)
        {
            EventBus.Publish(new ToolPausedEventData(tool));
        }
        
        /// <summary>
        /// 触发工具完成事件
        /// </summary>
        public static void InvokeToolCompleted(CookingTool tool)
        {
            EventBus.Publish(new ToolCompletedEventData(tool));
        }
        
        /// <summary>
        /// 触发工具输出变化事件
        /// </summary>
        public static void InvokeToolOutputChanged(CookingTool tool)
        {
            EventBus.Publish(new ToolOutputChangedEventData(tool));
        }
        
        /// <summary>
        /// 触发工具输入变化事件
        /// </summary>
        public static void InvokeToolInputChanged(CookingTool tool)
        {
            EventBus.Publish(new ToolInputChangedEventData(tool));
        }
        
        /// <summary>
        /// 触发菜品创建事件
        /// </summary>
        public static void InvokeDishCreated(Item item)
        {
            EventBus.Publish(new DishCreatedEventData(item));
        }
        
        /// <summary>
        /// 触发菜品更新事件
        /// </summary>
        public static void InvokeDishUpdated(Item item)
        {
            EventBus.Publish(new DishUpdatedEventData(item));
        }
        
        /// <summary>
        /// 触发菜品删除事件
        /// </summary>
        public static void InvokeDishDeleted(Item item)
        {
            EventBus.Publish(new DishDeletedEventData(item));
        }
        
        /// <summary>
        /// 触发订单创建事件
        /// </summary>
        public static void InvokeOrderCreated(Order order)
        {
            EventBus.Publish(new OrderCreatedEventData(order));
        }
        
        /// <summary>
        /// 触发订单提交事件
        /// </summary>
        public static void InvokeOrderSubmitted(Order order)
        {
            EventBus.Publish(new OrderSubmittedEventData(order));
        }
        
        /// <summary>
        /// 触发订单完成事件
        /// </summary>
        public static void InvokeOrderCompleted(Order order)
        {
            EventBus.Publish(new OrderCompletedEventData(order));
        }
        
        /// <summary>
        /// 触发订单状态变化事件
        /// </summary>
        public static void InvokeOrderStatusChanged(Order order)
        {
            EventBus.Publish(new OrderStatusChangedEventData(order, order.Status, order.Status));
        }
        
        /// <summary>
        /// 触发订单状态变化事件（带旧状态）
        /// </summary>
        public static void InvokeOrderStatusChanged(Order order, OrderStatus oldStatus)
        {
            EventBus.Publish(new OrderStatusChangedEventData(order, oldStatus, order.Status));
        }
        
        /// <summary>
        /// 触发状态变化事件
        /// </summary>
        public static void InvokeStateChanged(string stateName, object newValue)
        {
            EventBus.Publish(new StateChangedEventData(stateName, newValue));
        }
        
        #endregion
        
        #region 事件注册辅助方法
        
        /// <summary>
        /// 安全订阅工具启动事件
        /// </summary>
        public static void SubscribeToolStarted(Action<CookingTool> callback)
        {
            EventBus.Subscribe(new ToolStartedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅工具启动事件
        /// </summary>
        public static void UnsubscribeToolStarted(Action<CookingTool> callback)
        {
            EventBus.Unsubscribe(new ToolStartedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅工具暂停事件
        /// </summary>
        public static void SubscribeToolPaused(Action<CookingTool> callback)
        {
            EventBus.Subscribe(new ToolPausedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅工具暂停事件
        /// </summary>
        public static void UnsubscribeToolPaused(Action<CookingTool> callback)
        {
            EventBus.Unsubscribe(new ToolPausedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅工具完成事件
        /// </summary>
        public static void SubscribeToolCompleted(Action<CookingTool> callback)
        {
            EventBus.Subscribe(new ToolCompletedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅工具完成事件
        /// </summary>
        public static void UnsubscribeToolCompleted(Action<CookingTool> callback)
        {
            EventBus.Unsubscribe(new ToolCompletedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅工具输出变化事件
        /// </summary>
        public static void SubscribeToolOutputChanged(Action<CookingTool> callback)
        {
            EventBus.Subscribe(new ToolOutputChangedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅工具输出变化事件
        /// </summary>
        public static void UnsubscribeToolOutputChanged(Action<CookingTool> callback)
        {
            EventBus.Unsubscribe(new ToolOutputChangedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅工具输入变化事件
        /// </summary>
        public static void SubscribeToolInputChanged(Action<CookingTool> callback)
        {
            EventBus.Subscribe(new ToolInputChangedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅工具输入变化事件
        /// </summary>
        public static void UnsubscribeToolInputChanged(Action<CookingTool> callback)
        {
            EventBus.Unsubscribe(new ToolInputChangedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅菜品创建事件
        /// </summary>
        public static void SubscribeDishCreated(Action<Item> callback)
        {
            EventBus.Subscribe(new DishCreatedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅菜品创建事件
        /// </summary>
        public static void UnsubscribeDishCreated(Action<Item> callback)
        {
            EventBus.Unsubscribe(new DishCreatedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅菜品更新事件
        /// </summary>
        public static void SubscribeDishUpdated(Action<Item> callback)
        {
            EventBus.Subscribe(new DishUpdatedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅菜品更新事件
        /// </summary>
        public static void UnsubscribeDishUpdated(Action<Item> callback)
        {
            EventBus.Unsubscribe(new DishUpdatedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅菜品删除事件
        /// </summary>
        public static void SubscribeDishDeleted(Action<Item> callback)
        {
            EventBus.Subscribe(new DishDeletedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅菜品删除事件
        /// </summary>
        public static void UnsubscribeDishDeleted(Action<Item> callback)
        {
            EventBus.Unsubscribe(new DishDeletedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅订单创建事件
        /// </summary>
        public static void SubscribeOrderCreated(Action<Order> callback)
        {
            EventBus.Subscribe(new OrderCreatedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅订单创建事件
        /// </summary>
        public static void UnsubscribeOrderCreated(Action<Order> callback)
        {
            EventBus.Unsubscribe(new OrderCreatedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅订单提交事件
        /// </summary>
        public static void SubscribeOrderSubmitted(Action<Order> callback)
        {
            EventBus.Subscribe(new OrderSubmittedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅订单提交事件
        /// </summary>
        public static void UnsubscribeOrderSubmitted(Action<Order> callback)
        {
            EventBus.Unsubscribe(new OrderSubmittedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅订单完成事件
        /// </summary>
        public static void SubscribeOrderCompleted(Action<Order> callback)
        {
            EventBus.Subscribe(new OrderCompletedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅订单完成事件
        /// </summary>
        public static void UnsubscribeOrderCompleted(Action<Order> callback)
        {
            EventBus.Unsubscribe(new OrderCompletedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅订单状态变化事件
        /// </summary>
        public static void SubscribeOrderStatusChanged(Action<Order> callback)
        {
            EventBus.Subscribe(new OrderStatusChangedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅订单状态变化事件
        /// </summary>
        public static void UnsubscribeOrderStatusChanged(Action<Order> callback)
        {
            EventBus.Unsubscribe(new OrderStatusChangedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全订阅状态变化事件
        /// </summary>
        public static void SubscribeStateChanged(Action<string, object> callback)
        {
            EventBus.Subscribe(new StateChangedEventHandler(callback));
        }
        
        /// <summary>
        /// 安全取消订阅状态变化事件
        /// </summary>
        public static void UnsubscribeStateChanged(Action<string, object> callback)
        {
            EventBus.Unsubscribe(new StateChangedEventHandler(callback));
        }
        
        #endregion
        
        #region 内部类 - 事件处理器包装器
        
        /// <summary>
        /// 工具启动事件处理器包装器
        /// </summary>
        private class ToolStartedEventHandler : IEventHandler<ToolStartedEventData>
        {
            private readonly Action<CookingTool> _callback;
            
            public ToolStartedEventHandler(Action<CookingTool> callback)
            {
                _callback = callback;
            }
            
            public void Handle(ToolStartedEventData eventData)
            {
                _callback(eventData.Tool);
            }
        }
        
        /// <summary>
        /// 工具暂停事件处理器包装器
        /// </summary>
        private class ToolPausedEventHandler : IEventHandler<ToolPausedEventData>
        {
            private readonly Action<CookingTool> _callback;
            
            public ToolPausedEventHandler(Action<CookingTool> callback)
            {
                _callback = callback;
            }
            
            public void Handle(ToolPausedEventData eventData)
            {
                _callback(eventData.Tool);
            }
        }
        
        /// <summary>
        /// 工具完成事件处理器包装器
        /// </summary>
        private class ToolCompletedEventHandler : IEventHandler<ToolCompletedEventData>
        {
            private readonly Action<CookingTool> _callback;
            
            public ToolCompletedEventHandler(Action<CookingTool> callback)
            {
                _callback = callback;
            }
            
            public void Handle(ToolCompletedEventData eventData)
            {
                _callback(eventData.Tool);
            }
        }
        
        /// <summary>
        /// 工具输出变化事件处理器包装器
        /// </summary>
        private class ToolOutputChangedEventHandler : IEventHandler<ToolOutputChangedEventData>
        {
            private readonly Action<CookingTool> _callback;
            
            public ToolOutputChangedEventHandler(Action<CookingTool> callback)
            {
                _callback = callback;
            }
            
            public void Handle(ToolOutputChangedEventData eventData)
            {
                _callback(eventData.Tool);
            }
        }
        
        /// <summary>
        /// 工具输入变化事件处理器包装器
        /// </summary>
        private class ToolInputChangedEventHandler : IEventHandler<ToolInputChangedEventData>
        {
            private readonly Action<CookingTool> _callback;
            
            public ToolInputChangedEventHandler(Action<CookingTool> callback)
            {
                _callback = callback;
            }
            
            public void Handle(ToolInputChangedEventData eventData)
            {
                _callback(eventData.Tool);
            }
        }
        
        /// <summary>
        /// 菜品创建事件处理器包装器
        /// </summary>
        private class DishCreatedEventHandler : IEventHandler<DishCreatedEventData>
        {
            private readonly Action<Item> _callback;
            
            public DishCreatedEventHandler(Action<Item> callback)
            {
                _callback = callback;
            }
            
            public void Handle(DishCreatedEventData eventData)
            {
                _callback(eventData.Dish);
            }
        }
        
        /// <summary>
        /// 菜品更新事件处理器包装器
        /// </summary>
        private class DishUpdatedEventHandler : IEventHandler<DishUpdatedEventData>
        {
            private readonly Action<Item> _callback;
            
            public DishUpdatedEventHandler(Action<Item> callback)
            {
                _callback = callback;
            }
            
            public void Handle(DishUpdatedEventData eventData)
            {
                _callback(eventData.Dish);
            }
        }
        
        /// <summary>
        /// 菜品删除事件处理器包装器
        /// </summary>
        private class DishDeletedEventHandler : IEventHandler<DishDeletedEventData>
        {
            private readonly Action<Item> _callback;
            
            public DishDeletedEventHandler(Action<Item> callback)
            {
                _callback = callback;
            }
            
            public void Handle(DishDeletedEventData eventData)
            {
                _callback(eventData.Dish);
            }
        }
        
        /// <summary>
        /// 订单创建事件处理器包装器
        /// </summary>
        private class OrderCreatedEventHandler : IEventHandler<OrderCreatedEventData>
        {
            private readonly Action<Order> _callback;
            
            public OrderCreatedEventHandler(Action<Order> callback)
            {
                _callback = callback;
            }
            
            public void Handle(OrderCreatedEventData eventData)
            {
                _callback(eventData.Order);
            }
        }
        
        /// <summary>
        /// 订单提交事件处理器包装器
        /// </summary>
        private class OrderSubmittedEventHandler : IEventHandler<OrderSubmittedEventData>
        {
            private readonly Action<Order> _callback;
            
            public OrderSubmittedEventHandler(Action<Order> callback)
            {
                _callback = callback;
            }
            
            public void Handle(OrderSubmittedEventData eventData)
            {
                _callback(eventData.Order);
            }
        }
        
        /// <summary>
        /// 订单完成事件处理器包装器
        /// </summary>
        private class OrderCompletedEventHandler : IEventHandler<OrderCompletedEventData>
        {
            private readonly Action<Order> _callback;
            
            public OrderCompletedEventHandler(Action<Order> callback)
            {
                _callback = callback;
            }
            
            public void Handle(OrderCompletedEventData eventData)
            {
                _callback(eventData.Order);
            }
        }
        
        /// <summary>
        /// 订单状态变化事件处理器包装器
        /// </summary>
        private class OrderStatusChangedEventHandler : IEventHandler<OrderStatusChangedEventData>
        {
            private readonly Action<Order> _callback;
            
            public OrderStatusChangedEventHandler(Action<Order> callback)
            {
                _callback = callback;
            }
            
            public void Handle(OrderStatusChangedEventData eventData)
            {
                _callback(eventData.Order);
            }
        }
        
        /// <summary>
        /// 状态变化事件处理器包装器
        /// </summary>
        private class StateChangedEventHandler : IEventHandler<StateChangedEventData>
        {
            private readonly Action<string, object> _callback;
            
            public StateChangedEventHandler(Action<string, object> callback)
            {
                _callback = callback;
            }
            
            public void Handle(StateChangedEventData eventData)
            {
                _callback(eventData.StateName, eventData.NewValue);
            }
        }
        
        #endregion
    }
}
