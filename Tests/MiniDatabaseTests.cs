using NBSSRServer.MiniDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Tests
{
    public class MiniDatabaseTests
    {
        MiniDatabase<string> database;
        List<string> dataList;

        public MiniDatabaseTests() 
        {
            database = new MiniDatabase<string>(MiniDataManager.GetDBPath("Tests"));
            dataList = new List<string>
            {
                "Data 1",
                "Data 2",
                "Data 3",
                "Data 4",
                "Data 5"
            };
        }

        // 模拟大量无序增删改查操作
        public void SimulateRandomOperations()
        {
            // 模拟随机增删改查操作
            var random = new Random();
            for (int i = 0; i < 5000; i++)
            {
                var randomIndex = random.Next(dataList.Count);
                var randomData = dataList[randomIndex];

                // 随机选择增删改查操作
                var operation = random.Next(4);
                switch (operation)
                {
                    case 0:
                        // 添加数据
                        database.Add(randomData);
                        break;
                    case 1:
                        // 移除数据
                        database.Remove(randomData);
                        break;
                    case 2:
                        // 修改数据（这里简化为移除后再添加）
                        database.Remove(randomData);
                        database.Add(randomData);
                        break;
                    case 3:
                        // 查询数据
                        var found = database.datasList.Contains(randomData);
                        break;
                }
            }

            // Assert
            // 这里可以添加断言来验证数据库状态是否符合预期
            // 例如，验证数据库中是否包含预期的数据，验证移除操作是否成功等等
        }
    }
}
