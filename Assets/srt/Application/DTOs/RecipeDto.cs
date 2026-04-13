using System.Collections.Generic;
using System.Linq;
using CookingGame.Core.Models;

namespace CookingGame.Application.DTOs
{
    /// <summary>
    /// 食谱数据传输对象
    /// 用于在不同层之间传递食谱数据
    /// </summary>
    public class RecipeDto
    {
        /// <summary>
        /// 食谱ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 食谱名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 食材列表
        /// </summary>
        public List<RecipeIngredientDto> Ingredients { get; set; }
        
        /// <summary>
        /// 最低得分
        /// </summary>
        public int MinScore { get; set; }
        
        /// <summary>
        /// 最高得分
        /// </summary>
        public int MaxScore { get; set; }

        /// <summary>
        /// 从实体创建DTO
        /// </summary>
        /// <param name="recipe">食谱实体</param>
        /// <returns>食谱DTO</returns>
        public static RecipeDto FromRecipe(Recipe recipe)
        {
            return new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Ingredients = recipe.Ingredients.Select(ri => RecipeIngredientDto.FromRecipeIngredient(ri)).ToList(),
                MinScore = recipe.MinScore,
                MaxScore = recipe.MaxScore
            };
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <returns>食谱实体</returns>
        public Recipe ToRecipe()
        {
            return new Recipe(
                Id,
                Name,
                Ingredients.Select(ri => ri.ToRecipeIngredient()).ToList(),
                MinScore,
                MaxScore
            );
        }

        /// <summary>
        /// 更新DTO
        /// </summary>
        /// <param name="dto">要更新的DTO</param>
        public void Update(RecipeDto dto)
        {
            if (dto == null) return;
            
            Name = dto.Name;
            Ingredients = dto.Ingredients;
            MinScore = dto.MinScore;
            MaxScore = dto.MaxScore;
        }
    }

    /// <summary>
    /// 食谱成分数传输对象
    /// 用于在不同层之间传递食谱成分数据
    /// </summary>
    public class RecipeIngredientDto
    {
        /// <summary>
        /// 食材模板ID
        /// </summary>
        public string TemplateId { get; set; }
        
        /// <summary>
        /// 要求的形状
        /// </summary>
        public Shape RequiredShape { get; set; }
        
        /// <summary>
        /// 要求的熟度
        /// </summary>
        public CookingStage RequiredStage { get; set; }

        /// <summary>
        /// 从实体创建DTO
        /// </summary>
        /// <param name="ingredient">食谱成分实体</param>
        /// <returns>食谱成分DTO</returns>
        public static RecipeIngredientDto FromRecipeIngredient(RecipeIngredient ingredient)
        {
            return new RecipeIngredientDto
            {
                TemplateId = ingredient.TemplateId,
                RequiredShape = ingredient.RequiredShape,
                RequiredStage = ingredient.RequiredStage
            };
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <returns>食谱成分实体</returns>
        public RecipeIngredient ToRecipeIngredient()
        {
            return new RecipeIngredient(TemplateId, RequiredShape, RequiredStage);
        }

        /// <summary>
        /// 更新DTO
        /// </summary>
        /// <param name="dto">要更新的DTO</param>
        public void Update(RecipeIngredientDto dto)
        {
            if (dto == null) return;
            
            TemplateId = dto.TemplateId;
            RequiredShape = dto.RequiredShape;
            RequiredStage = dto.RequiredStage;
        }
    }
}
