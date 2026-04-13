namespace CookingGame.Core.Logging
{
    /// <summary>
    /// 日志接口
    /// 定义日志记录的基本操作
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        void Info(string message, params object[] args);
        
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        void Warning(string message, params object[] args);
        
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        void Error(string message, params object[] args);
        
        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="args">格式化参数</param>
        void Debug(string message, params object[] args);
        
        /// <summary>
        /// 记录领域事件日志
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="details">事件详情</param>
        void DomainEvent(string eventName, string details);
    }
}
