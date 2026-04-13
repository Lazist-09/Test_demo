namespace CookingGame.Core
{
    /// <summary>
    /// 数学工具类
    /// 提供Unity Mathf的替代实现
    /// </summary>
    public static class Mathf
    {
        /// <summary>
        /// 限制整数值在指定范围内
        /// </summary>
        /// <param name="value">要限制的值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>限制后的值</returns>
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// 限制浮点数值在指定范围内
        /// </summary>
        /// <param name="value">要限制的值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>限制后的值</returns>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
