using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;

namespace CookingGame.Presentation.Views
{
    /// <summary>
    /// 订单视图
    /// 负责显示订单的视觉效果
    /// 根据订单状态更新UI
    /// </summary>
    public class OrderView : MonoBehaviour
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [SerializeField] private string _orderId;
        
        /// <summary>
        /// 订单管理用例
        /// </summary>
        private OrderManagementUseCase _useCase;
        
        /// <summary>
        /// 背景图像组件(可选,在 Inspector 中拖拽设置)
        /// </summary>
        [SerializeField] private UnityEngine.UI.Image _backgroundImage;
        
        /// <summary>
        /// 名称文本组件(可选,在 Inspector 中拖拽设置)
        /// </summary>
        [SerializeField] private UnityEngine.UI.Text _nameText;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="useCase">订单管理用例</param>
        /// <param name="orderId">订单ID</param>
        public void Initialize(OrderManagementUseCase useCase, string orderId)
        {
            _useCase = useCase;
            _orderId = orderId;
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
        /// 根据订单数据更新UI
        /// </summary>
        public void UpdateVisuals()
        {
            if (_useCase != null)
            {
                var orderDto = _useCase.GetOrderById(_orderId);
                if (orderDto != null)
                {
                    UpdateStatusVisuals(orderDto.Status);
                    UpdateNameVisuals(orderDto.Recipe?.Name ?? "未知订单");
                }
            }
        }
        
        /// <summary>
        /// 更新名称视觉效果
        /// </summary>
        /// <param name="name">订单名称</param>
        private void UpdateNameVisuals(string name)
        {
            if (_nameText != null)
            {
                _nameText.text = name;
            }
        }

        /// <summary>
        /// 更新状态视觉效果
        /// 根据订单状态更新UI
        /// 待处理 = 黄色, 已提交 = 蓝色, 已完成 = 绿色
        /// </summary>
        /// <param name="status">订单状态</param>
        private void UpdateStatusVisuals(OrderStatus status)
        {
            if (_backgroundImage == null) return;
            
            switch (status)
            {
                case OrderStatus.Pending:
                    _backgroundImage.color = new Color(1f, 0.8f, 0f, 1f);
                    break;
                case OrderStatus.Submitted:
                    _backgroundImage.color = new Color(0.5f, 0.5f, 1f, 1f);
                    break;
                case OrderStatus.Completed:
                    _backgroundImage.color = new Color(0.5f, 1f, 0.5f, 1f);
                    break;
            }
        }
    }
}
