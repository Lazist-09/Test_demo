# UI系统脚本挂载指南

## 📋 脚本列表

以下脚本已经创建，需要手动挂载到Unity场景中：

### 1. IngredientButtonManager.cs
**功能**: 控制食材面板的展开和收起

**挂载位置**: KitchenPanel (或 Canvas)

**需要配置的字段**:
- `_ingredientPanel`: 食材面板对象
- `_ingredientButton`: 食材按钮对象

**使用方法**:
1. 将脚本挂载到 KitchenPanel
2. 在 Inspector 中分配 `_ingredientPanel` 和 `_ingredientButton`
3. 在食材按钮上添加 Button 组件
4. Button 的 OnClick() 事件绑定到 IngredientButtonManager.TogglePanel()

---

### 2. MatchPanelController.cs
**功能**: 处理菜品和订单的匹配逻辑

**挂载位置**: MatchPanel (中间匹配区域)

**需要配置的字段**:
- `_dishSlotImage`: 菜品槽的 Image 组件
- `_orderSlotImage`: 订单槽的 Image 组件

**使用方法**:
1. 将脚本挂载到 MatchPanel
2. 在 Inspector 中分配 `_dishSlotImage` 和 `_orderSlotImage`
3. 在菜品槽上添加点击事件，调用 SelectDish(dishItemId)
4. 在订单槽上添加点击事件，调用 SelectOrder(orderId)
5. 在提交按钮上添加点击事件，调用 SubmitMatchedOrder()

**可调用的方法**:
- `SelectDish(string dishItemId)` - 选择菜品
- `SelectOrder(string orderId)` - 选择订单
- `SubmitMatchedOrder()` - 提交匹配的订单
- `ClearDishSelection()` - 清空菜品选择
- `ClearOrderSelection()` - 清空订单选择

---

### 3. DragDropManager.cs
**功能**: 处理食材从食材面板拖拽到工具的逻辑

**挂载位置**: Canvas (根对象)

**需要配置的字段**: 无

**使用方法**:
1. 将脚本挂载到 Canvas
2. 确保 Canvas 上有 EventSystem (如果没有，Unity会自动创建)
3. 在食材预制体上添加 Image 组件
4. 在食材预制体上添加 DragDropManager 脚本

**注意**: 这个脚本实现了 IBeginDragHandler, IDragHandler, IEndDragHandler 接口

---

### 4. IngredientSlotHandler.cs
**功能**: 处理食材槽的点击和拖拽交互

**挂载位置**: 食材预制体 (IngredientPrefab)

**需要配置的字段**:
- `_ingredientId`: 食材ID (可选，可以在代码中设置)
- `_ingredientCount`: 食材数量 (可选，可以在代码中设置)

**使用方法**:
1. 将脚本挂载到食材预制体
2. 在 Inspector 中设置 `_ingredientId` 和 `_ingredientCount`
3. 食材预制体需要有 Image 组件用于拖拽

**可调用的方法**:
- `SetIngredientId(string id)` - 设置食材ID
- `SetIngredientCount(int count)` - 设置食材数量
- `GetIngredientId()` - 获取食材ID
- `GetIngredientCount()` - 获取食材数量

---

### 5. ToolSlotHandler.cs
**功能**: 处理工具槽的点击交互

**挂载位置**: 工具预制体 (ToolPrefab)

**需要配置的字段**:
- `_toolId`: 工具ID (可选，可以在代码中设置)

**使用方法**:
1. 将脚本挂载到工具预制体
2. 在 Inspector 中设置 `_toolId`
3. 左键点击启动/停止工具
4. 右键点击清空工具

**可调用的方法**:
- `SetToolId(string id)` - 设置工具ID
- `GetToolId()` - 获取工具ID

---

## 🎯 Unity场景结构建议

```
Canvas (挂载 DragDropManager)
├── KitchenPanel (挂载 IngredientButtonManager)
│   ├── OrderPanel
│   │   └── OrdersContainer (OrderManager)
│   │
│   ├── MatchPanel (挂载 MatchPanelController)
│   │   ├── DishSlot (Image)
│   │   ├── Arrow
│   │   └── OrderSlot (Image)
│   │
│   ├── TablePanel
│   │   └── ToolsContainer (CookingToolManager)
│   │
│   └── IngredientPanel (默认隐藏)
│       └── IngredientsContainer (IngredientManager)
│
└── IngredientButton (挂载 Button 组件)
```

---

## 🔧 食材预制体结构

```
IngredientPrefab
├── Image (背景，用于拖拽)
├── Text (显示名称和数量)
└── IngredientSlotHandler (脚本)
```

**要求**:
- Image 组件必须有
- 需要设置 `_ingredientId` 和 `_ingredientCount`

---

## 🔧 工具预制体结构

```
ToolPrefab
├── Background (Image)
│   └── Icon (Image)
├── InputSlot (Image)
│   └── SlotContent (Image)
├── ProgressOverlay (Canvas)
│   └── ProgressSlider (Slider)
├── Label (Text)
├── CookingToolView (脚本)
├── CookingToolController (脚本)
└── ToolSlotHandler (脚本)
```

**要求**:
- 需要设置 `_toolId`
- CookingToolController 需要初始化

---

## 📝 使用流程

### 食材拖拽流程:
1. 点击食材按钮唤出食材面板
2. 从面板中拖拽食材到工具区域
3. 拖拽管理器自动检测并添加食材到工具

### 订单匹配流程:
1. 点击菜品槽选择已完成的菜品
2. 点击订单槽选择待处理的订单
3. 点击提交按钮匹配并提交订单

### 工具操作流程:
1. 左键点击工具启动/停止
2. 右键点击工具清空内容
3. 工具自动显示进度条

---

## ⚠️ 注意事项

1. **DragDropManager** 必须挂载在 Canvas 上，因为需要处理全局拖拽事件
2. **MatchPanelController** 需要在 Start() 中自动获取 OrderManager
3. **ToolSlotHandler** 需要正确设置 `_toolId` 才能工作
4. 所有脚本都使用 ServiceLocator 获取用例，确保 ServiceLocator 已初始化

---

## 🐛 常见问题

**Q: 拖拽不工作？**
A: 检查 Canvas 上是否有 EventSystem，检查食材是否有 Image 组件

**Q: 订单提交失败？**
A: 检查 MatchPanelController 是否正确获取了 OrderManager

**Q: 工具无法启动？**
A: 检查 ToolSlotHandler 的 `_toolId` 是否正确设置

---

## 📚 相关文件

- IngredientButtonManager.cs
- MatchPanelController.cs
- DragDropManager.cs
- IngredientSlotHandler.cs
- ToolSlotHandler.cs
- CookingToolManagementUseCase.cs (已添加 ClearToolInput 方法)
- CookingToolManager.cs (已添加 ToolsContainer 属性)
- OrderManager.cs (已添加 OrdersContainer 属性)
- IngredientManager.cs (已添加 IngredientsContainer 属性)
- ItemView.cs (已添加 ItemId 属性)
