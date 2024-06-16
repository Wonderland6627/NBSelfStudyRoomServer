using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using NBSSRServer.Logger;
using NBSSRServer.Extensions;

namespace NBSSRServer.MiniDatabase
{
    /// <summary>
    /// 使用Json模拟数据库服务：增删改查
    /// </summary>
    public class MiniDatabase<T> where T : class
    {
        private StreamWriter _writer;
        private string _dbPath;
        private MiniData<T> _miniData;

        public List<T> datasList => _miniData.datasList;

        private NBSSRLogger logger = new($"MiniDatabase<{typeof(T).Name}>");

        public MiniDatabase(string path)
        {
            Load(path);
            _writer = new StreamWriter(path, false);
        }

        ~MiniDatabase()
        {
            _writer?.Dispose();
        }

        private void Load(string path)
        {
            _dbPath = path;
            if (string.IsNullOrEmpty(_dbPath))
            {
                return;
            }

            MiniData<T> miniData = default;
            if (File.Exists(_dbPath))
            {
                string json = File.ReadAllText(_dbPath);
                miniData = MiniData<T>.Build(json);
            }
            if (miniData == null)
            {
                miniData = new();
                miniData.createdTime = DateTime.Now;
                miniData.datasList = new();
            }
            _miniData = miniData;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_dbPath))
            {
                return;
            }
            if (_miniData == null)
            {
                return;
            }

            _miniData.updatedTime = DateTime.Now;
            try
            {
                string json = MiniData<T>.Build(_miniData);
                _writer?.BaseStream.SetLength(0);
                _writer?.Write(json);
                _writer?.Flush();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
        }

        //添加数据 根据repeatChecker决定是否检查重复元素
        public void Add(T data, Predicate<T> repeatChecker = null)
        {
            if (datasList.Count != 0 && repeatChecker != null && repeatChecker(data)) //是否需要检查重复元素
            {
                logger.LogInfo($"repeat data, ignore add behaviour: {data.Json()},");
                return;
            }

            datasList.Add(data);
            logger.LogInfo($"add data: {data.Json()}");
            Save();
        }

        public bool Remove(T data)
        {
            if (datasList.Remove(data))
            {
                logger.LogInfo($"remove data: {data.Json()}");
                Save();
                return true;
            }

            return false;
        }

        //根据predicate更新数据 如果数据不存在 根据append决定是否添加
        public bool Update(Predicate<T> predicate, T newData, bool append = false)
        {
            int index = datasList.FindIndex(predicate);
            if (index == -1)
            {
                logger.LogInfo($"can not found data: {newData.Json()}, append: {append}");
                if (append)
                {
                    Add(newData);
                }

                return false;
            }

            datasList[index] = newData;
            logger.LogInfo($"update data at: [{index}] {newData.Json()}");
            Save();

            return true;
        }

        public T Get(Predicate<T> predicate)
        {
            return datasList.Find(predicate);
        }
    }
}