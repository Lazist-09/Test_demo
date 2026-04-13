using System.Collections.Generic;
using CookingGame.Core.Models;

namespace CookingGame.Core.Repositories
{
    /// <summary>
    /// 烹饪工具仓储接口
    /// 定义烹饪工具的持久化操作
    /// </summary>
    public interface ICookingToolRepository
    {
        /// <summary>
        /// 保存烹饪工具
        /// 如果工具没有ID则生成新的ID
        /// </summary>
        /// <param name="tool">要保存的烹饪工具</param>
        void Save(CookingTool tool);

        /// <summary>
        /// 根据ID获取烹饪工具
        /// </summary>
        /// <param name="id">工具ID</param>
        /// <returns>找到的工具，如果不存在则返回null</returns>
        CookingTool GetById(string id);

        /// <summary>
        /// 获取所有烹饪工具
        /// </summary>
        /// <returns>所有工具的列表</returns>
        List<CookingTool> GetAll();

        /// <summary>
        /// 根据ID删除工具
        /// </summary>
        /// <param name="id">工具ID</param>
        void Remove(string id);

        /// <summary>
        /// 更新烹饪工具
        /// </summary>
        /// <param name="tool">要更新的工具</param>
        void Update(CookingTool tool);
        
        /// <summary>
        /// 生成唯一ID
        /// </summary>
        /// <returns>生成的ID</returns>
        string GenerateId();
    }
}
