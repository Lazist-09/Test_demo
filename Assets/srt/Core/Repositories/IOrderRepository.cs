using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Repositories
{
    /// <summary>
    /// 订单仓储接口
    /// 定义订单的持久化操作
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// 保存订单
        /// 如果订单没有ID则生成新的ID
        /// </summary>
        /// <param name="order">要保存的订单</param>
        void Save(Order order);

        /// <summary>
        /// 根据ID获取订单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <returns>找到的订单，如果不存在则返回null</returns>
        Order GetById(string id);

        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns>所有订单的列表</returns>
        List<Order> GetAll();

        /// <summary>
        /// 根据ID删除订单
        /// </summary>
        /// <param name="id">订单ID</param>
        void Remove(string id);

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order">要更新的订单</param>
        void Update(Order order);
    }
}
