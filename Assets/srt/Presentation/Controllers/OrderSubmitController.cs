using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;

namespace CookingGame.Presentation.Controllers
{
    /// <summary>
    /// 订单提交控制器
    /// 处理订单提交的交互逻辑
    /// </summary>
    public class OrderSubmitController : MonoBehaviour
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { get; set; }
        
        /// <summary>
        /// 订单提交用例
        /// </summary>
        private OrderSubmissionUseCase _useCase;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="useCase">订单提交用例</param>
        public void Initialize(OrderSubmissionUseCase useCase)
        {
            _useCase = useCase;
        }

        /// <summary>
        /// 提交按钮点击事件
        /// 提交菜品到订单
        /// </summary>
        /// <param name="dishItemId">菜品物品ID</param>
        public void OnSubmitButtonClicked(string dishItemId)
        {
            if (_useCase == null) return;

            var isValid = _useCase.ValidateDishForOrder(OrderId, dishItemId);
            if (!isValid)
            {
                Debug.Log("Invalid dish for this order");
                return;
            }

            var reward = _useCase.CalculateOrderReward(OrderId, dishItemId);
            var orderDto = _useCase.SubmitOrder(OrderId, dishItemId);

            if (orderDto != null)
            {
                Debug.Log($"Order submitted! Reward: {reward}");
            }
            else
            {
                Debug.Log("Failed to submit order");
            }
        }

        /// <summary>
        /// 按铃按钮点击事件
        /// 请求订单提交
        /// </summary>
        public void OnBellButtonClicked()
        {
            Debug.Log("Bell rang! Order submission requested");
        }
    }
}
