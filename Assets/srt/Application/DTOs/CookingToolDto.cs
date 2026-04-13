using UnityEngine;
using CookingGame.Core.Models;

namespace CookingGame.Application.DTOs
{
    /// <summary>
    /// 烹饪工具数据传输对象
    /// 用于在不同层之间传递烹饪工具数据
    /// </summary>
    public class CookingToolDto
    {
        /// <summary>
        /// 工具ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 工具类型
        /// </summary>
        public CookingToolType Type { get; set; }
        
        /// <summary>
        /// 输入的食材数量
        /// </summary>
        public int InputItemCount { get; set; }
        
        /// <summary>
        /// 最大容量
        /// </summary>
        public int MaxCapacity { get; set; }
        
        /// <summary>
        /// 当前进度
        /// </summary>
        public float CurrentProgress { get; set; }
        
        /// <summary>
        /// 是否正在运行 (由状态机计算)
        /// </summary>
        public bool IsRunning { get; set; }
        
        /// <summary>
        /// 当前状态 (由状态机管理)
        /// </summary>
        public CookingToolState State { get; set; }

        /// <summary>
        /// 从实体创建DTO
        /// </summary>
        /// <param name="tool">烹饪工具实体</param>
        /// <returns>烹饪工具DTO</returns>
        public static CookingToolDto FromTool(CookingTool tool)
        {
            Debug.Log($"CookingToolDto.FromTool: {tool.Id}, Type: {tool.Type}, InputItems: {tool.InputItems.Count}");
            var dto = new CookingToolDto
            {
                Id = tool.Id,
                Type = tool.Type,
                InputItemCount = tool.InputItems.Count,
                MaxCapacity = tool.GetMaxCapacity(),
                CurrentProgress = tool.CurrentProgress,
                IsRunning = tool.IsRunning,
                State = GetToolState(tool)
            };
            Debug.Log($"CookingToolDto created: Id={dto.Id}, IsRunning={dto.IsRunning}");
            return dto;
        }
        
        /// <summary>
        /// 获取工具状态
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>工具状态</returns>
        private static CookingToolState GetToolState(CookingTool tool)
        {
            if (tool.InputItems.Count == 0 && !tool.IsRunning)
            {
                return CookingToolState.Idle;
            }
            
            if (tool.InputItems.Count > 0 && !tool.IsRunning)
            {
                return CookingToolState.Preparing;
            }
            
            if (tool.IsRunning && tool.CurrentProgress < 1.0f)
            {
                return CookingToolState.Cooking;
            }
            
            if (tool.IsRunning && tool.CurrentProgress >= 1.0f)
            {
                return CookingToolState.Finished;
            }
            
            return CookingToolState.Paused;
        }

        /// <summary>
        /// 更新DTO
        /// </summary>
        /// <param name="dto">要更新的DTO</param>
        public void Update(CookingToolDto dto)
        {
            if (dto == null) return;
            
            Type = dto.Type;
            InputItemCount = dto.InputItemCount;
            MaxCapacity = dto.MaxCapacity;
            CurrentProgress = dto.CurrentProgress;
            IsRunning = dto.IsRunning;
            State = dto.State;
        }
    }
}
