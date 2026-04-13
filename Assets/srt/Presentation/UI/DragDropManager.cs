using UnityEngine;
using UnityEngine.EventSystems;
using CookingGame.Application.UseCases;
using CookingGame.Infrastructure;
using CookingGame.Presentation.Views;
using CookingGame.Presentation.Controllers;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 拖拽管理器
    /// 处理食材从食材面板拖拽到工具的逻辑
    /// </summary>
    public class DragDropManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 当前拖拽的对象
        /// </summary>
        private GameObject _draggedObject;

        /// <summary>
        /// 原始父对象
        /// </summary>
        private Transform _originalParent;

        /// <summary>
        /// 物品管理用例
        /// </summary>
        private ItemManagementUseCase _itemUseCase;

        /// <summary>
        /// 烹饪工具管理用例
        /// </summary>
        private CookingToolManagementUseCase _toolUseCase;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            // 获取用例
            _itemUseCase = ServiceLocator.Instance.ItemUseCase;
            _toolUseCase = ServiceLocator.Instance.ToolUseCase;
        }

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"DragDropManager.OnBeginDrag: {eventData.pointerDrag?.name}");
            _draggedObject = eventData.pointerDrag;

            if (_draggedObject == null) return;

            Debug.Log($"Dragging object: {_draggedObject.name}");

            // 保存原始父对象
            _originalParent = _draggedObject.transform.parent;

            // 将拖拽对象移动到Canvas层级，以便自由移动
            _draggedObject.transform.SetParent(transform);

            // 设置为最上层
            _draggedObject.transform.SetAsLastSibling();
            
            Debug.Log($"New parent: {_draggedObject.transform.parent.name}");
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
                _draggedObject.transform.position = eventData.position;
            }
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"DragDropManager.OnEndDrag: {_draggedObject?.name}");
            
            if (_draggedObject == null) return;

            Debug.Log($"Checking tool drop at position: {eventData.position}");

            // 检查是否拖拽到了工具区域
            CheckToolDrop(eventData.position);

            // 如果没有成功放置，返回原始位置
            if (_draggedObject.transform.parent == transform)
            {
                Debug.Log($"Returning to original parent: {_originalParent?.name}");
                _draggedObject.transform.SetParent(_originalParent);
            }
            else
            {
                Debug.Log($"Still under: {_draggedObject.transform.parent.name}");
            }

            _draggedObject = null;
        }

        /// <summary>
        /// 检查是否拖拽到了工具
        /// </summary>
        /// <param name="screenPosition">屏幕位置</param>
        private void CheckToolDrop(Vector3 screenPosition)
        {
            Debug.Log($"CheckToolDrop at {screenPosition}");
            
            // 获取所有烹饪工具管理器
            var toolManagers = FindObjectsOfType<CookingToolManager>();
            Debug.Log($"Found {toolManagers.Length} tool managers");

            foreach (var toolManager in toolManagers)
            {
                // 检查工具容器中的每个工具
                var toolsContainer = toolManager.ToolsContainer;
                if (toolsContainer == null)
                {
                    Debug.LogWarning("ToolsContainer is null");
                    continue;
                }

                Debug.Log($"Checking tools container: {toolsContainer.name}, child count: {toolsContainer.childCount}");

                foreach (Transform toolTransform in toolsContainer)
                {
                    // 获取工具的RectTransform
                    RectTransform toolRect = toolTransform.GetComponent<RectTransform>();

                    if (toolRect == null)
                    {
                        Debug.LogWarning($"Tool {toolTransform.name} has no RectTransform");
                        continue;
                    }

                    // 检查鼠标位置是否在工具区域内
                    if (IsPointerOverObject(screenPosition, toolRect))
                    {
                        Debug.Log($"Pointer is over tool: {toolTransform.name}");
                        
                        // 检查是否有ItemView组件
                        ItemView itemView = _draggedObject.GetComponent<ItemView>();
                        if (itemView != null && !string.IsNullOrEmpty(itemView.ItemId))
                        {
                            Debug.Log($"ItemView found: {itemView.ItemId}");
                            
                            // 获取工具控制器
                            CookingToolController toolController = toolTransform.GetComponent<CookingToolController>();
                            if (toolController != null)
                            {
                                Debug.Log($"Adding item {itemView.ItemId} to tool {toolController.ToolId}");
                                
                                // 添加食材到工具
                                AddItemToTool(toolController.ToolId, itemView.ItemId);
                                break;
                            }
                            else
                            {
                                Debug.LogWarning($"Tool {toolTransform.name} has no CookingToolController");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("No ItemView or ItemId on dragged object");
                        }
                    }
                }
            }
            
            // 检查是否拖拽到了菜品槽位
            CheckDishSlotDrop(screenPosition);
        }

        /// <summary>
        /// 检查是否拖拽到了菜品槽位
        /// </summary>
        /// <param name="screenPosition">屏幕位置</param>
        private void CheckDishSlotDrop(Vector3 screenPosition)
        {
            Debug.Log($"CheckDishSlotDrop at {screenPosition}");
            
            // 获取所有菜品槽位
            var dishSlots = FindObjectsOfType<DishSlot>();
            Debug.Log($"Found {dishSlots.Length} dish slots");

            foreach (var dishSlot in dishSlots)
            {
                // 获取槽位的RectTransform
                RectTransform slotRect = dishSlot.GetComponent<RectTransform>();

                if (slotRect == null)
                {
                    Debug.LogWarning($"DishSlot {dishSlot.name} has no RectTransform");
                    continue;
                }

                // 检查鼠标位置是否在槽位区域内
                if (IsPointerOverObject(screenPosition, slotRect))
                {
                    Debug.Log($"Pointer is over dish slot: {dishSlot.name}");
                    
                    // 检查是否有ItemView组件
                    ItemView itemView = _draggedObject.GetComponent<ItemView>();
                    if (itemView != null && !string.IsNullOrEmpty(itemView.ItemId))
                    {
                        Debug.Log($"ItemView found: {itemView.ItemId}");
                        
                        // 将菜品放入槽位
                        dishSlot.OnDrop(new PointerEventData(UnityEngine.EventSystems.EventSystem.current)
                        {
                            pointerDrag = _draggedObject
                        });
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("No ItemView or ItemId on dragged object");
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
            Debug.Log($"AddItemToTool: toolId={toolId}, itemId={itemId}");
            
            if (_toolUseCase == null)
            {
                Debug.LogError("CookingToolManagementUseCase未初始化");
                return;
            }

            // 检查是否可以添加
            if (_toolUseCase.CanAddItemToTool(toolId, itemId))
            {
                // 添加食材到工具
                _toolUseCase.AddItemToTool(toolId, itemId);
                Debug.Log($"已添加食材 {itemId} 到工具 {toolId}");
            }
            else
            {
                Debug.LogWarning($"无法添加食材 {itemId} 到工具 {toolId}，工具可能已满或正在运行");
            }
        }

        /// <summary>
        /// 获取拖拽的对象
        /// </summary>
        /// <returns>拖拽的对象</returns>
        public GameObject GetDraggedObject()
        {
            return _draggedObject;
        }

        /// <summary>
        /// 检查是否有对象正在被拖拽
        /// </summary>
        /// <returns>是否有对象正在被拖拽</returns>
        public bool IsDragging()
        {
            return _draggedObject != null;
        }
    }
}
