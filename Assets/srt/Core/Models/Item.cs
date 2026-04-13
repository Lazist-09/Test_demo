using System;
using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Events;

namespace CookingGame.Core.Models
{
    /// <summary>
    /// 物品类
    /// 游戏中所有可交互物品的基类
    /// 包含物品的基本属性和操作方法
    /// </summary>
    public class Item
    {
        /// <summary>
        /// 物品唯一标识符
        /// </summary>
        public string Id { get; internal set; }
        
        /// <summary>
        /// 模板ID，对应数据库中的物品模板
        /// </summary>
        public string TemplateId { get; private set; }

        /// <summary>
        /// 物品分类
        /// </summary>
        public ItemType Category { get; private set; }
        
        /// <summary>
        /// 物品形状 (切割程度)
        /// </summary>
        public Shape Shape { get; internal set; }
        
        /// <summary>
        /// 物品熟度 (烹饪程度)
        /// </summary>
        public CookingStage CookingStage { get; internal set; }
        
        /// <summary>
        /// 物品当前状态
        /// </summary>
        public ItemStatus Status { get; internal set; }

        /// <summary>
        /// 处理进度 (0.0 - 1.0)
        /// 用于追踪烹饪或处理的完成度
        /// </summary>
        public float Progress { get; internal set; }
        
        /// <summary>
        /// 组件ID列表
        /// 记录组成该物品的其他物品ID (用于复合食材)
        /// </summary>
        public List<string> ComponentIds { get; internal set; }
        
        /// <summary>
        /// 领域事件列表
        /// 记录该物品触发的领域事件
        /// </summary>
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">物品唯一ID</param>
        /// <param name="templateId">模板ID</param>
        /// <param name="category">物品分类</param>
        public Item(string id, string templateId, ItemType category)
        {
            Id = id;
            TemplateId = templateId;
            Category = category;
            SetShape(Shape.Whole);
            SetCookingStage(CookingStage.Raw);
            SetStatus(ItemStatus.Inactive);
            Progress = 0f;
            ComponentIds = new List<string>();
        }

        /// <summary>
        /// 完整构造函数
        /// 用于创建具有指定属性的物品
        /// </summary>
        /// <param name="id">物品唯一ID</param>
        /// <param name="templateId">模板ID</param>
        /// <param name="category">物品分类</param>
        /// <param name="shape">物品形状</param>
        /// <param name="cookingStage">物品熟度</param>
        /// <param name="status">物品状态</param>
        public Item(string id, string templateId, ItemType category, Shape shape, CookingStage cookingStage, ItemStatus status)
        {
            Id = id;
            TemplateId = templateId;
            Category = category;
            SetShape(shape);
            SetCookingStage(cookingStage);
            SetStatus(status);
            Progress = 0f;
            ComponentIds = new List<string>();
        }

        /// <summary>
        /// 增加处理进度
        /// </summary>
        /// <param name="amount">增加的进度值</param>
        /// <param name="maxThreshold">最大进度阈值，默认1.0</param>
        public void AddProgress(float amount, float maxThreshold = 1.0f)
        {
            Progress += amount;
            if (Progress > maxThreshold) Progress = maxThreshold;
        }

        /// <summary>
        /// 设置物品形状
        /// </summary>
        /// <param name="newShape">新的形状</param>
        public void SetShape(Shape newShape)
        {
            Shape = newShape;
        }

        /// <summary>
        /// 设置物品熟度
        /// </summary>
        /// <param name="newStage">新的熟度</param>
        public void SetCookingStage(CookingStage newStage)
        {
            CookingStage = newStage;
        }

        /// <summary>
        /// 转换物品属性
        /// 当物品从一种状态转换到另一种状态时调用
        /// </summary>
        /// <param name="newCategory">新的分类</param>
        /// <param name="newShape">新的形状</param>
        /// <param name="newStage">新的熟度</param>
        public void Transform(ItemType newCategory, Shape newShape, CookingStage newStage)
        {
            Category = newCategory;
            SetShape(newShape);
            SetCookingStage(newStage);
            Progress = 0f;
        }

        /// <summary>
        /// 设置物品状态
        /// </summary>
        /// <param name="status">新的状态</param>
        public void SetStatus(ItemStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 更新物品
        /// </summary>
        /// <param name="item">要更新的物品</param>
        public void Update(Item item)
        {
            if (item == null) return;
            
            SetShape(item.Shape);
            SetCookingStage(item.CookingStage);
            SetStatus(item.Status);
            AddProgress(item.Progress);
            ComponentIds = new List<string>(item.ComponentIds);
        }

        /// <summary>
        /// 克隆物品
        /// 创建一个新的物品实例，复制当前物品的所有属性
        /// </summary>
        /// <param name="newId">新物品的ID</param>
        /// <returns>克隆的新物品</returns>
        public Item Clone(string newId)
        {
            var newItem = new Item(newId, TemplateId, Category);
            newItem.SetShape(Shape);
            newItem.SetCookingStage(CookingStage);
            newItem.SetStatus(Status);
            newItem.AddProgress(Progress);
            newItem.ComponentIds = new List<string>(ComponentIds);
            return newItem;
        }
        
        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="event">领域事件</param>
        public void AddDomainEvent(IDomainEvent @event)
        {
            _domainEvents.Add(@event);
        }
        
        /// <summary>
        /// 获取所有领域事件
        /// </summary>
        /// <returns>领域事件列表</returns>
        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }
        
        /// <summary>
        /// 清除所有领域事件
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
