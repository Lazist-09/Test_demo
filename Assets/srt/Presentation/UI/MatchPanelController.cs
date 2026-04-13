using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Infrastructure;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 匹配面板控制器
    /// 处理菜品和订单的匹配逻辑
    /// </summary>
    public class MatchPanelController : MonoBehaviour
    {
        /// <summary>
        /// 菜品槽的图像组件
        /// </summary>
        [SerializeField] private UnityEngine.UI.Image _dishSlotImage;

        /// <summary>
        /// 订单槽的图像组件
        /// </summary>
        [SerializeField] private UnityEngine.UI.Image _orderSlotImage;

        /// <summary>
        /// 提交按钮组件
        /// </summary>
        [SerializeField] private UnityEngine.UI.Button _submitButton;

        /// <summary>
        /// 订单管理器引用
        /// </summary>
        private OrderManager _orderManager;

        /// <summary>
        /// 当前选择的菜品ID
        /// </summary>
        private string _selectedDishId;

        /// <summary>
        /// 当前选择的订单ID
        /// </summary>
        private string _selectedOrderId;

        /// <summary>
        /// 订单提交用例
        /// </summary>
        private OrderSubmissionUseCase _orderSubmissionUseCase;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            Debug.Log("MatchPanelController Start()");

            // 获取订单管理器
            _orderManager = FindObjectOfType<OrderManager>();
            if (_orderManager == null)
            {
                Debug.LogError("OrderManager not found!");
            }

            // 获取订单提交用例
            _orderSubmissionUseCase = ServiceLocator.Instance.OrderSubmissionUseCase;
            if (_orderSubmissionUseCase == null)
            {
                Debug.LogError("OrderSubmissionUseCase is null!");
            }

            // 初始化提交按钮
            InitializeSubmitButton();
            
            Debug.Log("MatchPanelController initialized");
        }

        /// <summary>
        /// 初始化提交按钮
        /// </summary>
        private void InitializeSubmitButton()
        {
            Debug.Log("InitializeSubmitButton()");

            if (_submitButton == null)
            {
                Debug.LogError("_submitButton is null! Check if the Button is assigned in Inspector");
                return;
            }
            
            Debug.Log($"_submitButton found: {_submitButton.name}, type: {_submitButton.GetType()}");

            // 移除旧的事件监听
            _submitButton.onClick.RemoveAllListeners();
            
            // 添加新的事件监听
            _submitButton.onClick.AddListener(SubmitMatchedOrder);
            
            Debug.Log($"Button onClick listeners count: {_submitButton.onClick.GetPersistentEventCount()}");
            
            // 更新按钮状态
            UpdateSubmitButtonState();
        }

        /// <summary>
        /// 选择菜品
        /// </summary>
        /// <param name="dishItemId">菜品物品ID</param>
        public void SelectDish(string dishItemId)
        {
            _selectedDishId = dishItemId;
            UpdateDishSlotVisuals();
            UpdateSubmitButtonState();
        }

        /// <summary>
        /// 选择订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        public void SelectOrder(string orderId)
        {
            _selectedOrderId = orderId;
            UpdateOrderSlotVisuals();
            UpdateSubmitButtonState();
        }

        /// <summary>
        /// 清空菜品选择
        /// </summary>
        public void ClearDishSelection()
        {
            _selectedDishId = null;
            UpdateDishSlotVisuals();
            UpdateSubmitButtonState();
        }

        /// <summary>
        /// 清空订单选择
        /// </summary>
        public void ClearOrderSelection()
        {
            _selectedOrderId = null;
            UpdateOrderSlotVisuals();
            UpdateSubmitButtonState();
        }

        /// <summary>
        /// 提交匹配的订单
        /// </summary>
        public void SubmitMatchedOrder()
        {
            Debug.Log($"SubmitMatchedOrder() called! dishId={_selectedDishId}, orderId={_selectedOrderId}");
            
            if (string.IsNullOrEmpty(_selectedDishId) || string.IsNullOrEmpty(_selectedOrderId))
            {
                Debug.LogWarning("请先选择菜品和订单");
                return;
            }

            // 提交订单
            bool success = SubmitOrderToManager(_selectedOrderId, _selectedDishId);

            if (success)
            {
                Debug.Log($"订单提交成功: 订单ID={_selectedOrderId}, 菜品ID={_selectedDishId}");

                // 清空选择
                ClearDishSelection();
                ClearOrderSelection();
            }
            else
            {
                Debug.LogError("订单提交失败，请检查菜品和订单是否匹配");
            }
        }

        /// <summary>
        /// 提交订单到管理器
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="dishItemId">菜品物品ID</param>
        /// <returns>是否成功</returns>
        private bool SubmitOrderToManager(string orderId, string dishItemId)
        {
            if (_orderManager == null)
            {
                Debug.LogError("OrderManager未找到");
                return false;
            }

            return _orderManager.SubmitOrder(orderId, dishItemId);
        }

        /// <summary>
        /// 更新菜品槽的视觉效果
        /// </summary>
        private void UpdateDishSlotVisuals()
        {
            if (_dishSlotImage == null) return;

            if (string.IsNullOrEmpty(_selectedDishId))
            {
                // 未选择菜品 - 灰色
                _dishSlotImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else
            {
                // 已选择菜品 - 绿色
                _dishSlotImage.color = new Color(0.5f, 1f, 0.5f, 1f);
            }
        }

        /// <summary>
        /// 更新订单槽的视觉效果
        /// </summary>
        private void UpdateOrderSlotVisuals()
        {
            if (_orderSlotImage == null) return;

            if (string.IsNullOrEmpty(_selectedOrderId))
            {
                // 未选择订单 - 灰色
                _orderSlotImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else
            {
                // 已选择订单 - 黄色
                _orderSlotImage.color = new Color(1f, 0.8f, 0f, 1f);
            }
        }

        /// <summary>
        /// 设置当前选择的菜品
        /// </summary>
        /// <param name="dishItemId">菜品ID</param>
        public void SetSelectedDish(string dishItemId)
        {
            _selectedDishId = dishItemId;
            UpdateDishSlotVisuals();
            UpdateSubmitButtonState();
        }

        /// <summary>
        /// 设置当前选择的订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        public void SetSelectedOrder(string orderId)
        {
            _selectedOrderId = orderId;
            UpdateOrderSlotVisuals();
            UpdateSubmitButtonState();
        }

        /// <summary>
        /// 获取当前选择的菜品ID
        /// </summary>
        /// <returns>菜品ID</returns>
        public string GetSelectedDishId()
        {
            return _selectedDishId;
        }

        /// <summary>
        /// 获取当前选择的订单ID
        /// </summary>
        /// <returns>订单ID</returns>
        public string GetSelectedOrderId()
        {
            return _selectedOrderId;
        }

        /// <summary>
        /// 更新提交按钮状态
        /// 根据是否选择了菜品和订单来启用/禁用按钮
        /// </summary>
        private void UpdateSubmitButtonState()
        {
            if (_submitButton == null) return;

            // 只有当菜品和订单都已选择时才启用按钮
            bool canSubmit = !string.IsNullOrEmpty(_selectedDishId) && !string.IsNullOrEmpty(_selectedOrderId);
            _submitButton.interactable = canSubmit;

            // 更新按钮颜色
            if (canSubmit)
            {
                // 启用状态 - 绿色
                _submitButton.image.color = new Color(0.5f, 1f, 0.5f, 1f);
            }
            else
            {
                // 禁用状态 - 灰色
                _submitButton.image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }
}
