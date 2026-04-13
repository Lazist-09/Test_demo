using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Configuration
{
    /// <summary>
    /// 游戏配置类
    /// 定义游戏的各种配置参数
    /// </summary>
    public class GameConfig
    {
        /// <summary>
        /// 烹饪进度速率
        /// 每次更新增加的进度值
        /// </summary>
        public float CookingProgressRate { get; set; } = 0.05f;
        
        /// <summary>
        /// 丢弃超时时间
        /// 完成后超过此时间仍未取出则自动丢弃
        /// </summary>
        public float DiscardTimeout { get; set; } = 5.0f;
        
        /// <summary>
        /// 工具最大容量
        /// </summary>
        public int MaxToolCapacity { get; set; } = 3;
        
        /// <summary>
        /// 有效的形状顺序
        /// 定义形状转换的合法顺序
        /// </summary>
        public List<Shape> ValidShapeOrder { get; set; }
        
        /// <summary>
        /// 形状得分倍数
        /// 不同形状对得分的影响
        /// </summary>
        public Dictionary<Shape, int> ShapeScoreMultiplier { get; set; }

        /// <summary>
        /// 构造函数
        /// 初始化游戏配置
        /// </summary>
        public GameConfig()
        {
            ValidShapeOrder = new List<Shape>
            {
                Shape.Whole,     // 整体
                Shape.Chunk,     // 块状
                Shape.Slice,     // 片状
                Shape.Julienne,  // 细丝状
                Shape.Crumbled   // 碎状
            };

            ShapeScoreMultiplier = new Dictionary<Shape, int>
            {
                { Shape.Whole, 1 },    // 整体得分倍数
                { Shape.Chunk, 2 },    // 块状得分倍数
                { Shape.Slice, 3 },    // 片状得分倍数
                { Shape.Julienne, 4 }, // 细丝状得分倍数
                { Shape.Crumbled, 5 }  // 碎状得分倍数
            };
        }
    }
}
