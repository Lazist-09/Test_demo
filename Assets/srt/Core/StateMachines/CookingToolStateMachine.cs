using CookingGame.Core.Models;

namespace CookingGame.Core.StateMachines
{
    /// <summary>
    /// 烹饪工具状态机
    /// 管理烹饪工具的状态转换和进度更新
    /// </summary>
    public class CookingToolStateMachine
    {
        /// <summary>
        /// 关联的烹饪工具
        /// </summary>
        private CookingTool _tool;
        
        /// <summary>
        /// 当前状态
        /// </summary>
        private CookingToolState _currentState;

        /// <summary>
        /// 获取当前状态
        /// </summary>
        public CookingToolState CurrentState => _currentState;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tool">关联的烹饪工具</param>
        public CookingToolStateMachine(CookingTool tool)
        {
            _tool = tool;
            _currentState = CookingToolState.Idle;  // 初始状态为空闲
        }

        /// <summary>
        /// 进入空闲状态
        /// 工具未运行，可以添加或取出食材
        /// </summary>
        public void EnterIdle()
        {
            _tool._currentState = CookingToolState.Idle;
        }

        /// <summary>
        /// 进入准备状态
        /// 已添加食材，等待启动
        /// 只能从空闲状态转换
        /// </summary>
        public void EnterPreparing()
        {
            if (_currentState == CookingToolState.Idle)
            {
                _currentState = CookingToolState.Preparing;
            }
        }

        /// <summary>
        /// 进入烹饪状态
        /// 开始处理食材
        /// 只能从准备状态转换，且必须有食材
        /// </summary>
        public void EnterCooking()
        {
            if (_tool._currentState == CookingToolState.Preparing && _tool.InputItems.Count > 0)
            {
                _tool._currentState = CookingToolState.Cooking;
            }
        }

        /// <summary>
        /// 进入暂停状态
        /// 暂停烹饪进度
        /// 只能从烹饪状态转换
        /// </summary>
        public void EnterPaused()
        {
            if (_tool._currentState == CookingToolState.Cooking)
            {
                _tool._currentState = CookingToolState.Paused;
            }
        }

        /// <summary>
        /// 进入完成状态
        /// 烹饪完成，可以取出输出
        /// 可以从烹饪或暂停状态转换
        /// </summary>
        public void EnterFinished()
        {
            if (_tool._currentState == CookingToolState.Cooking || _tool._currentState == CookingToolState.Paused)
            {
                _tool._currentState = CookingToolState.Finished;
            }
        }

        /// <summary>
        /// 进入丢弃状态
        /// 超时或发生错误，强制丢弃
        /// </summary>
        public void EnterDiscarded()
        {
            _tool._currentState = CookingToolState.Discarded;
        }

        /// <summary>
        /// 更新处理进度
        /// 在烹饪过程中增加进度值
        /// </summary>
        /// <param name="amount">增加的进度值</param>
        public void UpdateProgress(float amount)
        {
            if (_tool._currentState == CookingToolState.Cooking)
            {
                _tool.AddProgress(amount);
                if (_tool.CurrentProgress >= 1.0f)
                {
                    EnterFinished();  // 进度完成，进入完成状态
                }
            }
        }

        /// <summary>
        /// 强制完成
        /// 立即完成烹饪，用于意外情况
        /// </summary>
        public void ForceFinish()
        {
            if (_tool._currentState == CookingToolState.Cooking || _tool._currentState == CookingToolState.Paused)
            {
                _tool.CurrentProgress = 1.0f;  // 设置进度为100%
                EnterFinished();
            }
        }

        /// <summary>
        /// 超时丢弃
        /// 完成后超过时限仍未取出，强制丢弃
        /// </summary>
        public void TimeoutDiscard()
        {
            if (_tool._currentState == CookingToolState.Finished)
            {
                _tool.CurrentProgress = 1.0f;
                EnterDiscarded();
            }
        }
    }
}
