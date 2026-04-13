using UnityEngine;
using UnityEngine.EventSystems;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;
using CookingGame.Presentation.Views;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 菜品槽位
    /// 可拖拽的菜品容器,支持拖入/拖出菜品
    /// </summary>
    public class DishSlot : MonoBehaviour, IDropHandler, IDragHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 槽位图像组件
        /// </summary>
        [SerializeField] private UnityEngine.UI.Image _slotImage;

        /// <summary>
        /// 菜品对象
        /// </summary>
        private GameObject _dishObject;

        /// <summary>
        /// 菜品ID
        /// </summary>
        private string _dishId;

        /// <summary>
        /// 原始父级
        /// </summary>
        private Transform _originalParent;

        /// <summary>
        /// 物品管理用例
        /// </summary>
        private ItemManagementUseCase _itemUseCase;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="itemUseCase">物品管理用例</param>
        public void Initialize(ItemManagementUseCase itemUseCase)
        {
            _itemUseCase = itemUseCase;
        }

        /// <summary>
        /// 拖拽放置事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnDrop: {eventData.pointerDrag?.name}");

            // 获取拖拽的菜品对象
            var draggedObject = eventData.pointerDrag;
            if (draggedObject == null) return;

            // 保存原始父级
            _originalParent = draggedObject.transform.parent;

            // 检查是否是菜品
            var itemView = draggedObject.GetComponent<ItemView>();
            if (itemView == null)
            {
                Debug.LogWarning("Dragged object is not a dish!");
                return;
            }

            // 如果槽位已有菜品,先移除
            if (_dishObject != null)
            {
                RemoveDish();
            }

            // 将菜品移动到槽位
            draggedObject.transform.SetParent(transform);
            draggedObject.transform.localPosition = Vector3.zero;
            _dishObject = draggedObject;
            _dishId = itemView.ItemId;

            Debug.Log($"Dish placed in slot: {_dishId}");
        }

        /// <summary>
        /// 拖拽进入事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnDragEnter(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnDragEnter: {eventData.pointerDrag?.name}");
        }

        /// <summary>
        /// 拖拽离开事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnDragExit(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnDragExit: {eventData.pointerDrag?.name}");
        }

        /// <summary>
        /// 获取槽位中的菜品ID
        /// </summary>
        /// <returns>菜品ID,如果没有则返回null</returns>
        public string GetDishId()
        {
            return _dishId;
        }

        /// <summary>
        /// 移除槽位中的菜品
        /// </summary>
        public void RemoveDish()
        {
            if (_dishObject != null)
            {
                Debug.Log($"Removing dish from slot: {_dishId}");
                _dishObject.transform.SetParent(_originalParent);
                _dishObject = null;
                _dishId = null;
            }
        }

        /// <summary>
        /// 拖拽初始化事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnInitializePotentialDrag: {_dishId}");
            eventData.useDragThreshold = false;
        }

        /// <summary>
        /// 拖拽事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnDrag: {_dishId}");
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="eventData">点击事件数据</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnPointerClick: {_dishId}");
            
            // 如果槽位中有菜品,点击可以将其拖出
            if (_dishObject != null)
            {
                Debug.Log($"Removing dish from slot: {_dishId}");
                
                // 确保有原始父对象
                if (_originalParent != null)
                {
                    _dishObject.transform.SetParent(_originalParent);
                }
                else
                {
                    // 如果没有原始父对象，返回到原料区
                    var ingredientManager = FindObjectOfType<IngredientManager>();
                    if (ingredientManager != null)
                    {
                        _dishObject.transform.SetParent(ingredientManager.IngredientsContainer);
                    }
                }
                
                _dishObject.transform.position = Vector3.zero;
                _dishObject = null;
                _dishId = null;
            }
        }

        /// <summary>
        /// 拖拽开始事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnBeginDrag: {_dishId}");
            
            // 如果槽位中有菜品,设置拖拽对象
            if (_dishObject != null)
            {
                Debug.Log($"Setting dragged object: {_dishId}");
                eventData.useDragThreshold = false;
            }
        }

        /// <summary>
        /// 拖拽结束事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"DishSlot.OnEndDrag: {_dishId}");
        }

        /// <summary>
        /// 清空槽位
        /// </summary>
        public void Clear()
        {
            RemoveDish();
        }
    }
}
