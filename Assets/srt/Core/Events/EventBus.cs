using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CookingGame.Core.Events
{
    /// <summary>
    /// 事件总线实现
    /// 使用泛型字典存储事件处理器，支持事件订阅、取消订阅和发布
    /// </summary>
    public class EventBus : IEventBus
    {
        #region 字段
        
        /// <summary>
        /// 事件处理器字典
        /// 键为事件数据类型，值为处理器列表
        /// </summary>
        private readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();
        
        /// <summary>
        /// 事件日志
        /// 用于调试和追踪事件
        /// </summary>
        private readonly Queue<EventData> _eventLog = new Queue<EventData>();
        
        /// <summary>
        /// 事件日志最大大小
        /// </summary>
        private const int MaxEventLogSize = 1000;
        
        /// <summary>
        /// 是否正在发布事件
        /// 防止递归发布
        /// </summary>
        private bool _isPublishing;
        
        /// <summary>
        /// 事件日志回调
        /// </summary>
        private Action<EventData> _eventLogCallback;
        
        #endregion
        
        #region 属性
        
        /// <summary>
        /// 是否正在发布事件
        /// </summary>
        public bool IsPublishing => _isPublishing;
        
        /// <summary>
        /// 事件日志大小
        /// </summary>
        public int EventLogSize => _eventLog.Count;
        
        #endregion
        
        #region 订阅方法
        
        /// <summary>
        /// 订阅事件（使用处理器）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="handler">事件处理器</param>
        public void Subscribe<T>(IEventHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            var eventType = typeof(T);
            
            lock (_handlers)
            {
                if (!_handlers.ContainsKey(eventType))
                {
                    _handlers[eventType] = new List<object>();
                }
                
                // 检查是否已订阅
                if (!_handlers[eventType].Any(h => h.Equals(handler)))
                {
                    _handlers[eventType].Add(handler);
                    LogEventSubscription(eventType, "Subscribe");
                }
            }
        }
        
        /// <summary>
        /// 订阅事件（使用回调）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="callback">回调函数</param>
        public void Subscribe<T>(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            
            // 创建包装处理器
            var wrapper = new ActionEventHandler<T>(callback);
            Subscribe(wrapper);
        }
        
        #endregion
        
        #region 取消订阅方法
        
        /// <summary>
        /// 取消订阅事件（使用处理器）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="handler">事件处理器</param>
        public void Unsubscribe<T>(IEventHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            var eventType = typeof(T);
            
            lock (_handlers)
            {
                if (_handlers.ContainsKey(eventType))
                {
                    _handlers[eventType] = _handlers[eventType]
                        .Where(h => !h.Equals(handler))
                        .ToList();
                    
                    if (_handlers[eventType].Count == 0)
                    {
                        _handlers.Remove(eventType);
                    }
                    
                    LogEventSubscription(eventType, "Unsubscribe");
                }
            }
        }
        
        /// <summary>
        /// 取消订阅事件（使用回调）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="callback">回调函数</param>
        public void Unsubscribe<T>(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            
            // 创建包装处理器用于查找
            var wrapper = new ActionEventHandler<T>(callback);
            Unsubscribe(wrapper);
        }
        
        #endregion
        
        #region 发布方法
        
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="eventData">事件数据</param>
        public void Publish<T>(T eventData)
        {
            if (eventData == null) throw new ArgumentNullException(nameof(eventData));
            
            // 防止递归发布
            if (_isPublishing)
            {
                System.Console.WriteLine($"[EventBus] Warning: Recursive publishing detected for event type: {typeof(T).Name}");
                return;
            }
            
            _isPublishing = true;
            
            try
            {
                var eventType = typeof(T);
                
                // 记录事件日志
                LogEvent(eventData);
                
                // 获取处理器
                List<object> handlers;
                lock (_handlers)
                {
                    handlers = _handlers.ContainsKey(eventType) 
                        ? new List<object>(_handlers[eventType]) // 创建副本以避免修改集合时的异常
                        : new List<object>();
                }
                
                // 调用处理器
                foreach (var handler in handlers)
                {
                    try
                    {
                        if (handler is IEventHandler<T> typedHandler)
                        {
                            typedHandler.Handle(eventData);
                        }
                        else if (handler is ActionEventHandler<T> actionHandler)
                        {
                            actionHandler.Handle(eventData);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"[EventBus] Error: {ex.Message}");
                    }
                }
            }
            finally
            {
                _isPublishing = false;
            }
        }
        
        /// <summary>
        /// 发布事件（创建新的事件数据）
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="dataFactory">事件数据工厂</param>
        public void Publish<T>(EventType eventType, Func<T> dataFactory)
        {
            if (dataFactory == null) throw new ArgumentNullException(nameof(dataFactory));
            
            var eventData = dataFactory();
            Publish(eventData);
        }
        
        #endregion
        
        #region 清理方法
        
        /// <summary>
        /// 清空所有订阅
        /// </summary>
        public void ClearAllSubscriptions()
        {
            lock (_handlers)
            {
                _handlers.Clear();
                LogEventSubscription(null, "ClearAll");
            }
        }
        
        /// <summary>
        /// 清空特定类型的订阅
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        public void ClearSubscriptions<T>()
        {
            var eventType = typeof(T);
            
            lock (_handlers)
            {
                if (_handlers.ContainsKey(eventType))
                {
                    _handlers.Remove(eventType);
                    LogEventSubscription(eventType, "Clear");
                }
            }
        }
        
        #endregion
        
        #region 辅助方法
        
        /// <summary>
        /// 获取订阅者数量
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        /// <returns>订阅者数量</returns>
        public int GetSubscriberCount<T>()
        {
            var eventType = typeof(T);
            
            lock (_handlers)
            {
                return _handlers.ContainsKey(eventType) 
                    ? _handlers[eventType].Count 
                    : 0;
            }
        }
        
        /// <summary>
        /// 设置事件日志回调
        /// </summary>
        /// <param name="callback">回调函数</param>
        public void SetEventLogCallback(Action<EventData> callback)
        {
            _eventLogCallback = callback;
        }
        
        /// <summary>
        /// 记录事件日志
        /// </summary>
        /// <param name="eventData">事件数据</param>
        private void LogEvent(EventData eventData)
        {
            _eventLog.Enqueue(eventData);
            
            // 限制日志大小
            while (_eventLog.Count > MaxEventLogSize)
            {
                _eventLog.Dequeue();
            }
            
            // 调用回调
            _eventLogCallback?.Invoke(eventData);
        }
        
        /// <summary>
        /// 记录事件订阅日志
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">订阅动作</param>
        private void LogEventSubscription(Type eventType, string action)
        {
            System.Console.WriteLine($"[EventBus] {action}: {eventType?.Name ?? "All"}");
        }
        
        #endregion
        
        #region 内部类 - ActionEventHandler
        
        /// <summary>
        /// Action 包装器，实现 IEventHandler 接口
        /// </summary>
        /// <typeparam name="T">事件数据类型</typeparam>
        private class ActionEventHandler<T> : IEventHandler<T>
        {
            private readonly Action<T> _action;
            
            public ActionEventHandler(Action<T> action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
            }
            
            public void Handle(T eventData)
            {
                _action(eventData);
            }
            
            public override bool Equals(object obj)
            {
                if (obj is ActionEventHandler<T> other)
                {
                    return _action.Equals(other._action);
                }
                return false;
            }
            
            public override int GetHashCode()
            {
                return _action.GetHashCode();
            }
        }
        
        #endregion
    }
}
