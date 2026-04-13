using System.Collections.Generic;
using UnityEngine;
using CookingGame.Core.Repositories;
using CookingGame.Core.Models;

namespace CookingGame.Infrastructure.Repositories
{
    /// <summary>
    /// 内存烹饪工具仓储
    /// 使用内存字典存储烹饪工具数据，用于开发和测试
    /// </summary>
    public class InMemoryCookingToolRepository : ICookingToolRepository
    {
        /// <summary>
        /// 烹饪工具存储字典
        /// </summary>
        private readonly Dictionary<string, CookingTool> _tools = new Dictionary<string, CookingTool>();
        
        /// <summary>
        /// ID计数器
        /// 用于生成唯一ID
        /// </summary>
        private int _idCounter = 0;

        /// <summary>
        /// 保存烹饪工具
        /// 如果工具没有ID则生成新的ID
        /// </summary>
        /// <param name="tool">要保存的烹饪工具</param>
        public void Save(CookingTool tool)
        {
            if (string.IsNullOrEmpty(tool.Id))
            {
                tool.Id = GenerateId();
            }
            Debug.Log($"InMemoryCookingToolRepository.Save: {tool.Id}, Type: {tool.Type}, total={_tools.Count + 1}");
            _tools[tool.Id] = tool;
        }

        /// <summary>
        /// 根据ID获取烹饪工具
        /// </summary>
        /// <param name="id">工具ID</param>
        /// <returns>找到的工具，如果不存在则返回null</returns>
        public CookingTool GetById(string id)
        {
            Debug.Log($"InMemoryCookingToolRepository.GetById: {id}, count={_tools.Count}");
            if (_tools.TryGetValue(id, out var tool))
            {
                Debug.Log($"Found tool: {tool.Id}, Type: {tool.Type}");
                return tool;
            }
            else
            {
                Debug.Log($"Tool not found: {id}");
                return null;
            }
        }

        /// <summary>
        /// 获取所有烹饪工具
        /// </summary>
        /// <returns>所有工具的列表</returns>
        public List<CookingTool> GetAll()
        {
            return new List<CookingTool>(_tools.Values);
        }

        /// <summary>
        /// 根据ID删除工具
        /// </summary>
        /// <param name="id">工具ID</param>
        public void Remove(string id)
        {
            _tools.Remove(id);
        }

        /// <summary>
        /// 更新烹饪工具
        /// </summary>
        /// <param name="tool">要更新的工具</param>
        public void Update(CookingTool tool)
        {
            Debug.Log($"InMemoryCookingToolRepository.Update: {tool.Id}, InputItems count: {tool.InputItems.Count}");
            if (_tools.ContainsKey(tool.Id))
            {
                _tools[tool.Id] = tool;
            }
        }

        /// <summary>
        /// 生成唯一ID
        /// </summary>
        /// <returns>生成的ID</returns>
        public string GenerateId()
        {
            return $"tool_{++_idCounter}";
        }
    }
}
