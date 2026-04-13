using System.Collections.Generic;

namespace CookingGame.Core.Models
{
    /// <summary>
    /// 食谱类
    /// 定义一道菜品的制作配方，包含所需的食材和要求
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// 食谱唯一标识符
        /// </summary>
        public string Id { get; internal set; }
        
        /// <summary>
        /// 食谱名称
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// 所需食材列表
        /// </summary>
        public List<RecipeIngredient> Ingredients { get; private set; }
        
        /// <summary>
        /// 最低得分要求
        /// 用于确定菜品的最低质量要求
        /// </summary>
        public int MinScore { get; private set; }
        
        /// <summary>
        /// 最高得分
        /// 表示完美菜品的得分
        /// </summary>
        public int MaxScore { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">食谱ID</param>
        /// <param name="name">食谱名称</param>
        /// <param name="reward">奖励分数</param>
        public Recipe(string id, string name, int reward)
        {
            Id = id;
            Name = name;
            Ingredients = new List<RecipeIngredient>();
            MinScore = 60;
            MaxScore = 100;
        }

        /// <summary>
        /// 完整构造函数
        /// </summary>
        /// <param name="id">食谱ID</param>
        /// <param name="name">食谱名称</param>
        /// <param name="ingredients">所需食材列表</param>
        /// <param name="minScore">最低得分要求</param>
        /// <param name="maxScore">最高得分</param>
        public Recipe(string id, string name, List<RecipeIngredient> ingredients, int minScore, int maxScore)
        {
            Id = id;
            Name = name;
            Ingredients = ingredients ?? new List<RecipeIngredient>();
            MinScore = minScore;
            MaxScore = maxScore;
        }

        /// <summary>
        /// 添加所需食材
        /// </summary>
        /// <param name="templateId">食材模板ID</param>
        public void AddRequiredItem(string templateId)
        {
            Ingredients.Add(new RecipeIngredient(templateId, Shape.Slice, CookingStage.Medium));
        }

        /// <summary>
        /// 更新食谱
        /// </summary>
        /// <param name="recipe">要更新的食谱</param>
        public void Update(Recipe recipe)
        {
            if (recipe == null) return;
            
            Name = recipe.Name;
            Ingredients = new List<RecipeIngredient>(recipe.Ingredients);
            MinScore = recipe.MinScore;
            MaxScore = recipe.MaxScore;
        }
    }

    /// <summary>
    /// 食谱成分类
    /// 定义食谱中每个成分的具体要求
    /// </summary>
    public class RecipeIngredient
    {
        /// <summary>
        /// 需要的食材模板ID
        /// </summary>
        public string TemplateId { get; internal set; }
        
        /// <summary>
        /// 要求的形状
        /// 例如：切片、切丝等
        /// </summary>
        public Shape RequiredShape { get; internal set; }
        
        /// <summary>
        /// 要求的熟度
        /// 例如：生、半熟、全熟等
        /// </summary>
        public CookingStage RequiredStage { get; internal set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templateId">食材模板ID</param>
        /// <param name="requiredShape">要求的形状</param>
        /// <param name="requiredStage">要求的熟度</param>
        public RecipeIngredient(string templateId, Shape requiredShape, CookingStage requiredStage)
        {
            TemplateId = templateId;
            RequiredShape = requiredShape;
            RequiredStage = requiredStage;
        }
    }
}
