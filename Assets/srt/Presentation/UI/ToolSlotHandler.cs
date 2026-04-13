using UnityEngine;
using UnityEngine.EventSystems;
using CookingGame.Application.UseCases;
using CookingGame.Infrastructure;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 工具槽处理器
    /// 处理工具槽的点击交互
    /// </summary>
    public class ToolSlotHandler : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// 工具ID
        /// </summary>
        [SerializeField] private string _toolId;

        /// <summary>
        /// 烹饪工具管理用例
        /// </summary>
        private CookingToolManagementUseCase _toolUseCase;

        /// <summary>
        /// 指针点击事件
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(_toolId))
            {
                Debug.LogWarning("工具ID为空");
                return;
            }

            // 获取工具状态
            var toolDto = _toolUseCase?.GetTool(_toolId);

            if (toolDto == null)
            {
                Debug.LogWarning($"无法找到工具: {_toolId}");
                return;
            }

            // 根据点击类型处理
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // 左键点击 - 切换工具启动/停止
                ToggleToolState();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // 右键点击 - 清空工具
                ClearTool();
            }
        }

        /// <summary>
        /// 切换工具状态
        /// </summary>
        private void ToggleToolState()
        {
            var toolDto = _toolUseCase?.GetTool(_toolId);

            if (toolDto == null) return;

            if (toolDto.IsRunning)
            {
                // 停止工具
                _toolUseCase?.PauseCooking(_toolId);
                Debug.Log($"工具 {_toolId} 已停止");
            }
            else
            {
                // 启动工具
                _toolUseCase?.StartCooking(_toolId);
                Debug.Log($"工具 {_toolId} 已启动");
            }
        }

        /// <summary>
        /// 清空工具
        /// </summary>
        private void ClearTool()
        {
            var toolDto = _toolUseCase?.GetTool(_toolId);

            if (toolDto == null) return;

            // 检查工具是否可以清空
            if (!toolDto.IsRunning)
            {
                // 清空输入
                _toolUseCase?.ClearToolInput(_toolId);
                Debug.Log($"工具 {_toolId} 已清空");
            }
            else
            {
                Debug.LogWarning("工具正在运行，无法清空");
            }
        }

        /// <summary>
        /// 设置工具ID
        /// </summary>
        /// <param name="id">工具ID</param>
        public void SetToolId(string id)
        {
            _toolId = id;
        }

        /// <summary>
        /// 获取工具ID
        /// </summary>
        /// <returns>工具ID</returns>
        public string GetToolId()
        {
            return _toolId;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            // 获取用例
            _toolUseCase = CookingGame.Infrastructure.ServiceLocator.Instance.ToolUseCase;
        }
    }
}
