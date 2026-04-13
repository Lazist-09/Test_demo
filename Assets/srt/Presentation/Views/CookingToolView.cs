using UnityEngine;
using CookingGame.Application.UseCases;

namespace CookingGame.Presentation.Views
{
    /// <summary>
    /// 烹饪工具视图
    /// 负责显示烹饪工具的视觉效果
    /// 根据工具的状态和进度更新外观
    /// </summary>
    public class CookingToolView : MonoBehaviour
    {
        /// <summary>
        /// 工具ID
        /// </summary>
        [SerializeField] private string _toolId;
        
        /// <summary>
        /// 烹饪工具管理用例
        /// </summary>
        private CookingToolManagementUseCase _useCase;
        
        /// <summary>
        /// 背景图像组件(可选,在 Inspector 中拖拽设置)
        /// </summary>
        [SerializeField] private UnityEngine.UI.Image _backgroundImage;
        
        /// <summary>
        /// 进度条组件(可选,在 Inspector 中拖拽设置)
        /// </summary>
        [SerializeField] private UnityEngine.UI.Slider _progressSlider;
        
        /// <summary>
        /// 名称文本组件(可选,在 Inspector 中拖拽设置)
        /// </summary>
        [SerializeField] private UnityEngine.UI.Text _nameText;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="useCase">烹饪工具管理用例</param>
        /// <param name="toolId">工具ID</param>
        public void Initialize(CookingToolManagementUseCase useCase, string toolId)
        {
            _useCase = useCase;
            _toolId = toolId;
            InitializeComponents();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponents()
        {
            // 如果没有在 Inspector 中设置,尝试自动查找
            if (_backgroundImage == null)
            {
                Transform backgroundTransform = transform.Find("Background");
                if (backgroundTransform != null)
                {
                    _backgroundImage = backgroundTransform.GetComponent<UnityEngine.UI.Image>();
                }
            }
            
            if (_progressSlider == null)
            {
                Transform progressTransform = transform.Find("Progress");
                if (progressTransform != null)
                {
                    _progressSlider = progressTransform.GetComponent<UnityEngine.UI.Slider>();
                }
            }
            
            if (_nameText == null)
            {
                Transform labelTransform = transform.Find("Label");
                if (labelTransform != null)
                {
                    _nameText = labelTransform.GetComponent<UnityEngine.UI.Text>();
                }
                
                if (_nameText == null)
                {
                    _nameText = GetComponentInChildren<UnityEngine.UI.Text>();
                }
            }
        }

        /// <summary>
        /// 更新视觉效果
        /// 根据工具数据更新外观
        /// </summary>
        public void UpdateVisuals()
        {
            if (_useCase != null)
            {
                var toolDto = _useCase.GetTool(_toolId);
                if (toolDto != null)
                {
                    Debug.Log($"CookingToolView.UpdateVisuals: {toolDto.Id}, IsRunning={toolDto.IsRunning}, Progress={toolDto.CurrentProgress}");
                    UpdateProgressVisuals(toolDto.CurrentProgress);
                    UpdateStateVisuals(toolDto.IsRunning);
                    UpdateNameVisuals(toolDto.Type.ToString());
                }
            }
        }
        
        /// <summary>
        /// 更新名称视觉效果
        /// </summary>
        /// <param name="name">工具名称</param>
        private void UpdateNameVisuals(string name)
        {
            if (_nameText != null)
            {
                _nameText.text = name;
            }
        }

        /// <summary>
        /// 更新进度视觉效果
        /// 更新进度条的视觉显示
        /// </summary>
        /// <param name="progress">进度值</param>
        private void UpdateProgressVisuals(float progress)
        {
            if (_progressSlider != null)
            {
                _progressSlider.value = progress;
            }
        }

        /// <summary>
        /// 更新状态视觉效果
        /// 根据工具状态更新外观
        /// </summary>
        /// <param name="isRunning">是否正在运行</param>
        private void UpdateStateVisuals(bool isRunning)
        {
            if (_backgroundImage == null) return;
            
            if (isRunning)
            {
                _backgroundImage.color = new Color(0.5f, 1f, 0.5f, 1f);
            }
            else
            {
                _backgroundImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            }
        }
    }
}
