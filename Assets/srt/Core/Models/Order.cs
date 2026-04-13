using System.Collections.Generic;
using UnityEngine;

namespace CookingGame.Core.Models
{
    /// <summary>
    /// 订单类
    /// 定义顾客点单的订单，包含所需菜品和奖励
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 订单唯一标识符
        /// </summary>
        public string Id { get; internal set; }
        
        /// <summary>
        /// 关联的食谱
        /// 定义订单要求的菜品
        /// </summary>
        public Recipe Recipe { get; private set; }
        
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; private set; }
        
        /// <summary>
        /// 订单奖励分数
        /// 完成订单后获得的分数
        /// </summary>
        public int Reward { get; private set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public float CreatedAt { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <param name="recipe">关联的食谱</param>
        /// <param name="reward">订单奖励分数</param>
        public Order(string id, Recipe recipe, int reward)
        {
            Id = id;
            Recipe = recipe;
            Reward = reward;
            Status = OrderStatus.Pending;  // 初始状态为待处理
            CreatedAt = Time.time;
        }

        /// <summary>
        /// 标记订单为已提交
        /// 当玩家将菜品放入出餐口时调用
        /// </summary>
        public void MarkAsSubmitted()
        {
            Status = OrderStatus.Submitted;
        }

        /// <summary>
        /// 标记订单为已完成
        /// 当菜品被验证并接受时调用
        /// </summary>
        public void MarkAsCompleted()
        {
            Status = OrderStatus.Completed;
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="newStatus">新的订单状态</param>
        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order">要更新的订单</param>
        public void Update(Order order)
        {
            if (order == null) return;
            
            Recipe = order.Recipe;
            Reward = order.Reward;
            Status = order.Status;
        }
    }
}
