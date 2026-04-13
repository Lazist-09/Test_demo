using System.Collections.Generic;
using System.Linq;
using CookingGame.Core.Repositories;
using CookingGame.Core.Services;
using CookingGame.Core.Models;
using CookingGame.Application.DTOs;
using CookingGame.Core.Events;

namespace CookingGame.Application.UseCases
{
    /// <summary>
    /// 订单提交用例
    /// 处理订单提交的核心业务逻辑
    /// </summary>
    public class OrderSubmissionUseCase
    {
        /// <summary>
        /// 订单仓储
        /// </summary>
        private readonly IOrderRepository _orderRepository;
        
        /// <summary>
        /// 食谱仓储
        /// </summary>
        private readonly IRecipeRepository _recipeRepository;
        
        /// <summary>
        /// 物品仓储
        /// </summary>
        private readonly IItemRepository _itemRepository;
        
        /// <summary>
        /// 配方匹配器
        /// </summary>
        private readonly IRecipeMatcher _recipeMatcher;

        /// <summary>
        /// 构造函数
        /// 通过依赖注入获取所需的服务
        /// </summary>
        /// <param name="orderRepository">订单仓储</param>
        /// <param name="recipeRepository">食谱仓储</param>
        /// <param name="itemRepository">物品仓储</param>
        /// <param name="recipeMatcher">配方匹配器</param>
        public OrderSubmissionUseCase(
            IOrderRepository orderRepository,
            IRecipeRepository recipeRepository,
            IItemRepository itemRepository,
            IRecipeMatcher recipeMatcher)
        {
            _orderRepository = orderRepository;
            _recipeRepository = recipeRepository;
            _itemRepository = itemRepository;
            _recipeMatcher = recipeMatcher;
        }

        /// <summary>
        /// 提交订单
        /// 验证菜品并提交订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="dishItemId">菜品物品ID</param>
        /// <returns>提交的订单DTO，如果失败则返回null</returns>
        public OrderDto SubmitOrder(string orderId, string dishItemId)
        {
            var order = _orderRepository.GetById(orderId);
            var dishItem = _itemRepository.GetById(dishItemId);

            if (order == null || dishItem == null)
            {
                return null;
            }

            // 只有待处理订单可以提交
            if (order.Status != OrderStatus.Pending)
            {
                return null;
            }

            // 匹配方并验证
            var matchedRecipe = _recipeMatcher.MatchRecipe(new List<Item> { dishItem });
            
            if (matchedRecipe == null || matchedRecipe.Id != order.Recipe.Id)
            {
                return null;
            }

            // 验证菜品属性是否符合订单要求
            var recipeIngredient = order.Recipe.Ingredients.FirstOrDefault();
            if (recipeIngredient != null)
            {
                if (dishItem.Shape != recipeIngredient.RequiredShape ||
                    dishItem.CookingStage != recipeIngredient.RequiredStage)
                {
                    return null;
                }
            }

            // 计算得分
            int score = _recipeMatcher.CalculateScore(order.Recipe, new List<Item> { dishItem });

            // 标记订单为已提交
            order.MarkAsSubmitted();
            _orderRepository.Update(order);
            
            // 触发订单提交事件
            GameEvents.InvokeOrderSubmitted(order);

            return OrderDto.FromOrder(order);
        }

        /// <summary>
        /// 验证菜品是否适合订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="dishItemId">菜品物品ID</param>
        /// <returns>是否适合</returns>
        public bool ValidateDishForOrder(string orderId, string dishItemId)
        {
            var order = _orderRepository.GetById(orderId);
            var dishItem = _itemRepository.GetById(dishItemId);

            if (order == null || dishItem == null)
            {
                return false;
            }

            var matchedRecipe = _recipeMatcher.MatchRecipe(new List<Item> { dishItem });
            
            if (matchedRecipe == null)
            {
                return false;
            }

            // 验证食谱ID是否匹配
            return matchedRecipe.Id == order.Recipe.Id;
        }

        /// <summary>
        /// 计算订单奖励
        /// 根据菜品得分计算实际奖励分数
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="dishItemId">菜品物品ID</param>
        /// <returns>奖励分数</returns>
        public int CalculateOrderReward(string orderId, string dishItemId)
        {
            var order = _orderRepository.GetById(orderId);
            var dishItem = _itemRepository.GetById(dishItemId);

            if (order == null || dishItem == null)
            {
                return 0;
            }

            var matchedRecipe = _recipeMatcher.MatchRecipe(new List<Item> { dishItem });
            
            if (matchedRecipe == null || matchedRecipe.Id != order.Recipe.Id)
            {
                return 0;
            }

            // 计算菜品得分
            int score = _recipeMatcher.CalculateScore(order.Recipe, new List<Item> { dishItem });

            // 根据得分比例计算奖励
            return (int)(order.Reward * (score / 100f));
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order">要更新的订单</param>
        public void UpdateOrder(Order order)
        {
            if (order != null)
            {
                _orderRepository.Update(order);
            }
        }
    }
}
