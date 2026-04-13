using UnityEngine;
using System.Collections.Generic;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;
using CookingGame.Presentation.Views;
using CookingGame.Presentation.Controllers;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 烹饪工具管理器
    /// 管理烹饪工具的显示、进度和交互
    /// </summary>
    public class CookingToolManager : MonoBehaviour
    {
        /// <summary>
        /// 烹饪工具管理用例
        /// </summary>
        private CookingToolManagementUseCase _toolUseCase;

        /// <summary>
        /// 烹饪工具容器
        /// </summary>
        [SerializeField] private Transform _toolsContainer;

        /// <summary>
        /// 烹饪工具预制体
        /// </summary>
        [SerializeField] private GameObject _toolPrefab;

        /// <summary>
        /// 烹饪工具数据
        /// </summary>
        private List<CookingToolData> _tools = new List<CookingToolData>();

        /// <summary>
        /// 烹饪工具数据结构
        /// </summary>
        private struct CookingToolData
        {
            public string ToolId;
            public CookingToolType Type;
            public float Progress;
            public bool IsRunning;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="toolUseCase">烹饪工具管理用例</param>
        public void Initialize(CookingToolManagementUseCase toolUseCase)
        {
            Debug.Log("CookingToolManager.Initialize()");
            
            _toolUseCase = toolUseCase;

            // 初始化默认烹饪工具
            InitializeDefaultTools();
        }

        /// <summary>
        /// 初始化默认烹饪工具
        /// </summary>
        private void InitializeDefaultTools()
        {
            Debug.Log("InitializeDefaultTools()");
            
            // 创建平底锅
            CreateTool(CookingToolType.Pan);
            
            // 创建锅
            CreateTool(CookingToolType.Pot);
            
            // 创建烤箱
            CreateTool(CookingToolType.Oven);
        }

        /// <summary>
        /// 创建烹饪工具
        /// </summary>
        /// <param name="type">工具类型</param>
        private void CreateTool(CookingToolType type)
        {
            Debug.Log($"CreateTool: type={type}");
            
            // 调用UseCase在仓储中创建工具
            var toolDto = _toolUseCase.CreateTool(type);
            if (toolDto == null)
            {
                Debug.LogError($"Failed to create tool for type {type}");
                return;
            }
            
            Debug.Log($"Tool created: {toolDto.Id}, Type: {toolDto.Type}");
            
            // 创建工具数据
            var toolData = new CookingToolData
            {
                ToolId = toolDto.Id,
                Type = type,
                Progress = 0f,
                IsRunning = false
            };

            Debug.Log($"Tool data created: {toolData.ToolId}");
            _tools.Add(toolData);

            // 创建 UI 对象
            CreateToolUI(toolData);
        }

        /// <summary>
        /// 创建烹饪工具 UI
        /// </summary>
        /// <param name="data">烹饪工具数据</param>
        private void CreateToolUI(CookingToolData data)
        {
            Debug.Log($"CreateToolUI: {data.ToolId}");
            
            if (_toolPrefab == null)
            {
                Debug.LogError("_toolPrefab is null!");
                return;
            }
            
            if (_toolsContainer == null)
            {
                Debug.LogError("_toolsContainer is null!");
                return;
            }

            // 实例化 UI 对象
            var toolObj = Instantiate(_toolPrefab, _toolsContainer);
            Debug.Log($"Tool object instantiated: {toolObj.name}");
            
            // 设置工具 ID
            var controller = toolObj.GetComponent<CookingToolController>();
            if (controller != null)
            {
                controller.Initialize(_toolUseCase);
                controller.ToolId = data.ToolId;
                Debug.Log($"CookingToolController initialized with ToolId={data.ToolId}");
                
                // 添加 EventTrigger 组件来处理鼠标事件
                var eventTrigger = toolObj.AddComponent<UnityEngine.EventSystems.EventTrigger>();
                
                var onClick = new UnityEngine.EventSystems.EventTrigger.Entry
                {
                    eventID = UnityEngine.EventSystems.EventTriggerType.PointerClick
                };
                onClick.callback.AddListener((eventData) => controller.OnClick());
                eventTrigger.triggers.Add(onClick);
            }
            else
            {
                Debug.LogError("CookingToolController not found on tool prefab!");
            }

            // 获取 CookingToolView 组件
            var toolView = toolObj.GetComponent<CookingToolView>();
            if (toolView != null)
            {
                toolView.Initialize(_toolUseCase, data.ToolId);
                Debug.Log($"CookingToolView initialized with ToolId={data.ToolId}");
            }
            else
            {
                Debug.LogError("CookingToolView not found on tool prefab!");
            }

            // 设置工具名称
            var text = toolObj.GetComponent<UnityEngine.UI.Text>();
            if (text != null)
            {
                text.text = data.Type.ToString();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 更新所有烹饪工具
            UpdateTools();
        }

        /// <summary>
        /// 更新烹饪工具
        /// </summary>
        private void UpdateTools()
        {
            for (int i = 0; i < _tools.Count; i++)
            {
                var tool = _tools[i];
                
                // 如果工具正在运行，增加进度
                if (tool.IsRunning)
                {
                    tool.Progress += Time.deltaTime * 0.1f; // 每秒增加 10% 进度
                    
                    // 如果进度达到 100%，停止运行
                    if (tool.Progress >= 1.0f)
                    {
                        tool.Progress = 1.0f;
                        tool.IsRunning = false;
                        StopTool(tool.ToolId);
                    }
                    
                    // 更新列表中的工具数据
                    _tools[i] = tool;
                }
            }
        }

        /// <summary>
        /// 启动工具
        /// </summary>
        /// <param name="toolId">工具ID</param>
        public void StartTool(string toolId)
        {
            var tool = _tools.Find(t => t.ToolId == toolId);
            if (!IsDefault(tool))
            {
                tool.IsRunning = true;
                _toolUseCase.StartCooking(toolId);
            }
        }

        /// <summary>
        /// 停止工具
        /// </summary>
        /// <param name="toolId">工具ID</param>
        public void StopTool(string toolId)
        {
            var tool = _tools.Find(t => t.ToolId == toolId);
            if (!IsDefault(tool))
            {
                tool.IsRunning = false;
                _toolUseCase.PauseCooking(toolId);
            }
        }

        /// <summary>
        /// 增加工具进度
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <param name="amount">增加的进度值</param>
        public void AddToolProgress(string toolId, float amount)
        {
            var tool = _tools.Find(t => t.ToolId == toolId);
            if (!IsDefault(tool))
            {
                tool.Progress += amount;
                if (tool.Progress > 1.0f) tool.Progress = 1.0f;
                
                _toolUseCase.AddToolProgress(toolId, amount);
            }
        }

        /// <summary>
        /// 获取工具进度
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <returns>进度值 (0-1)</returns>
        public float GetToolProgress(string toolId)
        {
            var tool = _tools.Find(t => t.ToolId == toolId);
            return !IsDefault(tool) ? tool.Progress : 0f;
        }

        /// <summary>
        /// 获取工具状态
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <returns>是否正在运行</returns>
        public bool IsToolRunning(string toolId)
        {
            var tool = _tools.Find(t => t.ToolId == toolId);
            return !IsDefault(tool) && tool.IsRunning;
        }
        
        /// <summary>
        /// 获取工具容器
        /// </summary>
        /// <returns>工具容器</returns>
        public Transform ToolsContainer
        {
            get { return _toolsContainer; }
        }

        /// <summary>
        /// 检查数据是否为默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否为默认值</returns>
        private bool IsDefault(CookingToolData data)
        {
            return data.ToolId == null;
        }
    }
}
