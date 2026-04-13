using System.Collections.Generic;
using UnityEngine;

namespace CookingGame.Core.Models
{
    /// <summary>
    /// 烹饪工具类
    /// 定义游戏中的各种烹饪工具，如刀、锅、烤箱等
    /// 支持食材输入、处理和输出
    /// </summary>
    public class CookingTool
    {
        /// <summary>
        /// 工具唯一标识符
        /// </summary>
        public string Id { get; internal set; }
        
        /// <summary>
        /// 工具类型
        /// 决定工具的功能和容量
        /// </summary>
        public CookingToolType Type { get; private set; }
        
        /// <summary>
        /// 输入的食材列表
        /// 存储当前放入工具中的所有食材
        /// </summary>
        public List<Item> InputItems { get; private set; }
        
        /// <summary>
        /// 输出的菜品
        /// 烹饪完成后生成的菜品
        /// </summary>
        public Item OutputItem { get; private set; }
        
        /// <summary>
        /// 当前处理进度 (0.0 - 1.0)
        /// 表示烹饪进度的完成度
        /// </summary>
        public float CurrentProgress { get; internal set; }
        
        /// <summary>
        /// 最大容量
        /// </summary>
        private int _maxCapacity;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">工具ID</param>
        /// <param name="type">工具类型</param>
        public CookingTool(string id, CookingToolType type)
        {
            Id = id;
            Type = type;
            InputItems = new List<Item>();
            OutputItem = null;
            CurrentProgress = 0f;
            _maxCapacity = GetMaxCapacity();
        }

        /// <summary>
        /// 检查是否可以添加食材
        /// 工具必须未运行且未满才能添加食材
        /// </summary>
        /// <returns>是否可以添加食材</returns>
        public bool CanAddItem()
        {
            var canAdd = !IsRunning && InputItems.Count < GetMaxCapacity();
            Debug.Log($"CookingTool.CanAddItem: {Id}, IsRunning={IsRunning}, InputItems.Count={InputItems.Count}, MaxCapacity={GetMaxCapacity()}, CanAdd={canAdd}");
            return canAdd;
        }
        
        /// <summary>
        /// 是否正在运行 (由状态机计算)
        /// </summary>
        public bool IsRunning => _currentState == CookingToolState.Cooking || 
                                  _currentState == CookingToolState.Paused;
        
        /// <summary>
        /// 当前状态 (由状态机管理)
        /// </summary>
        internal CookingToolState _currentState = CookingToolState.Idle;

        /// <summary>
        /// 检查是否可以取出输出
        /// 工具必须未运行且有输出才能取出
        /// </summary>
        /// <returns>是否可以取出输出</returns>
        public bool CanRemoveItem()
        {
            return !IsRunning && OutputItem != null;
        }

        /// <summary>
        /// 获取工具的最大容量
        /// 不同类型的工具有不同的容量限制
        /// </summary>
        /// <returns>最大容量</returns>
        public int GetMaxCapacity()
        {
            Debug.Log($"CookingTool.GetMaxCapacity: {Id}, Type: {Type}");
            switch (Type)
            {
                case CookingToolType.Knife:
                case CookingToolType.Mixer:
                    Debug.Log($"Max capacity: 1 (Knife/Mixer)");
                    return 1;  // 刀具和搅拌器只能处理1个食材
                case CookingToolType.Pan:
                case CookingToolType.Pot:
                    Debug.Log($"Max capacity: 3 (Pan/Pot)");
                    return 3;  // 锅和平底锅可以处理3个食材
                case CookingToolType.Oven:
                    Debug.Log($"Max capacity: 3 (Oven)");
                    return 3;  // 烤箱可以处理3个食材
                default:
                    Debug.Log($"Max capacity: 1 (default)");
                    return 1;
            }
        }

        /// <summary>
        /// 启动工具的烹饪流程
        /// </summary>
        public void StartCooking()
        {
            Debug.Log($"CookingTool.StartCooking: {Id}, InputItems: {InputItems.Count}");
            _currentState = CookingToolState.Cooking;
            Debug.Log($"Cooking started: IsRunning={IsRunning}");
        }

        /// <summary>
        /// 停止烹饪
        /// 将工具状态重置为空闲，允许再次添加食材
        /// </summary>
        public void StopCooking()
        {
            _currentState = CookingToolState.Idle;
        }

        /// <summary>
        /// 增加处理进度
        /// 在烹饪过程中增加进度值
        /// </summary>
        /// <param name="amount">增加的进度值</param>
        public void AddProgress(float amount)
        {
            if (IsRunning)
            {
                CurrentProgress += amount;
                if (CurrentProgress >= 1.0f)
                {
                    CurrentProgress = 1.0f;  // 进度不能超过1.0
                }
            }
        }

        /// <summary>
        /// 清空输入
        /// 移除所有输入的食材
        /// </summary>
        public void ClearInput()
        {
            InputItems.Clear();
            CurrentProgress = 0f;
            _currentState = CookingToolState.Idle;
        }

        /// <summary>
        /// 设置输出
        /// 设置烹饪完成后的输出菜品
        /// </summary>
        /// <param name="item">输出的菜品</param>
        public void SetOutput(Item item)
        {
            OutputItem = item;
        }

        /// <summary>
        /// 清空输出
        /// 移除输出的菜品
        /// </summary>
        public void ClearOutput()
        {
            OutputItem = null;
        }

        /// <summary>
        /// 更新烹饪工具
        /// </summary>
        /// <param name="tool">要更新的工具</param>
        public void Update(CookingTool tool)
        {
            if (tool == null) return;
            
            Type = tool.Type;
            CurrentProgress = tool.CurrentProgress;
            _currentState = tool._currentState;
            OutputItem = tool.OutputItem;
        }
    }
}
