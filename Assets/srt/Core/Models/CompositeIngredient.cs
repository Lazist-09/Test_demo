using System.Collections.Generic;
using UnityEngine;

namespace CookingGame.Core.Models
{
    /// <summary>
    /// 临时复合食材类
    /// 在烹饪过程中创建的中间产物，用于动态匹配配方
    /// 烹饪结束后会被销毁
    /// </summary>
    public class CompositeIngredient
    {
        /// <summary>
        /// 复合食材唯一标识符
        /// </summary>
        public string Id { get; private set; }
        
        /// <summary>
        /// 组成该复合食材的物品ID列表
        /// 记录所有参与烹饪的食材
        /// </summary>
        public List<string> ComponentItemIds { get; private set; }
        
        /// <summary>
        /// 当前熟度
        /// 在烹饪过程中动态更新
        /// </summary>
        public CookingStage CookingStage { get; private set; }
        
        /// <summary>
        /// 烹饪进度 (0.0 - 1.0)
        /// </summary>
        public float CookingProgress { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">复合食材ID</param>
        public CompositeIngredient(string id)
        {
            Id = id;
            ComponentItemIds = new List<string>();
            CookingStage = CookingStage.Raw;
            CookingProgress = 0f;
        }

        /// <summary>
        /// 添加组件食材
        /// </summary>
        /// <param name="itemId">食材ID</param>
        public void AddComponent(string itemId)
        {
            ComponentItemIds.Add(itemId);
        }

        /// <summary>
        /// 移除组件食材
        /// </summary>
        /// <param name="itemId">食材ID</param>
        public void RemoveComponent(string itemId)
        {
            ComponentItemIds.Remove(itemId);
        }

        /// <summary>
        /// 设置熟度
        /// </summary>
        /// <param name="stage">新的熟度</param>
        public void SetCookingStage(CookingStage stage)
        {
            CookingStage = stage;
        }

        /// <summary>
        /// 增加烹饪进度
        /// </summary>
        /// <param name="amount">增加的进度值</param>
        public void AddCookingProgress(float amount)
        {
            CookingProgress += amount;
            if (CookingProgress > 1.0f)
                CookingProgress = 1.0f;
        }

        /// <summary>
        /// 更新复合食材
        /// </summary>
        /// <param name="composite">要更新的复合食材</param>
        public void Update(CompositeIngredient composite)
        {
            if (composite == null) return;
            
            CookingStage = composite.CookingStage;
            CookingProgress = composite.CookingProgress;
            ComponentItemIds = new List<string>(composite.ComponentItemIds);
        }

        /// <summary>
        /// 检查复合食材是否有效
        /// </summary>
        public bool IsValid => ComponentItemIds.Count > 0 && CookingStage != CookingStage.Burnt;
    }
}
