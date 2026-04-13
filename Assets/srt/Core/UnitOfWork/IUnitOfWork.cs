using System;
using System.Collections.Generic;
using CookingGame.Core.Repositories;
using CookingGame.Core.Models;

namespace CookingGame.Core.UnitOfWork
{
    /// <summary>
    /// 单元OfWork接口
    /// 确保多个仓储操作的原子性
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 物品仓储
        /// </summary>
        IItemRepository Items { get; }
        
        /// <summary>
        /// 食谱仓储
        /// </summary>
        IRecipeRepository Recipes { get; }
        
        /// <summary>
        /// 订单仓储
        /// </summary>
        IOrderRepository Orders { get; }
        
        /// <summary>
        /// 烹饪工具仓储
        /// </summary>
        ICookingToolRepository Tools { get; }
        
        /// <summary>
        /// 保存所有更改
        /// </summary>
        /// <returns>受影响的行数</returns>
        int SaveChanges();
        
        /// <summary>
        /// 异步保存所有更改
        /// </summary>
        /// <returns>受影响的行数</returns>
        System.Threading.Tasks.Task<int> SaveChangesAsync();
    }
    
    /// <summary>
    /// 单元OfWork实现
    /// 管理所有仓储的事务
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// 物品仓储
        /// </summary>
        private readonly IItemRepository _items;
        
        /// <summary>
        /// 食谱仓储
        /// </summary>
        private readonly IRecipeRepository _recipes;
        
        /// <summary>
        /// 订单仓储
        /// </summary>
        private readonly IOrderRepository _orders;
        
        /// <summary>
        /// 烹饪工具仓储
        /// </summary>
        private readonly ICookingToolRepository _tools;
        
        /// <summary>
        /// 是否已处置
        /// </summary>
        private bool _disposed = false;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">物品仓储</param>
        /// <param name="recipes">食谱仓储</param>
        /// <param name="orders">订单仓储</param>
        /// <param name="tools">烹饪工具仓储</param>
        public UnitOfWork(
            IItemRepository items,
            IRecipeRepository recipes,
            IOrderRepository orders,
            ICookingToolRepository tools)
        {
            _items = items;
            _recipes = recipes;
            _orders = orders;
            _tools = tools;
        }
        
        /// <summary>
        /// 物品仓储
        /// </summary>
        public IItemRepository Items => _items;
        
        /// <summary>
        /// 食谱仓储
        /// </summary>
        public IRecipeRepository Recipes => _recipes;
        
        /// <summary>
        /// 订单仓储
        /// </summary>
        public IOrderRepository Orders => _orders;
        
        /// <summary>
        /// 烹饪工具仓储
        /// </summary>
        public ICookingToolRepository Tools => _tools;
        
        /// <summary>
        /// 保存所有更改
        /// </summary>
        /// <returns>受影响的行数</returns>
        public int SaveChanges()
        {
            // 对于内存仓储,此方法主要用于确保一致性
            // 实际的数据库仓储可以在这里实现事务
            return 1;
        }
        
        /// <summary>
        /// 异步保存所有更改
        /// </summary>
        /// <returns>受影响的行数</returns>
        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            return System.Threading.Tasks.Task.FromResult(SaveChanges());
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否正在释放</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            
            if (disposing)
            {
                // 释放托管资源
                // readonly 字段无法在 Dispose 中赋值为 null
                // 这些仓储对象由 DI 容器管理生命周期
            }
            
            // 释放非托管资源
            _disposed = true;
        }
    }
}
