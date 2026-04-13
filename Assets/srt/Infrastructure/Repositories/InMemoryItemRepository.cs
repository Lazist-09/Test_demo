using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Models;
using CookingGame.Core.Events;

namespace CookingGame.Infrastructure.Repositories
{
    /// <summary>
    /// 内存物品仓储
    /// 使用内存字典存储物品数据，用于开发和测试
    /// </summary>
    public class InMemoryItemRepository : IItemRepository
    {
        /// <summary>
        /// 物品存储字典
        /// </summary>
        private readonly Dictionary<string, Item> _items = new Dictionary<string, Item>();
        
        /// <summary>
        /// ID计数器
        /// 用于生成唯一ID
        /// </summary>
        private int _idCounter = 0;

        /// <summary>
        /// 保存物品
        /// 如果物品没有ID则生成新的ID
        /// </summary>
        /// <param name="item">要保存的物品</param>
        public void Save(Item item)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                item.Id = GenerateId();
            }
            Debug.Log($"InMemoryItemRepository.Save: {item.Id}, total={_items.Count + 1}");
            _items[item.Id] = item;
            
            // 发布领域事件
            PublishDomainEvents(item);
        }

        /// <summary>
        /// 根据ID获取物品
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>找到的物品，如果不存在则返回null</returns>
        public Item GetById(string id)
        {
            Debug.Log($"InMemoryItemRepository.GetById: {id}, count={_items.Count}");
            if (_items.TryGetValue(id, out var item))
            {
                Debug.Log($"Found item: {item.Id}, TemplateId: {item.TemplateId}");
                return item;
            }
            else
            {
                Debug.Log($"Item not found: {id}");
                return null;
            }
        }

        /// <summary>
        /// 获取所有物品
        /// </summary>
        /// <returns>所有物品的列表</returns>
        public List<Item> GetAll()
        {
            return new List<Item>(_items.Values);
        }

        /// <summary>
        /// 根据ID删除物品
        /// </summary>
        /// <param name="id">物品ID</param>
        public void Remove(string id)
        {
            _items.Remove(id);
        }

        /// <summary>
        /// 更新物品
        /// </summary>
        /// <param name="item">要更新的物品</param>
        public void Update(Item item)
        {
            if (_items.ContainsKey(item.Id))
            {
                _items[item.Id] = item;
                
                // 发布领域事件
                PublishDomainEvents(item);
            }
        }

        /// <summary>
        /// 发布领域事件
        /// </summary>
        /// <param name="item">物品</param>
        private void PublishDomainEvents(Item item)
        {
            var domainEvents = item.GetDomainEvents();
            foreach (var @event in domainEvents)
            {
                DomainEvents.Publish(@event);
            }
            item.ClearDomainEvents();
        }

        /// <summary>
        /// 生成唯一ID
        /// </summary>
        /// <returns>生成的ID</returns>
        private string GenerateId()
        {
            return $"item_{++_idCounter}";
        }
    }
}
