using UnityEngine;
using System.Collections.Generic;
using CookingGame.Application.UseCases;
using CookingGame.Core.Events;
using CookingGame.Core.Models;
using CookingGame.Presentation.Controllers;
using CookingGame.Infrastructure;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 订单管理器
    /// 管理订单的显示和提交
    /// </summary>
    public class OrderManager : MonoBehaviour
    {
        /// <summary>
        /// 订单管理用例
        /// </summary>
        private OrderManagementUseCase _orderUseCase;

        /// <summary>
        /// 烹饪工具管理用例
        /// </summary>
        private CookingToolManagementUseCase _toolUseCase;

        /// <summary>
        /// 订单容器
        /// </summary>
        [SerializeField] private Transform _ordersContainer;

        /// <summary>
        /// 订单预制体
        /// </summary>
        [SerializeField] private GameObject _orderPrefab;

        /// <summary>
        /// 订单数据
        /// </summary>
        private List<OrderData> _orders = new List<OrderData>();
        
        /// <summary>
        /// 已更新的订单ID集合
        /// </summary>
        private HashSet<string> _updatedOrderIds = new HashSet<string>();

        /// <summary>
        /// 订单数据结构
        /// </summary>
        private struct OrderData
        {
            public string OrderId;
            public string RecipeId;
            public string RecipeName;
            public int Reward;
            public OrderStatus Status;
            
            public bool IsDefault => string.IsNullOrEmpty(OrderId);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="orderUseCase">订单管理用例</param>
        /// <param name="toolUseCase">烹饪工具管理用例</param>
        public void Initialize(OrderManagementUseCase orderUseCase, CookingToolManagementUseCase toolUseCase)
        {
            if (orderUseCase == null)
            {
                Debug.LogError("OrderManagementUseCase is null in OrderManager.Initialize()");
                return;
            }
            
            if (toolUseCase == null)
            {
                Debug.LogError("CookingToolManagementUseCase is null in OrderManager.Initialize()");
                return;
            }

            _orderUseCase = orderUseCase;
            _toolUseCase = toolUseCase;

            // 初始化默认订单
            InitializeDefaultOrders();
            
            // 订阅订单状态变化事件
            GameEvents.SubscribeOrderStatusChanged(OnOrderStatusChanged);
        }
        
        /// <summary>
        /// 订单状态变化事件处理
        /// </summary>
        /// <param name="order">状态变化的订单</param>
        private void OnOrderStatusChanged(Order order)
        {
            // 找到对应的订单数据
            var orderData = _orders.Find(o => o.OrderId == order.Id);
            if (!orderData.IsDefault && orderData.Status != order.Status)
            {
                orderData.Status = order.Status;
                
                // 更新 UI
                var orderObj = FindOrderObject(order.Id);
                if (orderObj != null)
                {
                    UpdateOrderStatusVisuals(orderObj, orderData.Status);
                }
                
                // 更新列表中的订单数据
                int index = _orders.IndexOf(orderData);
                if (index >= 0)
                {
                    _orders[index] = orderData;
                }
                
                Debug.Log($"Order {order.Id} status changed to {order.Status}");
            }
        }

        /// <summary>
        /// 初始化默认订单
        /// </summary>
        private void InitializeDefaultOrders()
        {
            if (_orderUseCase == null)
            {
                Debug.LogError("OrderManagementUseCase is null in InitializeDefaultOrders()");
                return;
            }

            // 创建订单1: 切片胡萝卜
            CreateOrder("recipe_1", "Chopped Carrot", 100);
            
            // 创建订单2: 烹饪胡萝卜
            CreateOrder("recipe_2", "Cooked Carrot", 150);
            
            // 创建订单3: 细丝胡萝卜
            CreateOrder("recipe_3", "Julienne Carrot", 200);
            
            // 创建订单4: 胡萝卜沙拉
            CreateOrder("recipe_4", "Carrot Salad", 300);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="recipeId">食谱ID</param>
        /// <param name="recipeName">食谱名称</param>
        /// <param name="reward">奖励分数</param>
        private void CreateOrder(string recipeId, string recipeName, int reward)
        {
            Debug.Log($"CreateOrder: recipeId={recipeId}, recipeName={recipeName}, reward={reward}");
            
            if (_orderUseCase == null)
            {
                Debug.LogError("OrderManagementUseCase is null in CreateOrder()");
                return;
            }

            // 调用UseCase创建订单并保存到仓储
            var orderDto = _orderUseCase.CreateOrder($"order_{_orders.Count}", recipeId, reward);
            if (orderDto == null)
            {
                Debug.LogError($"Failed to create order for recipe {recipeId}");
                return;
            }

            Debug.Log($"Order created: {orderDto.Id}");

            // 创建订单数据
            var orderData = new OrderData
            {
                OrderId = orderDto.Id,
                RecipeId = recipeId,
                RecipeName = recipeName,
                Reward = reward,
                Status = orderDto.Status
            };

            _orders.Add(orderData);

            // 创建 UI 对象
            CreateOrderUI(orderData);
        }

        /// <summary>
        /// 创建订单 UI
        /// </summary>
        /// <param name="data">订单数据</param>
        private void CreateOrderUI(OrderData data)
        {
            if (_orderPrefab == null)
            {
                Debug.LogError("_orderPrefab is null in CreateOrderUI()");
                return;
            }
            
            if (_ordersContainer == null)
            {
                Debug.LogError("_ordersContainer is null in CreateOrderUI()");
                return;
            }

            // 实例化 UI 对象
            var orderObj = Instantiate(_orderPrefab, _ordersContainer);
            
            // 设置订单数据
            var text = orderObj.GetComponent<UnityEngine.UI.Text>();
            if (text != null)
            {
                text.text = $"{data.RecipeName}\n奖励: {data.Reward}";
            }

            // 获取 OrderSubmitController 组件
            var submitController = orderObj.GetComponent<OrderSubmitController>();
            if (submitController != null)
            {
                submitController.Initialize(ServiceLocator.Instance.OrderSubmissionUseCase);
                submitController.OrderId = data.OrderId;
            }

            // 设置订单状态颜色
            UpdateOrderStatusVisuals(orderObj, data.Status);
        }

        /// <summary>
        /// 更新订单状态视觉效果
        /// </summary>
        /// <param name="orderObj">订单对象</param>
        /// <param name="status">订单状态</param>
        private void UpdateOrderStatusVisuals(GameObject orderObj, OrderStatus status)
        {
            var image = orderObj.GetComponent<UnityEngine.UI.Image>();
            if (image == null)
            {
                Debug.LogWarning($"Order object {orderObj.name} does not have Image component");
                return;
            }

            switch (status)
            {
                case OrderStatus.Pending:
                    image.color = new Color(1f, 0.8f, 0f); // 黄色
                    break;
                case OrderStatus.Submitted:
                    image.color = new Color(0.5f, 0.5f, 1f); // 蓝色
                    break;
                case OrderStatus.Completed:
                    image.color = new Color(0.5f, 1f, 0.5f); // 绿色
                    break;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 此方法已废弃,改为事件驱动
            // 保留以兼容旧代码,但不再调用
        }
        
        /// <summary>
        /// 更新订单
        /// </summary>
        private void UpdateOrders()
        {
            // 此方法已废弃,改为事件驱动
            // 保留以兼容旧代码,但不再调用
        }

        /// <summary>
        /// 查找订单对象
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>订单 UI 对象</returns>
        private GameObject FindOrderObject(string orderId)
        {
            if (_ordersContainer == null)
            {
                Debug.LogError("_ordersContainer is null in FindOrderObject()");
                return null;
            }

            foreach (Transform child in _ordersContainer)
            {
                if (child.gameObject.name.Contains(orderId))
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="dishItemId">菜品物品ID</param>
        /// <returns>是否成功</returns>
        public bool SubmitOrder(string orderId, string dishItemId)
        {
            if (_orderUseCase == null)
            {
                Debug.LogError("OrderManagementUseCase is null in SubmitOrder()");
                return false;
            }

            var order = _orders.Find(o => o.OrderId == orderId);
            if (IsDefault(order)) return false;

            // 使用 OrderSubmissionUseCase 提交订单
            var submissionUseCase = ServiceLocator.Instance.OrderSubmissionUseCase;
            if (submissionUseCase == null)
            {
                Debug.LogError("OrderSubmissionUseCase is null in SubmitOrder()");
                return false;
            }

            var result = submissionUseCase.SubmitOrder(orderId, dishItemId);

            if (result != null)
            {
                order.Status = OrderStatus.Submitted;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// 获取订单容器
        /// </summary>
        /// <returns>订单容器</returns>
        public Transform OrdersContainer
        {
            get { return _ordersContainer; }
        }

        /// <summary>
        /// 检查数据是否为默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否为默认值</returns>
        private bool IsDefault(OrderData data)
        {
            return data.IsDefault;
        }

        /// <summary>
        /// 获取待处理订单数量
        /// </summary>
        /// <returns>待处理订单数量</returns>
        public int GetPendingOrderCount()
        {
            int count = 0;
            foreach (var order in _orders)
            {
                if (order.Status == OrderStatus.Pending)
                {
                    count++;
                }
            }
            return count;
        }
        
        /// <summary>
        /// 清理资源
        /// </summary>
        private void OnDestroy()
        {
            // 取消订阅事件
            GameEvents.UnsubscribeOrderStatusChanged(OnOrderStatusChanged);
        }
    }
}
