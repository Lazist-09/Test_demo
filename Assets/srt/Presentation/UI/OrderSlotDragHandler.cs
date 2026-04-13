using UnityEngine;
using UnityEngine.EventSystems;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 订单槽位拖拽处理器
    /// 处理订单槽位的拖拽逻辑
    /// </summary>
    public class OrderSlotDragHandler : MonoBehaviour, IDropHandler
    {
        /// <summary>
        /// 拖拽放置事件
        /// </summary>
        /// <param name="eventData">拖拽事件数据</param>
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"OrderSlotDragHandler.OnDrop: {eventData.pointerDrag?.name}");
        }
    }
}
