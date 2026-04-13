using UnityEngine;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 食材按钮管理器
    /// 控制食材面板的展开和收起
    /// </summary>
    public class IngredientButtonManager : MonoBehaviour
    {
        /// <summary>
        /// 食材面板
        /// </summary>
        [SerializeField] private GameObject _ingredientPanel;

        /// <summary>
        /// 食材按钮
        /// </summary>
        [SerializeField] private GameObject _ingredientButton;

        /// <summary>
        /// 食材管理器引用
        /// </summary>
        private IngredientManager _ingredientManager;

        /// <summary>
        /// 是否面板已打开
        /// </summary>
        private bool _isPanelOpen = false;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            // 尝试获取IngredientManager
            _ingredientManager = FindObjectOfType<IngredientManager>();
        }

        /// <summary>
        /// 切换面板显示状态
        /// </summary>
        public void TogglePanel()
        {
            _isPanelOpen = !_isPanelOpen;

            if (_isPanelOpen)
            {
                // 显示面板
                _ingredientPanel.SetActive(true);

                // 刷新食材列表
                RefreshIngredientList();
            }
            else
            {
                // 隐藏面板
                _ingredientPanel.SetActive(false);
            }
        }

        /// <summary>
        /// 刷新食材列表
        /// </summary>
        private void RefreshIngredientList()
        {
            // 强制刷新食材管理器的UI
            if (_ingredientManager != null)
            {
                // 调用Update方法刷新显示
                _ingredientManager.Update();
            }
        }
    }
}
