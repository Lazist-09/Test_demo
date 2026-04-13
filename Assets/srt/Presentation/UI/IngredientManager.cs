using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CookingGame.Application.UseCases;
using CookingGame.Core.Events;
using CookingGame.Core.Models;
using CookingGame.Core.Logging;
using CookingGame.Infrastructure;
using CookingGame.Presentation.Views;
using UnityEngine.EventSystems;

namespace CookingGame.Presentation.UI
{
    /// <summary>
    /// 食材管理器
    /// 管理原始食材的显示和交互
    /// </summary>
    public class IngredientManager : MonoBehaviour
    {
        /// <summary>
        /// 物品管理用例
        /// </summary>
        private ItemManagementUseCase _itemUseCase;

        /// <summary>
        /// 食谱管理用例
        /// </summary>
        private RecipeManagementUseCase _recipeUseCase;

        /// <summary>
        /// 日志器
        /// </summary>
        private CookingGame.Core.Logging.ILogger _logger;

        /// <summary>
        /// 食材容器
        /// </summary>
        [SerializeField] private Transform _ingredientsContainer;

        /// <summary>
        /// 食材预制体
        /// </summary>
        [SerializeField] private GameObject _ingredientPrefab;

        /// <summary>
        /// 食材数据
        /// </summary>
        private List<IngredientData> _ingredients = new List<IngredientData>();
        
        /// <summary>
        /// 已显示的菜品ID集合
        /// </summary>
        private HashSet<string> _displayedDishIds = new HashSet<string>();
        
        /// <summary>
        /// 已显示的Trash ID集合
        /// </summary>
        private HashSet<string> _displayedTrashIds = new HashSet<string>();

        /// <summary>
        /// 食材数据结构
        /// </summary>
        private class IngredientData
        {
            public string TemplateId;
            public string ItemId;
            public int Count;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="itemUseCase">物品管理用例</param>
        /// <param name="recipeUseCase">食谱管理用例</param>
        public void Initialize(ItemManagementUseCase itemUseCase, RecipeManagementUseCase recipeUseCase)
        {
            Debug.Log("IngredientManager.Initialize()");

            _itemUseCase = itemUseCase;
            _recipeUseCase = recipeUseCase;
            _logger = DependencyContainer.GetLogger();

            // 初始化默认食材
            InitializeDefaultIngredients();
            
            // 订阅菜品创建事件
            GameEvents.SubscribeDishCreated(OnDishCreated);
            
            // 订阅领域事件
            SubscribeDomainEvents();
        }
        
        /// <summary>
        /// 菜品创建事件处理
        /// </summary>
        /// <param name="item">创建的菜品</param>
        private void OnDishCreated(Item item)
        {
            if (item.Category == ItemType.FinishedDish && !_displayedDishIds.Contains(item.Id))
            {
                Debug.Log($"New dish detected: {item.Id}, Type: {item.Category}");
                var ingredientData = new IngredientData
                {
                    TemplateId = item.TemplateId,
                    ItemId = item.Id,
                    Count = 1
                };
                _ingredients.Add(ingredientData);
                _displayedDishIds.Add(item.Id);
                CreateIngredientUI(ingredientData);
            }
        }
        
        /// <summary>
        /// 订阅领域事件
        /// </summary>
        private void SubscribeDomainEvents()
        {
            DomainEvents.Subscribe<TrashCreatedDomainEvent>(OnTrashCreated);
            DomainEvents.Subscribe<TrashMergedDomainEvent>(OnTrashMerged);
        }
        
        /// <summary>
        /// Trash创建事件处理
        /// </summary>
        /// <param name="event">Trash创建领域事件</param>
        private void OnTrashCreated(TrashCreatedDomainEvent @event)
        {
            _logger.DomainEvent("TrashCreated", $"TrashId={@event.TrashItem.Id}, Count=1");
            
            if (!_displayedTrashIds.Contains(@event.TrashItem.Id))
            {
                Debug.Log($"New trash detected: {@event.TrashItem.Id}");
                var ingredientData = new IngredientData
                {
                    TemplateId = @event.TrashItem.TemplateId,
                    ItemId = @event.TrashItem.Id,
                    Count = 1
                };
                _ingredients.Add(ingredientData);
                _displayedTrashIds.Add(@event.TrashItem.Id);
                CreateIngredientUI(ingredientData);
            }
        }
        
        /// <summary>
        /// Trash合并事件处理
        /// </summary>
        /// <param name="event">Trash合并领域事件</param>
        private void OnTrashMerged(TrashMergedDomainEvent @event)
        {
            _logger.DomainEvent("TrashMerged", $"TrashId={@event.TrashId}, NewCount={@event.NewCount}");
            
            var trashData = _ingredients.Find(i => i.ItemId == @event.TrashId);
            if (trashData != null)
            {
                trashData.Count = @event.NewCount;
                UpdateIngredientUI();
                _logger.Info("Trash count updated: {TrashId}, NewCount: {Count}", 
                    trashData.ItemId, trashData.Count);
            }
        }

        /// <summary>
        /// 初始化默认食材
        /// </summary>
        private void InitializeDefaultIngredients()
        {
            Debug.Log("InitializeDefaultIngredients()");

            // 添加胡萝卜
            AddIngredient("carrot", 5);
            
            // 添加生菜
            AddIngredient("lettuce", 5);
            
            // 添加番茄
            AddIngredient("tomato", 5);
            
            Debug.Log($"Total ingredients created: {_ingredients.Count}");
        }

