using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Validation
{
    /// <summary>
    /// 订单验证服务
    /// 验证订单的各种操作是否合法
    /// </summary>
    public interface IOrderValidationService
    {
        /// <summary>
        /// 验证是否可以提交订单
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="dish">菜品</param>
        /// <returns>验证结果</returns>
        Result CanSubmitOrder(Order order, Item dish);
        
        /// <summary>
        /// 验证是否可以完成订单
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>验证结果</returns>
        Result CanCompleteOrder(Order order);
        
        /// <summary>
        /// 验证订单是否匹配菜品
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="dish">菜品</param>
        /// <returns>验证结果</returns>
        Result IsOrderMatched(Order order, Item dish);
    }
    
    /// <summary>
    /// 默认订单验证服务实现
    /// </summary>
    public class OrderValidationService : IOrderValidationService
    {
        /// <summary>
        /// 验证是否可以提交订单
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="dish">菜品</param>
        /// <returns>验证结果</returns>
        public Result CanSubmitOrder(Order order, Item dish)
        {
            var errors = new List<string>();
            
            if (order == null)
            {
                errors.Add("订单不存在");
            }
            
            if (dish == null)
            {
                errors.Add("菜品不存在");
            }
            
            if (order.Status != OrderStatus.Pending)
            {
                errors.Add($"订单状态为 {order.Status},无法提交");
            }
            
            if (dish.Category != ItemType.FinishedDish)
            {
                errors.Add("只有完成的菜品才能提交订单");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        /// <summary>
        /// 验证是否可以完成订单
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>验证结果</returns>
        public Result CanCompleteOrder(Order order)
        {
            var errors = new List<string>();
            
            if (order == null)
            {
                errors.Add("订单不存在");
                return Result.Failure(errors);
            }
            
            if (order.Status != OrderStatus.Submitted)
            {
                errors.Add($"订单状态为 {order.Status},无法完成");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        /// <summary>
        /// 验证订单是否匹配菜品
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="dish">菜品</param>
        /// <returns>验证结果</returns>
        public Result IsOrderMatched(Order order, Item dish)
        {
            var errors = new List<string>();
            
            if (order == null || dish == null)
            {
                errors.Add("订单或菜品不存在");
                return Result.Failure(errors);
            }
            
            if (order.Recipe == null)
            {
                errors.Add("订单没有关联食谱");
                return Result.Failure(errors);
            }
            
            if (dish.TemplateId != order.Recipe.Id)
            {
                errors.Add($"菜品类型不匹配: 订单需要 {order.Recipe.Id},实际为 {dish.TemplateId}");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
    }
}
