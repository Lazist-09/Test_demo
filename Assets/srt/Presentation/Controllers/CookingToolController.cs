using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;

namespace CookingGame.Presentation.Controllers
{
    /// <summary>
    /// 烹饪工具控制器
    /// 处理烹饪工具的交互逻辑
    /// 支持点击启动/停止和长按推进进度
    /// </summary>
    public class CookingToolController : MonoBehaviour
    {
        /// <summary>
        /// 工具ID
        /// </summary>
        public string ToolId { get; set; }
        
        /// <summary>
        /// 用例实例
        /// </summary>
        private CookingToolManagementUseCase _useCase;
        
        /// <summary>
        /// 用例实例
        /// </summary>
        private ItemManagementUseCase _itemUseCase;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="useCase">烹饪工具管理用例</param>
        public void Initialize(CookingToolManagementUseCase useCase)
        {
            Debug.Log($"CookingToolController.Initialize(CookingToolManagementUseCase): ToolId={ToolId}");
            _useCase = useCase;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="useCase">物品管理用例</param>
        public void Initialize(ItemManagementUseCase useCase)
        {
            Debug.Log($"CookingToolController.Initialize(ItemManagementUseCase): ToolId={ToolId}");
            _itemUseCase = useCase;
        }

        /// <summary>
        /// 鼠标点击事件
        /// 启动工具或停止工具并输出菜品
        /// </summary>
        public void OnClick()
        {
            Debug.Log($"CookingToolController.OnClick: ToolId={ToolId}");
            
            if (_useCase == null)
            {
                Debug.LogError("_useCase is null!");
                return;
            }

            var toolDto = _useCase.GetTool(ToolId);
            if (toolDto == null)
            {
                Debug.LogError($"Tool {ToolId} not found!");
                return;
            }

            Debug.Log($"Tool status: IsRunning={toolDto.IsRunning}");
            
            if (!toolDto.IsRunning)
            {
                StartTool();
            }
            else
            {
                CompleteAndOutput();
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// 长按推进烹饪进度
        /// </summary>
        public void OnMouseDown()
        {
            Debug.Log($"CookingToolController.OnMouseDown: ToolId={ToolId}");
            if (_useCase == null) return;
            var toolDto = _useCase.GetTool(ToolId);
            if (toolDto != null && toolDto.IsRunning)
            {
                AddProgress();
            }
        }

        /// <summary>
        /// 鼠标抬起事件
        /// 停止长按操作
        /// </summary>
        public void OnMouseUp()
        {
            Debug.Log($"CookingToolController.OnMouseUp: ToolId={ToolId}");
            if (_useCase == null) return;
            var toolDto = _useCase.GetTool(ToolId);
            if (toolDto != null && toolDto.IsRunning)
            {
                Debug.Log($"Stopping tool: {ToolId}");
                StopTool();
            }
        }

        /// <summary>
        /// 启动工具
        /// </summary>
        private void StartTool()
        {
            _useCase.StartCooking(ToolId);
        }

        /// <summary>
        /// 停止工具
        /// </summary>
        private void StopTool()
        {
            _useCase.PauseCooking(ToolId);
        }

        /// <summary>
        /// 完成烹饪并输出菜品
        /// </summary>
        private void CompleteAndOutput()
        {
            Debug.Log($"CookingToolController.CompleteAndOutput: ToolId={ToolId}");
            _useCase.CompleteCooking(ToolId);
        }

        /// <summary>
        /// 增加工具进度
        /// </summary>
        private void AddProgress()
        {
            _useCase.AddToolProgress(ToolId, 0.1f);
        }
    }
}
