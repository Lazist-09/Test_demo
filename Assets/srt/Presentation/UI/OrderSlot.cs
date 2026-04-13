using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 订单槽位
    /// 显示订单信息,用于匹配菜品
    /// </summary>
    public class OrderSlot : MonoBehaviour
    {
        /// <summary>
        /// 槽位图像组件
        /// </summary>
        [SerializeField] private UnityEngine.UI.Image _slotImage;

        /// <summary>
        /// 订单ID
        /// </summary>
        private string _orderId;

        /// <summary>
        /// 订单管理用例
        /// </summary>
        private OrderManagementUseCase _orderUseCase;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="orderUseCase">订单管理用例</param>
        /// <param name="orderId">订单ID</param>
        public void Initialize(OrderManagementUseCase orderUseCase, string orderId)
        {
            _orderUseCase = orderUseCase;
            _orderId = orderId;
        }

        /// <summary>
        /// 获取订单ID
        /// </summary>
        /// <returns>订单ID</returns>
        public string GetOrderId()
        {
            return _orderId;
        }
    }
}
