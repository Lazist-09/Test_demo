using UnityEngine;
using CookingGame.Infrastructure;

namespace CookingGame.Presentation
{
    /// <summary>
    /// 游戏启动引导器
    /// 负责初始化游戏依赖注入容器和设置用例
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        /// <summary>
        /// 开始
        /// 在场景加载时初始化游戏
        /// </summary>
        private void Start()
        {
            // 初始化依赖注入容器
            DependencyContainer.Initialize();
            
            // 设置用例
            SetupUseCases();
        }

        /// <summary>
        /// 设置用例
        /// 初始化所有业务用例并注入到控制器
        /// </summary>
        private void SetupUseCases()
        {
            // 这是一个简化的启动示例
            // 实际项目中应该使用完整的依赖注入框架
        }
    }
}
