using UnityEngine;

namespace CookingGame.Presentation.Controllers
{
    /// <summary>
    /// 物品拖拽控制器
    /// 处理物品的拖拽交互逻辑
    /// </summary>
    public class ItemDragController : MonoBehaviour
    {
        /// <summary>
        /// 是否正在拖拽
        /// </summary>
        private bool _isDragging;
        
        /// <summary>
        /// 鼠标偏移量
        /// </summary>
        private Vector3 _offset;
        
        /// <summary>
        /// 主摄像机
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// 初始化
        /// 获取主摄像机引用
        /// </summary>
        private void Start()
        {
            _mainCamera = Camera.main;
        }

        /// <summary>
        /// 鼠标按下事件
        /// 开始拖拽物品
        /// </summary>
        private void OnMouseDown()
        {
            if (_mainCamera == null) return;
            _isDragging = true;
            var mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _offset = transform.position - mouseWorldPos;
        }

        /// <summary>
        /// 鼠标抬起事件
        /// 停止拖拽物品
        /// </summary>
        private void OnMouseUp()
        {
            _isDragging = false;
        }

        /// <summary>
        /// 更新
        /// 在拖拽过程中更新物品位置
        /// </summary>
        private void Update()
        {
            if (_isDragging && _mainCamera != null)
            {
                var mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mouseWorldPos + _offset;
            }
        }
    }
}
