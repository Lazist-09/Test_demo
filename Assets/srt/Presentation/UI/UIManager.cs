using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Presentation.Views;
using CookingGame.Infrastructure;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// UI 管理器
    /// 统一管理所有 UI 界面和组件
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        public static UIManager Instance { get; private set; }

        /// <summary>
        /// Canvas 对象
        /// </summary>
        [SerializeField] private Canvas _canvas;

        /// <summary>
        /// 主菜单面板
        /// </summary>
        [SerializeField] private GameObject _menuPanel;

        /// <summary>
        /// 厨房面板
        /// </summary>
        [SerializeField] private GameObject _kitchenPanel;

        /// <summary>
        /// 订单详情面板
        /// </summary>
        [SerializeField] private GameObject _orderDetailPanel;

        /// <summary>
        /// 物品详情面板
        /// </summary>
        [SerializeField] private GameObject _itemDetailPanel;

        /// <summary>
        /// 用例引用
        /// </summary>
        private ItemManagementUseCase _itemUseCase;
        private RecipeManagementUseCase _recipeUseCase;
        private OrderManagementUseCase _orderUseCase;
        private CookingToolManagementUseCase _toolUseCase;

        /// <summary>
        /// 当前显示的面板
        /// </summary>
        private GameObject _currentPanel;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Awake()
        {
            // 设置单例
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // 获取用例引用
            _itemUseCase = ServiceLocator.Instance.ItemUseCase;
            _recipeUseCase = ServiceLocator.Instance.RecipeUseCase;
            _orderUseCase = ServiceLocator.Instance.OrderUseCase;
            _toolUseCase = ServiceLocator.Instance.ToolUseCase;

            // 初始化 UI
            InitializeUI();
        }

        /// <summary>
        /// 初始化 UI
        /// </summary>
        private void InitializeUI()
        {
            // 隐藏所有面板
            HideAllPanels();

            // 显示主菜单
            ShowPanel(_menuPanel);
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="panel">要显示的面板</param>
        public void ShowPanel(GameObject panel)
        {
            if (panel == null) return;

            // 隐藏当前面板
            if (_currentPanel != null)
            {
                _currentPanel.SetActive(false);
            }

            // 显示新面板
            panel.SetActive(true);
            _currentPanel = panel;
        }

        /// <summary>
        /// 隐藏所有面板
        /// </summary>
        private void HideAllPanels()
        {
            if (_menuPanel != null) _menuPanel.SetActive(false);
            if (_kitchenPanel != null) _kitchenPanel.SetActive(false);
            if (_orderDetailPanel != null) _orderDetailPanel.SetActive(false);
            if (_itemDetailPanel != null) _itemDetailPanel.SetActive(false);
        }

        /// <summary>
        /// 显示主菜单
        /// </summary>
        public void ShowMenu()
        {
            ShowPanel(_menuPanel);
        }

        /// <summary>
        /// 显示厨房
        /// </summary>
        public void ShowKitchen()
        {
            HideAllPanels();
            _kitchenPanel.SetActive(true);
            _currentPanel = _kitchenPanel;
        }

        /// <summary>
        /// 显示订单详情
        /// </summary>
        /// <param name="orderId">订单ID</param>
        public void ShowOrderDetail(string orderId)
        {
            // 获取订单数据
            var orderDto = _orderUseCase.GetOrderById(orderId);
            if (orderDto == null) return;

            // 更新订单详情 UI
            // 这里需要实现具体的 UI 更新逻辑

            ShowPanel(_orderDetailPanel);
        }

        /// <summary>
        /// 显示物品详情
        /// </summary>
        /// <param name="itemId">物品ID</param>
        public void ShowItemDetail(string itemId)
        {
            // 获取物品数据
            var itemDto = _itemUseCase.GetItem(itemId);
            if (itemDto == null) return;

            // 更新物品详情 UI
            // 这里需要实现具体的 UI 更新逻辑

            ShowPanel(_itemDetailPanel);
        }

        /// <summary>
        /// 更新所有视图
        /// </summary>
        public void UpdateAllViews()
        {
            // 更新厨房中的所有视图
            // 这里需要实现具体的更新逻辑
        }
    }
}