        /// <summary>
        /// 添加食材
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="count">数量</param>
        private void AddIngredient(string templateId, int count)
        {
            // 调用UseCase创建实际的物品
            var itemDto = _itemUseCase.CreateItem(templateId, ItemType.RawIngredient);
            if (itemDto == null)
            {
                Debug.LogError($"Failed to create item for template {templateId}");
                return;
            }

            // 创建食材数据
            var ingredientData = new IngredientData
            {
                TemplateId = templateId,
                ItemId = itemDto.Id,
                Count = count
            };

            _ingredients.Add(ingredientData);

            // 创建 UI 对象
            CreateIngredientUI(ingredientData);
        }

        /// <summary>
        /// 创建食材 UI
        /// </summary>
        /// <param name="data">食材数据</param>
        private void CreateIngredientUI(IngredientData data)
        {
            Debug.Log($"CreateIngredientUI: templateId={data.TemplateId}, itemId={data.ItemId}, count={data.Count}");
            
            if (_ingredientPrefab == null)
            {
                Debug.LogError("_ingredientPrefab is null!");
                return;
            }
            
            if (_ingredientsContainer == null)
            {
                Debug.LogError("_ingredientsContainer is null!");
                return;
            }

            // 实例化 UI 对象
            var ingredientObj = Instantiate(_ingredientPrefab, _ingredientsContainer);
            
            // 获取 ItemView 组件
            var itemView = ingredientObj.GetComponent<ItemView>();
            if (itemView != null)
            {
                itemView.Initialize(_itemUseCase, data.ItemId);
                Debug.Log($"ItemView initialized with itemId={data.ItemId}");
            }
            else
            {
                Debug.LogError("ItemView component not found on ingredient prefab!");
            }

            // 设置食材名称
            var text = ingredientObj.GetComponent<UnityEngine.UI.Text>();
            if (text != null)
            {
                text.text = $"{data.TemplateId} x {data.Count}";
            }
            
            // 添加事件触发器,使食材可以被拖拽
            AddDragHandler(ingredientObj);
        }

        /// <summary>
        /// 添加拖拽处理器
        /// </summary>
        /// <param name="obj"> GameObject</param>
        private void AddDragHandler(GameObject obj)
        {
            var eventTrigger = obj.AddComponent<EventTrigger>();
            
            var beginDrag = new EventTrigger.Entry
            {
                eventID = EventTriggerType.BeginDrag
            };
            beginDrag.callback.AddListener((eventData) => OnBeginDrag(obj));
            
            var drag = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Drag
            };
            drag.callback.AddListener((eventData) => OnDrag(obj));
            
            var endDrag = new EventTrigger.Entry
            {
                eventID = EventTriggerType.EndDrag
            };
            endDrag.callback.AddListener((eventData) => OnEndDrag(obj));
            
            eventTrigger.triggers.Add(beginDrag);
            eventTrigger.triggers.Add(drag);
            eventTrigger.triggers.Add(endDrag);
        }

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="obj">拖拽的对象</param>
        private void OnBeginDrag(GameObject obj)
        {
            Debug.Log($"OnBeginDrag: {obj.name}");
            var dragDropManager = FindObjectOfType<DragDropManager>();
            if (dragDropManager != null)
            {
                // 手动调用DragDropManager的OnBeginDrag
                var pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.pointerDrag = obj;
                dragDropManager.OnBeginDrag(pointerEventData);
            }
        }

        /// <summary>
        /// 拖拽中
        /// </summary>
        /// <param name="obj">拖拽的对象</param>
        private void OnDrag(GameObject obj)
        {
            var dragDropManager = FindObjectOfType<DragDropManager>();
            if (dragDropManager != null)
            {
                var pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;
                dragDropManager.OnDrag(pointerEventData);
            }
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="obj">拖拽的对象</param>
        private void OnEndDrag(GameObject obj)
        {
            var dragDropManager = FindObjectOfType<DragDropManager>();
            if (dragDropManager != null)
            {
                var pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;
                dragDropManager.OnEndDrag(pointerEventData);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 更新所有食材 UI
            UpdateIngredientUI();
        }

        /// <summary>
        /// 检查新的菜品输出
        /// </summary>
        private void CheckForNewDishes()
        {
            // 此方法已废弃,改为事件驱动
            // 保留以兼容旧代码,但不再调用
        }

        /// <summary>
        /// 更新食材 UI
        /// </summary>
        private void UpdateIngredientUI()
        {
            // 这里可以添加食材数量的动态更新逻辑
        }

        /// <summary>
        /// 获取食材数量
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <returns>食材数量</returns>
        public int GetIngredientCount(string templateId)
        {
            var ingredient = _ingredients.Find(i => i.TemplateId == templateId);
            return ingredient.Count > 0 ? ingredient.Count : 0;
        }

        /// <summary>
        /// 使用食材
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="count">使用数量</param>
        /// <returns>是否成功</returns>
        public bool UseIngredient(string templateId, int count)
        {
            var ingredient = _ingredients.Find(i => i.TemplateId == templateId);
            if (IsDefault(ingredient) || ingredient.Count < count) return false;

            ingredient.Count -= count;

            // 更新 UI
            UpdateIngredientUI();

            return true;
        }
        
        /// <summary>
        /// 获取食材容器
        /// </summary>
        /// <returns>食材容器</returns>
        public Transform IngredientsContainer
        {
            get { return _ingredientsContainer; }
        }

        /// <summary>
        /// 检查数据是否为默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否为默认值</returns>
        private bool IsDefault(IngredientData data)
        {
            return data.TemplateId == null;
        }
        
        /// <summary>
        /// 清理资源
        /// </summary>
        private void OnDestroy()
        {
            // 取消订阅事件
            GameEvents.UnsubscribeDishCreated(OnDishCreated);
            
            // 取消订阅领域事件
            DomainEvents.Unsubscribe<TrashCreatedDomainEvent>(OnTrashCreated);
            DomainEvents.Unsubscribe<TrashMergedDomainEvent>(OnTrashMerged);
        }
    }
}
