using UnityEngine;
using System.Collections.Generic;
using CookingGame.Application.UseCases;
using CookingGame.Core.Events;
using CookingGame.Core.Models;
using CookingGame.Presentation.Views;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 菜品输出管理器
    /// 管理菜品输出的显示和处理
    /// </summary>
    public class DishOutputManager : MonoBehaviour
    {
        /// <summary>
        /// 烹饪工具管理用例
        /// </summary>
        private CookingToolManagementUseCase _toolUseCase;

        /// <summary>
        /// 菜品输出容器
        /// </summary>
        [SerializeField] private Transform _outputContainer;

        /// <summary>
        /// 菜品预制体
        /// </summary>
        [SerializeField] private GameObject _dishPrefab;

        /// <summary>
        /// 当前输出的菜品
        /// </summary>
        private string _currentDishId;
        
        /// <summary>
        /// 已处理的工具ID集合
        /// </summary>
        private HashSet<string> _processedToolIds = new HashSet<string>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="toolUseCase">烹饪工具管理用例</param>
        public void Initialize(CookingToolManagementUseCase toolUseCase)
        {
            _toolUseCase = toolUseCase;
            
            // 订阅工具完成事件
            GameEvents.SubscribeToolCompleted(OnToolCompleted);
        }
        
        /// <summary>
        /// 工具完成事件处理
        /// </summary>
        /// <param name="tool">完成的工具</param>
        private void OnToolCompleted(CookingTool tool)
        {
            // 检查是否有输出
            var outputItem = _toolUseCase.GetToolOutput(tool.Id);
            if (outputItem != null && !string.IsNullOrEmpty(outputItem.Id) && 
                !_processedToolIds.Contains(tool.Id))
            {
                Debug.Log($"DishOutputManager.OnToolCompleted: ToolId={tool.Id}, OutputItemId={outputItem.Id}");
                
                // 显示菜品
                ShowDishOutput(tool.Id);
                
                // 标记为已处理
                _processedToolIds.Add(tool.Id);
                
                // 清空工具输出
                _toolUseCase.ClearToolOutput(tool.Id);
                Debug.Log($"Cleared tool output: {tool.Id}");
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 此方法已废弃,改为事件驱动
            // 保留以兼容旧代码,但不再调用
        }
        
        /// <summary>
        /// 检查新的菜品输出
        /// </summary>
        private void CheckForNewDishOutput()
        {
            // 此方法已废弃,改为事件驱动
            // 保留以兼容旧代码,但不再调用
        }

        /// <summary>
        /// 显示菜品输出
        /// </summary>
        /// <param name="dishItemId">菜品物品ID</param>
        private void ShowDishOutput(string dishItemId)
        {
            // 清空之前的输出
            ClearOutput();

            // 获取菜品信息
            var item = _toolUseCase.GetToolOutput(dishItemId);
            if (item == null) return;

            // 创建菜品 UI
            if (_dishPrefab != null && _outputContainer != null)
            {
                var dishObj = Instantiate(_dishPrefab, _outputContainer);
                
                // 获取 ItemView 组件
                var itemView = dishObj.GetComponent<ItemView>();
                if (itemView != null)
                {
                    itemView.Initialize(item);
                }

                // 设置菜品名称
                var text = dishObj.GetComponent<UnityEngine.UI.Text>();
                if (text != null)
                {
                    text.text = item.Name;
                }

                _currentDishId = dishItemId;
            }
        }

        /// <summary>
        /// 清空输出
        /// </summary>
        private void ClearOutput()
        {
            // 销毁所有子对象
            foreach (Transform child in _outputContainer)
            {
                Destroy(child.gameObject);
            }
            
            _currentDishId = null;
        }

        /// <summary>
        /// 获取当前输出的菜品
        /// </summary>
        /// <returns>菜品物品ID，如果没有则返回 null</returns>
        public string GetCurrentDishId()
        {
            return _currentDishId;
        }

        /// <summary>
        /// 确认菜品
        /// </summary>
        /// <returns>是否成功</returns>
        public bool ConfirmDish()
        {
            if (string.IsNullOrEmpty(_currentDishId)) return false;

            // 清空输出
            ClearOutput();
            
            return true;
        }

        /// <summary>
        /// 丢弃菜品
        /// </summary>
        /// <returns>是否成功</returns>
        public bool DiscardDish()
        {
            if (string.IsNullOrEmpty(_currentDishId)) return false;

            // 清空输出
            ClearOutput();
            
            return true;
        }
        
        /// <summary>
        /// 清理资源
        /// </summary>
        private void OnDestroy()
        {
            // 取消订阅事件
            GameEvents.UnsubscribeToolCompleted(OnToolCompleted);
        }
    }
}
