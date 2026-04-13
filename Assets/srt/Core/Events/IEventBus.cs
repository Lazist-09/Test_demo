using System;
using System.Collections.Generic;

namespace CookingGame.Core.Events
{
    /// <summary>
    /// 事件类型枚举
    /// 定义所有支持的事件类型
    /// </summary>
    public enum EventType
    {
        // 烹饪工具事件
        ToolStarted,
        ToolPaused,
        ToolCompleted,
        ToolOutputChanged,
        ToolInputChanged,
        
        // 物品事件
        DishCreated,
        DishUpdated,
        DishDeleted,
        
        // 订单事件
        OrderCreated,
        OrderSubmitted,
        OrderCompleted,
        OrderStatusChanged,
        
        // 状态变化事件
        StateChanged
    }
    
    /// <summary>
    /// 事件处理器接口
    /// </summary>
    /// <typeparam name="T">事件数据类型</typeparam>
    public interface IEventHandler<T>
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        void Handle(T eventData);
    }
    
    /// <summary>
    /// 事件总线接口
    /// 定义事件总线的核心功能
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="handler">事件处理器</param>
        void Subscribe<T>(IEventHandler<T> handler);
        
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="handler">事件处理器</param>
        void Unsubscribe<T>(IEventHandler<T> handler);
        
        /// <summary>
        /// 订阅事件（使用回调）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="callback">回调函数</param>
        void Subscribe<T>(Action<T> callback);
        
        /// <summary>
        /// 取消订阅事件（使用回调）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="callback">回调函数</param>
        void Unsubscribe<T>(Action<T> callback);
        
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="eventData">事件数据</param>
        void Publish<T>(T eventData);
        
        /// <summary>
        /// 发布事件（创建新的事件数据）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="dataFactory">事件数据工厂</param>
        void Publish<T>(EventType eventType, Func<T> dataFactory);
        
        /// <summary>
        /// 获取订阅者数量
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <returns>订阅者数量</returns>
        int GetSubscriberCount<T>();
        
        /// <summary>
        /// 清空所有订阅
        /// </summary>
        void ClearAllSubscriptions();
        
        /// <summary>
        /// 清空特定类型的订阅
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        void ClearSubscriptions<T>();
        
        /// <summary>
        /// 是否正在发布事件
        /// </summary>
        bool IsPublishing { get; }
        
        /// <summary>
        /// 事件日志大小
        /// </summary>
        int EventLogSize { get; }
    }
}
