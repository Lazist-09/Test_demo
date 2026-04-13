using UnityEngine;
using CookingGame.Application.UseCases;
using CookingGame.Core.Models;
using CookingGame.Application.DTOs;
using CookingGame.Infrastructure;

namespace CookingGame.Presentation.Views
{
    /// <summary>
    /// 物品视图
    /// 负责显示物品的视觉效果
    /// 根据物品的形状和熟度更新外观
    /// </summary>
    public class ItemView : MonoBehaviour
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        [SerializeField] private string _itemId;
        
        /// <summary>
        /// 物品管理用例
        /// </summary>
        private ItemManagementUseCase _useCase;
        
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
        /// <param name="useCase">物品管理用例</param>
        /// <param name="itemId">物品ID</param>
        public void Initialize(ItemManagementUseCase useCase, string itemId)
        {
            Debug.Log($"ItemView.Initialize(useCase, itemId): {itemId}");
            _useCase = useCase;
            _itemId = itemId;
            InitializeComponents();
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="itemDto">物品DTO</param>
        public void Initialize(ItemDto itemDto)
        {
            _itemId = itemDto.Id;
            _useCase = ServiceLocator.Instance.ItemUseCase;
            InitializeComponents();
            UpdateVisuals();
        }
        
        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponents()
        {
            Debug.Log($"ItemView.InitializeComponents() for itemId={_itemId}");
            
            // 如果没有在 Inspector 中设置,尝试自动查找
            if (_backgroundImage == null)
            {
                // 查找名为 "Background" 的子对象的 Image 组件
                Transform backgroundTransform = transform.Find("Background");
                if (backgroundTransform != null)
                {
                    _backgroundImage = backgroundTransform.GetComponent<UnityEngine.UI.Image>();
                    Debug.Log("Background Image found");
                }
                else
                {
                    Debug.LogWarning("Background Image not found");
                }
            }
            
            if (_nameText == null)
            {
                // 查找名为 "Label" 的子对象的 Text 组件
                Transform labelTransform = transform.Find("Label");
                if (labelTransform != null)
                {
                    _nameText = labelTransform.GetComponent<UnityEngine.UI.Text>();
                    Debug.Log("Label Text found");
                }
                
                // 如果找不到,尝试查找任意 Text 组件
                if (_nameText == null)
                {
                    _nameText = GetComponentInChildren<UnityEngine.UI.Text>();
                }
            }
        }

        /// <summary>
        /// 更新视觉效果
        /// 根据物品数据更新外观
        /// </summary>
        public void UpdateVisuals()
        {
            Debug.Log($"ItemView.UpdateVisuals() for itemId={_itemId}");
            
            if (_useCase != null)
            {
                var itemDto = _useCase.GetItem(_itemId);
                if (itemDto != null)
                {
                    Debug.Log($"Item found: {itemDto.Name}, shape={itemDto.Shape}, stage={itemDto.CookingStage}");
                    UpdateShapeVisuals(itemDto.Shape);
                    UpdateStageVisuals(itemDto.CookingStage);
                    UpdateNameVisuals(itemDto.Name);
                }
                else
                {
                    Debug.LogError($"Item { _itemId} not found!");
                }
            }
        }
        
        /// <summary>
        /// 更新名称视觉效果
        /// </summary>
        /// <param name="name">物品名称</param>
        private void UpdateNameVisuals(string name)
        {
            if (_nameText != null)
            {
                _nameText.text = name;
            }
        }

        /// <summary>
        /// 更新形状视觉效果
        /// 根据物品形状更新颜色或透明度
        /// </summary>
        /// <param name="shape">物品形状</param>
        private void UpdateShapeVisuals(Shape shape)
        {
            if (_backgroundImage == null) return;
            
            switch (shape)
            {
                case Shape.Whole:
                    _backgroundImage.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case Shape.Chunk:
                    _backgroundImage.color = new Color(0.9f, 0.9f, 0.9f, 1f);
                    break;
                case Shape.Slice:
                    _backgroundImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                    break;
                case Shape.Julienne:
                    _backgroundImage.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                    break;
                case Shape.Crumbled:
                    _backgroundImage.color = new Color(0.6f, 0.6f, 0.6f, 1f);
                    break;
            }
        }

        /// <summary>
        /// 更新熟度视觉效果
        /// 根据熟度更新颜色或材质
        /// 生 = 白色, 半熟 = 浅棕色, 全熟 = 深棕色, 烧焦 = 黑色
        /// </summary>
        /// <param name="stage">熟度</param>
        private void UpdateStageVisuals(CookingStage stage)
        {
            if (_backgroundImage == null) return;
            
            switch (stage)
            {
                case CookingStage.Raw:
                    _backgroundImage.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case CookingStage.Medium:
                    _backgroundImage.color = new Color(0.8f, 0.6f, 0.4f, 1f);
                    break;
                case CookingStage.WellDone:
                    _backgroundImage.color = new Color(0.6f, 0.4f, 0.2f, 1f);
                    break;
                case CookingStage.Burnt:
                    _backgroundImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
                    break;
            }
        }

        /// <summary>
        /// 获取物品ID
        /// </summary>
        /// <returns>物品ID</returns>
        public string ItemId
        {
            get { return _itemId; }
        }
    }
}
