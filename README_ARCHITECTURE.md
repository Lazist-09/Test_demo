# 烹饪游戏 - 洋葱架构实现

## 项目概述

这是一个基于Unity的2D烹饪模拟游戏，采用**洋葱架构**（Onion Architecture）设计模式实现。

## 架构分层

### 1. Domain Layer (核心层)
- **位置**: `Assets/srt/Core/`
- **职责**: 包含业务核心逻辑和实体模型
- **内容**:
  - 实体模型 (Item, Recipe, Order, CookingTool等)
  - 枚举定义 (Shape, CookingStage, ItemType等)
  - 服务接口 (IRecipeMatcher, IItemValidator等)
  - 仓储接口 (IItemRepository, IRecipeRepository等)
  - 状态机 (CookingToolStateMachine)
  - 配置 (GameConfig)

### 2. Application Layer (应用层)
- **位置**: `Assets/srt/Application/`
- **职责**: 协调领域对象，实现业务用例
- **内容**:
  - DTOs (数据传输对象)
  - UseCases (业务用例)
  - ServiceLocator (服务定位器)

### 3. Infrastructure Layer (基础设施层)
- **位置**: `Assets/srt/Infrastructure/`
- **职责**: 实现具体的技术细节
- **内容**:
  - 仓储实现 (InMemoryItemRepository等)
  - 数据库初始化 (DatabaseInitializer)

### 4. Presentation Layer (表现层)
- **位置**: `Assets/srt/Presentation/`
- **职责**: Unity交互逻辑
- **内容**:
  - Controllers (控制器)
  - Views (视图)
  - GameBootstrap (游戏启动器)

## 核心概念

### 物品系统 (Item System)

#### 物品类型
- **RawIngredient**: 原始食材
- **ProcessedIngredient**: 加工食材
- **FinishedDish**: 完成菜品
- **Trash**: 垃圾

#### 处理状态
- **Shape**: 形状 (Whole → Chunk → Slice → Julienne → Crumbled)
- **CookingStage**: 熟度 (Raw → Medium → WellDone → Burnt)
- **Status**: 状态 (Inactive, Processing, Ready, Discarded)

### 烹饪工具 (Cooking Tool)

#### 状态机
```
Idle (空闲) → Preparing (准备) → Cooking (烹饪) → Finished (完成)
                              ↘ Paused (暂停) ↗
```

#### 操作规则
- **未启动**: 可以添加/移除食材
- **启动中**: 无法添加/移除食材
- **长按**: 推进处理进度
- **松开**: 暂停烹饪
- **超时**: 自动丢弃

### 配方匹配 (Recipe Matching)

#### 动态匹配算法
1. 遍历所有配方
2. 无序精准匹配
3. 验证所有成分的形状和熟度
4. 计算得分 (0-100分)
5. 返回匹配的配方或垃圾

#### 配方映射
- 采用**一对一**映射避免歧义
- 每个配方有唯一的ID
- 菜品根据熟度和形状记录阶段

### 订单系统 (Order System)

#### 订单状态
- **Pending**: 待处理
- **Submitted**: 已提交
- **Completed**: 已完成

#### 提交流程
1. 将菜品放入出餐口
2. 选择订单
3. 按铃提交
4. 验证菜品匹配
5. 计算奖励分数
6. 更新订单状态

## 使用示例

### 初始化游戏
```csharp
DependencyContainer.Initialize();
ServiceLocator.Instance.ItemUseCase.CreateItem("carrot", ItemType.RawIngredient);
```

### 烹饪流程
```csharp
var toolId = ServiceLocator.Instance.ToolUseCase.CreateTool(CookingToolType.Pan).Id;
ServiceLocator.Instance.ToolUseCase.AddItemToTool(toolId, itemId);
ServiceLocator.Instance.ToolUseCase.StartCooking(toolId);
ServiceLocator.Instance.ToolUseCase.AddToolProgress(toolId, 0.1f);
```

### 订单提交
```csharp
var isValid = ServiceLocator.Instance.OrderSubmissionUseCase.ValidateDishForOrder(orderId, dishItemId);
var reward = ServiceLocator.Instance.OrderSubmissionUseCase.CalculateOrderReward(orderId, dishItemId);
ServiceLocator.Instance.OrderSubmissionUseCase.SubmitOrder(orderId, dishItemId);
```

## 扩展性设计

### 添加新食材
1. 在 `GameConfig` 中添加配置
2. 在 `DatabaseInitializer` 中初始化数据
3. 在 `ItemValidator` 中添加验证规则

### 添加新配方
1. 在 `DatabaseInitializer` 中创建配方
2. 确保配方ID唯一
3. 验证成分的形状和熟度要求

### 添加新工具类型
1. 在 `CookingToolType` 枚举中添加
2. 在 `CookingTool` 中实现容量逻辑
3. 在 `ItemValidator` 中添加验证规则

## 代码规范

### 命名约定
- **接口**: `I` 前缀 (IRepository, IService)
- **类**: PascalCase (ItemRepository, RecipeMatcher)
- **方法**: PascalCase (CreateItem, GetRecipe)
- **字段**: `_` 前缀 (_itemRepository)
- **常量**: ALL_CAPS (MAX_TOOL_CAPACITY)

### 依赖注入
- 使用构造函数注入
- 依赖接口而非实现
- 使用ServiceLocator进行全局访问

### 错误处理
- 返回null表示未找到
- 使用Debug.Log记录错误
- 验证输入参数

## 下一步开发

1. **UI系统**: 实现Unity UI界面
2. **动画系统**: 添加烹饪动画
3. **音效系统**: 添加音效反馈
4. **存档系统**: 实现数据持久化
5. **测试系统**: 添加单元测试

## 许可证

MIT License
