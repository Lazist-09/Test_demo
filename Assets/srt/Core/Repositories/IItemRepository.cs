using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Repositories
{
    /// <summary>
    /// 物品仓储接口
    /// 定义物品的持久化操作
    /// </summary>
    public interface IItemRepository
    {
        /// <summary>
        /// 保存物品
        /// 如果物品没有ID则生成新的ID
        /// </summary>
        /// <param name="item">要保存的物品</param>
        void Save(Item item);

        /// <summary>
        /// 根据ID获取物品
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>找到的物品，如果不存在则返回null</returns>
        Item GetById(string id);

        /// <summary>
        /// 获取所有物品
        /// </summary>
        /// <returns>所有物品的列表</returns>
        List<Item> GetAll();

        /// <summary>
        /// 根据ID删除物品
        /// </summary>
        /// <param name="id">物品ID</param>
        void Remove(string id);

        /// <summary>
        /// 更新物品
        /// </summary>
        /// <param name="item">要更新的物品</param>
        void Update(Item item);
    }
}
