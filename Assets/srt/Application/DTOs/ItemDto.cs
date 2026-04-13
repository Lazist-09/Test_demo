using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Models;

namespace CookingGame.Application.DTOs
{
    /// <summary>
    /// 物品数据传输对象
    /// 用于在不同层之间传递物品数据
    /// </summary>
    public class ItemDto
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 模板ID
        /// </summary>
        public string TemplateId { get; set; }
        
        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 物品分类
        /// </summary>
        public ItemType Category { get; set; }
        
        /// <summary>
        /// 物品形状
        /// </summary>
        public Shape Shape { get; set; }
        
        /// <summary>
        /// 熟度
        /// </summary>
        public CookingStage CookingStage { get; set; }
        
        /// <summary>
        /// 物品状态
        /// </summary>
        public ItemStatus Status { get; set; }
        
        /// <summary>
        /// 处理进度
        /// </summary>
        public float Progress { get; set; }
        
        /// <summary>
        /// 组件ID列表
        /// </summary>
        public List<string> ComponentIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemDto()
        {
            ComponentIds = new List<string>();
        }

        /// <summary>
        /// 从物品创建DTO
        /// </summary>
        /// <param name="item">物品</param>
        /// <returns>DTO实例</returns>
        public static ItemDto FromItem(Item item)
        {
            Debug.Log($"ItemDto.FromItem: item.Id={item.Id}, item.TemplateId={item.TemplateId}");
            var dto = new ItemDto
            {
                Id = item.Id,
                TemplateId = item.TemplateId,
                Name = GetItemName(item),
                Category = item.Category,
                Shape = item.Shape,
                CookingStage = item.CookingStage,
                Status = item.Status,
                Progress = item.Progress,
                ComponentIds = new List<string>(item.ComponentIds)
            };
            Debug.Log($"ItemDto created: Id={dto.Id}, Name={dto.Name}");
            return dto;
        }
        
        /// <summary>
        /// 获取物品名称
        /// </summary>
        /// <param name="item">物品</param>
        /// <returns>物品名称</returns>
        private static string GetItemName(Item item)
        {
            string shapeName = item.Shape.ToString();
            string stageName = item.CookingStage.ToString();
            string categoryName = item.Category.ToString();
            
            // 使用模板ID作为名称
            return $"{item.TemplateId} ({shapeName} {stageName} {categoryName})";
        }

        /// <summary>
        /// 转换为物品
        /// </summary>
        /// <returns>物品实例</returns>
        public Item ToItem()
        {
            var item = new Item(Id, TemplateId, Category);
            item.SetShape(Shape);
            item.SetCookingStage(CookingStage);
            item.SetStatus(Status);
            item.AddProgress(Progress);
            item.ComponentIds = new List<string>(ComponentIds);
            return item;
        }

        /// <summary>
        /// 更新DTO
        /// </summary>
        /// <param name="dto">源DTO</param>
        public void Update(ItemDto dto)
        {
            if (dto == null) return;
            
            TemplateId = dto.TemplateId;
            Category = dto.Category;
            Shape = dto.Shape;
            CookingStage = dto.CookingStage;
            Status = dto.Status;
            Progress = dto.Progress;
            ComponentIds = new List<string>(dto.ComponentIds);
        }
    }
}
