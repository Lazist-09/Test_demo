using UnityEngine;
using CookingGame.Infrastructure;

namespace CookingGame.Presentation
{
    /// <summary>
    /// 游戏启动管理器
    /// 负责初始化依赖注入容器和启动游戏
    /// </summary>
    public class GameStarter : MonoBehaviour
    {
        /// <summary>
        /// 启动游戏
        /// 初始化依赖注入容器
        /// </summary>
        private void Start()
        {
            // 初始化依赖注入容器
            DependencyContainer.Initialize();
            
            Debug.Log("游戏初始化完成！");
            Debug.Log("依赖注入容器已初始化");
            Debug.Log("可用服务：");
            Debug.Log("- 物品管理用例");
            Debug.Log("- 食谱管理用例");
            Debug.Log("- 订单管理用例");
            Debug.Log("- 烹饪工具管理用例");
            Debug.Log("- 订单提交用例");
        }
    }
}
