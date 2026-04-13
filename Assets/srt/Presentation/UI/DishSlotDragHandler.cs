using UnityEngine;
using UnityEngine.EventSystems;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 菜品槽位拖拽处理器
    /// 处理槽位内部的拖拽逻辑
    /// </summary>
    public class DishSlotDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        /// <summary>
        /// 拖拽开始位置
        /// </summary>
        private Vector2 _dragStartPosition;

        /// <summary>
        /// 拖拽开始时的指针位置
        /// </summary>
        private Vector2 _pointerDownPosition;

        /// <summary>
        /// 指针按下事件
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDownPosition = eventData.position;
            Debug.Log($"DishSlotDragHandler.OnPointerDown: {gameObject.name}");
        }

        /// <summary>
        /// 拖拽事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"DishSlotDragHandler.OnDrag: {gameObject.name}");
        }

        /// <summary>
        /// 指针释放事件
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log($"DishSlotDragHandler.OnPointerUp: {gameObject.name}");
            
            // 检查是否是点击(拖拽距离很小)
            float dragDistance = Vector2.Distance(_pointerDownPosition, eventData.position);
            if (dragDistance < 5f)
            {
                Debug.Log($"Click detected on {gameObject.name}");
            }
        }
    }
}
