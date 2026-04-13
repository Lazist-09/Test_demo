using UnityEngine;
using UnityEngine.EventSystems;
using CookingGame.Presentation.Controllers;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 食材槽处理器
    /// 处理食材槽的点击和拖拽交互
    /// </summary>
    public class IngredientSlotHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 食材ID
        /// </summary>
        [SerializeField] private string _ingredientId;

        /// <summary>
        /// 食材数量
        /// </summary>
        [SerializeField] private int _ingredientCount;

        /// <summary>
        /// 当前拖拽的对象
        /// </summary>
        private GameObject _draggedObject;

        /// <summary>
        /// 原始父对象
        /// </summary>
        private Transform _originalParent;

        /// <summary>
        /// 指针点击事件
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            // 点击食材槽，可以选择该食材用于匹配
            // 这里可以根据需要实现选择逻辑
            Debug.Log($"点击了食材槽: {_ingredientId}, 数量: {_ingredientCount}");
        }

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            _draggedObject = gameObject;

            if (_draggedObject == null) return;

            // 保存原始父对象
            _originalParent = transform.parent;

            // 将拖拽对象移动到Canvas层级
            transform.SetParent(transform.parent.parent.parent); // 移动到Canvas层级

            // 设置为最上层
            transform.SetAsLastSibling();
        }

        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnDrag(PointerEventData eventData)
        {
            if (_draggedObject != null)
            {
                // 更新拖拽对象的位置
                transform.position = eventData.position;
            }
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_draggedObject == null) return;

            // 检查是否拖拽到了工具区域
            CheckToolDrop(eventData.position);

            // 如果没有成功放置，返回原始位置
            if (transform.parent.name == "Canvas")
            {
                transform.SetParent(_originalParent);
            }

            _draggedObject = null;
        }

        /// <summary>
        /// 检查是否拖拽到了工具
        /// </summary>
        /// <param name="screenPosition">屏幕位置</param>
        private void CheckToolDrop(Vector3 screenPosition)
        {
            // 获取所有烹饪工具管理器
            var toolManagers = FindObjectsOfType<CookingToolManager>();

            foreach (var toolManager in toolManagers)
            {
                // 检查工具容器中的每个工具
                var toolsContainer = toolManager.ToolsContainer;
                if (toolsContainer == null) continue;

                foreach (Transform toolTransform in toolsContainer)
                {
                    // 获取工具的RectTransform
                    RectTransform toolRect = toolTransform.GetComponent<RectTransform>();

                    if (toolRect == null) continue;

                    // 检查鼠标位置是否在工具区域内
                    if (IsPointerOverObject(screenPosition, toolRect))
                    {
                        // 获取工具控制器
                        CookingToolController toolController = toolTransform.GetComponent<CookingToolController>();
                        if (toolController != null)
                        {
                            // 添加食材到工具
                            AddItemToTool(toolController.ToolId, _ingredientId);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查指针是否在对象区域内
        /// </summary>
        /// <param name="screenPosition">屏幕位置</param>
        /// <param name="rect">RectTransform</param>
        /// <returns>是否在区域内</returns>
        private bool IsPointerOverObject(Vector3 screenPosition, RectTransform rect)
        {
            return rect.rect.Contains(rect.InverseTransformPoint(screenPosition));
        }

        /// <summary>
        /// 添加食材到工具
        /// </summary>
        /// <param name="toolId">工具ID</param>
        /// <param name="itemId">食材ID</param>
        private void AddItemToTool(string toolId, string itemId)
        {
            var toolUseCase = CookingGame.Infrastructure.ServiceLocator.Instance.ToolUseCase;
            if (toolUseCase == null)
            {
                Debug.LogError("CookingToolManagementUseCase未初始化");
                return;
            }

            // 检查是否可以添加
            if (toolUseCase.CanAddItemToTool(toolId, itemId))
            {
                // 添加食材到工具
                toolUseCase.AddItemToTool(toolId, itemId);
                Debug.Log($"已添加食材 {itemId} 到工具 {toolId}");
            }
            else
            {
                Debug.LogWarning($"无法添加食材 {itemId} 到工具 {toolId}，工具可能已满或正在运行");
            }
        }

        /// <summary>
        /// 设置食材ID
        /// </summary>
        /// <param name="id">食材ID</param>
        public void SetIngredientId(string id)
        {
            _ingredientId = id;
        }

        /// <summary>
        /// 设置食材数量
        /// </summary>
        /// <param name="count">食材数量</param>
        public void SetIngredientCount(int count)
        {
            _ingredientCount = count;
        }

        /// <summary>
        /// 获取食材ID
        /// </summary>
        /// <returns>食材ID</returns>
        public string GetIngredientId()
        {
            return _ingredientId;
        }

        /// <summary>
        /// 获取食材数量
        /// </summary>
        /// <returns>食材数量</returns>
        public int GetIngredientCount()
        {
            return _ingredientCount;
        }
    }
}
