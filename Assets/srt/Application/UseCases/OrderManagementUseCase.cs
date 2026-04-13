using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Services;
using CookingGame.Core.Models;
using CookingGame.Application.DTOs;
using CookingGame.Core.Events;

namespace CookingGame.Application.UseCases
{
    /// <summary>
    /// 订单管理用例
    /// 处理订单相关的业务逻辑
    /// </summary>
    public class OrderManagementUseCase
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
        /// 构造函数
        /// 通过依赖注入获取所需的服务
        /// </summary>
        /// <param name="orderRepository">订单仓储</param>
        /// <param name="recipeRepository">食谱仓储</param>
        public OrderManagementUseCase(
            IOrderRepository orderRepository,
            IRecipeRepository recipeRepository)
        {
            _orderRepository = orderRepository;
            _recipeRepository = recipeRepository;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <param name="recipeId">食谱ID</param>
        /// <param name="reward">奖励分数</param>
        /// <returns>创建的订单DTO</returns>
        public OrderDto CreateOrder(string id, string recipeId, int reward)
        {
            Debug.Log($"CreateOrder: id={id}, recipeId={recipeId}, reward={reward}");
            
            var recipe = _recipeRepository.GetById(recipeId);
            if (recipe == null)
            {
                Debug.LogError($"Recipe {recipeId} not found!");
                return null;
            }

            var order = new Order(id, recipe, reward);
            _orderRepository.Save(order);
            Debug.Log($"Order created: {order.Id}");
            
            // 触发订单创建事件
            GameEvents.InvokeOrderCreated(order);

            return OrderDto.FromOrder(order);
        }

        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns>订单DTO列表</returns>
        public List<OrderDto> GetAllOrders()
        {
            var orders = _orderRepository.GetAll();
            return orders.Select(OrderDto.FromOrder).ToList();
        }

        /// <summary>
        /// 根据ID获取订单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <returns>订单DTO，如果不存在则返回null</returns>
        public OrderDto GetOrderById(string id)
        {
            Debug.Log($"GetOrderById: {id}");
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                Debug.LogWarning($"Order {id} not found in repository");
                return null;
            }
            return OrderDto.FromOrder(order);
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="newStatus">新的订单状态</param>
        /// <returns>是否成功</returns>
        public bool UpdateOrderStatus(string orderId, OrderStatus newStatus)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                return false;
            }

            var oldStatus = order.Status;
            order.UpdateStatus(newStatus);
            _orderRepository.Update(order);
            
            // 触发订单状态变化事件
            GameEvents.InvokeOrderStatusChanged(order, oldStatus);

            return true;
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteOrder(string id)
        {
            var order = _orderRepository.GetById(id);
            if (order != null)
            {
                _orderRepository.Remove(id);
                return true;
            }
            return false;
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
