using System;
using System.Collections.Generic;

namespace CookingGame.Core.Validation
{
    /// <summary>
    /// 结果包装类
    /// 用于封装操作结果和错误信息
    /// 支持泛型和非泛型两种形式
    /// </summary>
    /// <typeparam name="T">成功时返回的数据类型</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }
        
        /// <summary>
        /// 成功时的值
        /// </summary>
        public T Value { get; private set; }
        
        /// <summary>
        /// 失败时的错误信息
        /// </summary>
        public string Error { get; private set; }
        
        /// <summary>
        /// 错误详情列表
        /// </summary>
        public List<string> Errors { get; private set; } = new List<string>();
        
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private Result()
        {
            Errors = new List<string>();
        }
        
        /// <summary>
        /// 成功结果
        /// </summary>
        /// <param name="value">成功时的值</param>
        /// <returns>成功的结果包装</returns>
        public static Result<T> Success(T value)
        {
            return new Result<T> { IsSuccess = true, Value = value };
        }
        
        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>失败的结果包装</returns>
        public static Result<T> Failure(string error)
        {
            return new Result<T> { IsSuccess = false, Error = error };
        }
        
        /// <summary>
        /// 失败结果(带多个错误)
        /// </summary>
        /// <param name="errors">错误列表</param>
        /// <returns>失败的结果包装</returns>
        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T> { IsSuccess = false, Errors = errors };
        }
        
        /// <summary>
        /// 隐式转换 - 从 T 转换为 Result<T>
        /// </summary>
        /// <param name="value">值</param>
        public static implicit operator Result<T>(T value)
        {
            return Success(value);
        }
        
        /// <summary>
        /// 隐式转换 - 从 string 转换为 Result<T>
        /// </summary>
        /// <param name="error">错误信息</param>
        public static implicit operator Result<T>(string error)
        {
            return Failure(error);
        }
        
        /// <summary>
        /// 获取值或默认值
        /// </summary>
        /// <param name="defaultValue">默认值</param>
        /// <returns>成功时返回值,失败时返回默认值</returns>
        public T GetValueOrDefault(T defaultValue = default)
        {
            return IsSuccess ? Value : defaultValue;
        }
        
        /// <summary>
        /// 如果成功则执行操作
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <returns>当前结果</returns>
        public Result<T> OnSuccess(Action<T> action)
        {
            if (IsSuccess && action != null)
            {
                action(Value);
            }
            return this;
        }
        
        /// <summary>
        /// 如果失败则执行操作
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <returns>当前结果</returns>
        public Result<T> OnFailure(Action<string> action)
        {
            if (!IsSuccess && action != null)
            {
                action(Error);
            }
            return this;
        }
        
        /// <summary>
        /// 转换成功值
        /// </summary>
        /// <typeparam name="TNew">新的类型</typeparam>
        /// <param name="func">转换函数</param>
        /// <returns>新的结果包装</returns>
        public Result<TNew> Then<TNew>(Func<T, TNew> func)
        {
            if (IsSuccess)
            {
                try
                {
                    var newValue = func(Value);
                    return Result<TNew>.Success(newValue);
                }
                catch (Exception ex)
                {
                    return Result<TNew>.Failure(ex.Message);
                }
            }
            return Result<TNew>.Failure(Error);
        }
        
        /// <summary>
        /// 重写 ToString
        /// </summary>
        public override string ToString()
        {
            return IsSuccess 
                ? $"Success: {Value}" 
                : $"Failure: {string.Join(", ", Errors)}";
        }
    }
    
    /// <summary>
    /// 非泛型结果类
    /// 用于不需要返回值的操作
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }
        
        /// <summary>
        /// 失败时的错误信息
        /// </summary>
        public string Error { get; private set; }
        
        /// <summary>
        /// 错误详情列表
        /// </summary>
        public List<string> Errors { get; private set; } = new List<string>();
        
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private Result()
        {
            Errors = new List<string>();
        }
        
        /// <summary>
        /// 成功结果
        /// </summary>
        public static Result Success()
        {
            return new Result { IsSuccess = true };
        }
        
        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>失败的结果</returns>
        public static Result Failure(string error)
        {
            return new Result { IsSuccess = false, Error = error };
        }
        
        /// <summary>
        /// 失败结果(带多个错误)
        /// </summary>
        /// <param name="errors">错误列表</param>
        /// <returns>失败的结果</returns>
        public static Result Failure(List<string> errors)
        {
            return new Result { IsSuccess = false, Errors = errors };
        }
        
        /// <summary>
        /// 如果成功则执行操作
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <returns>当前结果</returns>
        public Result OnSuccess(Action action)
        {
            if (IsSuccess && action != null)
            {
                action();
            }
            return this;
        }
        
        /// <summary>
        /// 如果失败则执行操作
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <returns>当前结果</returns>
        public Result OnFailure(Action<string> action)
        {
            if (!IsSuccess && action != null)
            {
                action(Error);
            }
            return this;
        }
        
        /// <summary>
        /// 重写 ToString
        /// </summary>
        public override string ToString()
        {
            return IsSuccess 
                ? "Success" 
                : $"Failure: {string.Join(", ", Errors)}";
        }
    }
}
