using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Application.DTOs
{
    /// <summary>
    /// 订单数据传输对象
    /// 用于在不同层之间传递订单数据
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 食谱
        /// </summary>
        public RecipeDto Recipe { get; set; }
        
        /// <summary>
        /// 奖励分数
        /// </summary>
        public int Reward { get; set; }
        
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public float CreatedAt { get; set; }

        /// <summary>
        /// 从订单实体创建DTO
        /// </summary>
        /// <param name="order">订单实体</param>
        /// <returns>订单DTO</returns>
        public static OrderDto FromOrder(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Recipe = RecipeDto.FromRecipe(order.Recipe),
                Reward = order.Reward,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };
        }

        /// <summary>
        /// 转换为订单实体
        /// </summary>
        /// <param name="recipe">食谱实体</param>
        /// <param name="createdAt">创建时间</param>
        /// <returns>订单实体</returns>
        public Order ToEntity(Recipe recipe, float createdAt)
        {
            return new Order(Id, recipe, Reward)
            {
                CreatedAt = createdAt
            };
        }

        /// <summary>
        /// 更新DTO
        /// </summary>
        /// <param name="dto">要更新的DTO</param>
        public void Update(OrderDto dto)
        {
            if (dto == null) return;
            
            Recipe = dto.Recipe;
            Reward = dto.Reward;
            Status = dto.Status;
            CreatedAt = dto.CreatedAt;
        }
    }
}
