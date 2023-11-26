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

        private List<T> _datasList = new List<T>();
        public List<T> datasList => _datasList;

        private NBSSRLogger logger = new($"MiniDatabase<{typeof(T)}>");

        public MiniDatabase(string path)
        {
            _dbPath = path;
            _datasList = Load();
            _writer = new StreamWriter(_dbPath, false);
        }

        ~MiniDatabase()
        {
            _writer?.Dispose();
        }

        private List<T> Load()
        {
            if (string.IsNullOrEmpty(_dbPath))
            {
                return _datasList;
            }
            if (!File.Exists(_dbPath))
            {
                return _datasList;
            }

            string json = File.ReadAllText(_dbPath);
            MiniData<T> miniData = MiniData<T>.Build(json);

            return miniData?.datasList ?? new();
        }

        public void Save()
        {
            if (_datasList == null || _datasList.Count == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(_dbPath))
            {
                return;
            }

            MiniData<T> miniData = new MiniData<T>();
            miniData.datasList = _datasList;
            miniData.updateTime = DateTime.Now;

            try
            {
                string json = MiniData<T>.Build(miniData);
                _writer?.BaseStream.SetLength(0);
                _writer?.Write(json);
                _writer?.Flush();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
        }

        public void Add(T data)
        {
            _datasList.Add(data);
            logger.LogInfo($"add data: {data.Json()}");
            Save();
        }

        public bool Remove(T data)
        {
            if (_datasList.Remove(data))
            {
                logger.LogInfo($"remove data: {data.Json()}");
                Save();
                return true;
            }

            return false;
        }
    }
}