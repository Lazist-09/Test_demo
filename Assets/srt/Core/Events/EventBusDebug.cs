using System;
using System.Collections.Generic;
using CookingGame.Core.Events;

namespace CookingGame.Core.Events
{
    /// <summary>
    /// 事件总线调试工具
    /// 提供事件日志和调试功能
    /// </summary>
    public static class EventBusDebug
    {
        #region 字段
        
        /// <summary>
        /// 事件日志
        /// </summary>
        private static readonly List<EventLogEntry> _eventLog = new List<EventLogEntry>();
        
        /// <summary>
        /// 事件日志最大大小
        /// </summary>
        private const int MaxEventLogSize = 1000;
        
        /// <summary>
        /// 是否启用日志
        /// </summary>
        private static bool _isEnabled = true;
        
        #endregion
        
        #region 属性
        
        /// <summary>
        /// 事件日志
        /// </summary>
        public static IReadOnlyList<EventLogEntry> EventLog => _eventLog;
        
        /// <summary>
        /// 是否启用日志
        /// </summary>
        public static bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                System.Console.WriteLine($"[EventBusDebug] Event logging {(value ? "enabled" : "disabled")}");
            }
        }
        
        #endregion
        
        #region 公共方法
        
        /// <summary>
        /// 清空事件日志
        /// </summary>
        public static void ClearLog()
        {
            lock (_eventLog)
            {
                _eventLog.Clear();
            }
        }
        
        /// <summary>
        /// 获取最近的事件日志
        /// </summary>
        /// <param name="count">日志数量</param>
        /// <returns>事件日志列表</returns>
        public static List<EventLogEntry> GetRecentLog(int count = 50)
        {
            lock (_eventLog)
            {
                if (count >= _eventLog.Count)
                {
                    return new List<EventLogEntry>(_eventLog);
                }
                
                return _eventLog.GetRange(_eventLog.Count - count, count);
            }
        }
        
        /// <summary>
        /// 获取特定类型的事件日志
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="count">日志数量</param>
        /// <returns>事件日志列表</returns>
        public static List<EventLogEntry> GetLogByType(EventType eventType, int count = 50)
        {
            lock (_eventLog)
            {
                var filtered = _eventLog.FindAll(e => e.EventType == eventType);
                
                if (count >= filtered.Count)
                {
                    return filtered;
                }
                
                return filtered.GetRange(filtered.Count - count, count);
            }
        }
        
        /// <summary>
        /// 记录事件日志
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public static void LogEvent(EventData eventData)
        {
            if (!_isEnabled || eventData == null) return;
            
            lock (_eventLog)
            {
                _eventLog.Add(new EventLogEntry(eventData));
                
                // 限制日志大小
                while (_eventLog.Count > MaxEventLogSize)
                {
                    _eventLog.RemoveAt(0);
                }
            }
        }
        
        /// <summary>
        /// 记录订阅日志
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">订阅动作</param>
        /// <param name="subscriberType">订阅者类型</param>
        public static void LogSubscription(EventType eventType, string action, Type subscriberType)
        {
            if (!_isEnabled) return;
            
            lock (_eventLog)
            {
                _eventLog.Add(new EventLogEntry(eventType, action, subscriberType));
                
                while (_eventLog.Count > MaxEventLogSize)
                {
                    _eventLog.RemoveAt(0);
                }
            }
        }
        
        #endregion
        
        #region 内部类 - EventLogEntry
        
        /// <summary>
        /// 事件日志条目
        /// </summary>
        public class EventLogEntry
        {
            /// <summary>
            /// 事件数据
            /// </summary>
            public EventData EventData { get; }
            
            /// <summary>
            /// 事件类型
            /// </summary>
            public EventType EventType => EventData?.EventType ?? EventType.StateChanged;
            
            /// <summary>
            /// 事件时间
            /// </summary>
            public DateTime Timestamp => EventData != null 
                ? new DateTime(EventData.Timestamp) 
                : DateTime.Now;
            
            /// <summary>
            /// 事件ID
            /// </summary>
            public Guid EventId => EventData?.EventId ?? Guid.Empty;
            
            /// <summary>
            /// 订阅动作（用于订阅/取消订阅日志）
            /// </summary>
            public string Action { get; }
            
            /// <summary>
            /// 订阅者类型（用于订阅/取消订阅日志）
            /// </summary>
            public Type SubscriberType { get; }
            
            /// <summary>
            /// 是否为订阅日志
            /// </summary>
            public bool IsSubscriptionLog => !string.IsNullOrEmpty(Action);
            
            /// <summary>
            /// 构造函数（用于事件日志）
            /// </summary>
            /// <param name="eventData">事件数据</param>
            public EventLogEntry(EventData eventData)
            {
                EventData = eventData;
                Action = null;
                SubscriberType = null;
            }
            
            /// <summary>
            /// 构造函数（用于订阅日志）
            /// </summary>
            /// <param name="eventType">事件类型</param>
            /// <param name="action">订阅动作</param>
            /// <param name="subscriberType">订阅者类型</param>
            public EventLogEntry(EventType eventType, string action, Type subscriberType)
            {
                EventData = null;
                Action = action;
                SubscriberType = subscriberType;
            }
            
            /// <summary>
            /// 获取日志描述
            /// </summary>
            /// <returns>日志描述</returns>
            public override string ToString()
            {
                if (IsSubscriptionLog)
                {
                    return $"[{Action}] {SubscriberType?.Name} on {EventType}";
                }
                
                return $"[{EventType}] {EventId} at {Timestamp:yyyy-MM-dd HH:mm:ss.fff}";
            }
        }
        
        #endregion
    }
}
