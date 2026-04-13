using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Models;

namespace CookingGame.Infrastructure.Repositories
{
    /// <summary>
    /// 内存订单仓储
    /// 使用内存字典存储订单数据，用于开发和测试
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        /// <summary>
        /// 订单存储字典
        /// </summary>
        private readonly Dictionary<string, Order> _orders = new Dictionary<string, Order>();
        
        /// <summary>
        /// ID计数器
        /// 用于生成唯一ID
        /// </summary>
        private int _idCounter = 0;

        /// <summary>
        /// 保存订单
        /// 如果订单没有ID则生成新的ID
        /// </summary>
        /// <param name="order">要保存的订单</param>
        public void Save(Order order)
        {
            if (string.IsNullOrEmpty(order.Id))
            {
                order.Id = GenerateId();
            }
            Debug.Log($"InMemoryOrderRepository.Save: {order.Id}, total={_orders.Count + 1}");
            _orders[order.Id] = order;
        }

        /// <summary>
        /// 根据ID获取订单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <returns>找到的订单，如果不存在则返回null</returns>
        public Order GetById(string id)
        {
            Debug.Log($"InMemoryOrderRepository.GetById: {id}, count={_orders.Count}");
            return _orders.TryGetValue(id, out var order) ? order : null;
        }

        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns>所有订单的列表</returns>
        public List<Order> GetAll()
        {
            return new List<Order>(_orders.Values);
        }

        /// <summary>
        /// 根据状态获取订单
        /// </summary>
        /// <param name="status">订单状态</param>
        /// <returns>指定状态的订单列表</returns>
        public List<Order> GetByStatus(OrderStatus status)
        {
            return new List<Order>(_orders.Values.Where(o => o.Status == status));
        }

        /// <summary>
        /// 根据ID删除订单
        /// </summary>
        /// <param name="id">订单ID</param>
        public void Remove(string id)
        {
            _orders.Remove(id);
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order">要更新的订单</param>
        public void Update(Order order)
        {
            if (_orders.ContainsKey(order.Id))
            {
                _orders[order.Id] = order;
            }
        }

        /// <summary>
        /// 生成唯一ID
        /// </summary>
        /// <returns>生成的ID</returns>
        private string GenerateId()
        {
            return $"order_{++_idCounter}";
        }
    }
}
