using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Validation
{
    /// <summary>
    /// 烹饪工具验证服务
    /// 验证烹饪工具的各种操作是否合法
    /// </summary>
    public interface ICookingToolValidationService
    {
        /// <summary>
        /// 验证是否可以添加食材
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        Result CanAddItem(CookingTool tool);
        
        /// <summary>
        /// 验证是否可以启动烹饪
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        Result CanStartCooking(CookingTool tool);
        
        /// <summary>
        /// 验证是否可以暂停烹饪
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        Result CanPauseCooking(CookingTool tool);
        
        /// <summary>
        /// 验证是否可以完成烹饪
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        Result CanCompleteCooking(CookingTool tool);
        
        /// <summary>
        /// 验证是否可以取出输出
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        Result CanRemoveOutput(CookingTool tool);
        
        /// <summary>
        /// 验证是否可以清空输入
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        Result CanClearInput(CookingTool tool);
    }
    
    /// <summary>
    /// 默认烹饪工具验证服务实现
    /// </summary>
    public class CookingToolValidationService : ICookingToolValidationService
    {
        /// <summary>
        /// 验证是否可以添加食材
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        public Result CanAddItem(CookingTool tool)
        {
            var errors = new List<string>();
            
            if (tool == null)
            {
                errors.Add("工具不存在");
                return Result.Failure(errors);
            }
            
            if (tool.IsRunning)
            {
                errors.Add("工具正在运行,无法添加食材");
            }
            
            if (tool.InputItems.Count >= tool.GetMaxCapacity())
            {
                errors.Add($"工具已满,最大容量为 {tool.GetMaxCapacity()}");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        /// <summary>
        /// 验证是否可以启动烹饪
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        public Result CanStartCooking(CookingTool tool)
        {
            var errors = new List<string>();
            
            if (tool == null)
            {
                errors.Add("工具不存在");
                return Result.Failure(errors);
            }
            
            if (tool.InputItems.Count == 0)
            {
                errors.Add("工具中没有食材");
            }
            
            if (tool.IsRunning)
            {
                errors.Add("工具已经在运行");
            }
            
            if (tool.OutputItem != null)
            {
                errors.Add("工具已有输出,请先清理");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        /// <summary>
        /// 验证是否可以暂停烹饪
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        public Result CanPauseCooking(CookingTool tool)
        {
            var errors = new List<string>();
            
            if (tool == null)
            {
                errors.Add("工具不存在");
                return Result.Failure(errors);
            }
            
            if (!tool.IsRunning)
            {
                errors.Add("工具未在运行,无法暂停");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        /// <summary>
        /// 验证是否可以完成烹饪
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        public Result CanCompleteCooking(CookingTool tool)
        {
            var errors = new List<string>();
            
            if (tool == null)
            {
                errors.Add("工具不存在");
                return Result.Failure(errors);
            }
            
            if (tool.InputItems.Count == 0)
            {
                errors.Add("工具中没有食材");
            }
            
            if (tool.OutputItem != null)
            {
                errors.Add("工具已有输出");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        /// <summary>
        /// 验证是否可以取出输出
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        public Result CanRemoveOutput(CookingTool tool)
        {
            var errors = new List<string>();
            
            if (tool == null)
            {
                errors.Add("工具不存在");
                return Result.Failure(errors);
            }
            
            if (tool.IsRunning)
            {
                errors.Add("工具正在运行,无法取出输出");
            }
            
            if (tool.OutputItem == null)
            {
                errors.Add("工具没有输出");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        /// <summary>
        /// 验证是否可以清空输入
        /// </summary>
        /// <param name="tool">烹饪工具</param>
        /// <returns>验证结果</returns>
        public Result CanClearInput(CookingTool tool)
        {
            var errors = new List<string>();
            
            if (tool == null)
            {
                errors.Add("工具不存在");
                return Result.Failure(errors);
            }
            
            if (tool.IsRunning)
            {
                errors.Add("工具正在运行,无法清空输入");
            }
            
            return errors.Count == 0 
                ? Result.Success() 
                : Result.Failure(errors);
        }
    }
}
