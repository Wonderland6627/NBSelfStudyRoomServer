using NBSSR.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Tests
{
    internal class MockData
    {
        public static UserInfo GetMockUserInfo()
        {
            return new UserInfo();
        }

        public static StudentInfo GetRandomMockUserInfo()
        {
            Random random = new Random();
            string[] names = { "Amelia", "Benjamin", "Chloe", "Daniel", "Emma", "Freya", "George", "Hannah", "Isaac", "Jessica",
                            "Lily", "Matthew", "Natalie", "Oliver", "Penelope", "Quentin", "Ruby", "Samuel", "Taylor", "Victoria",
                            "William", "Xavier", "Yasmine", "Zachary" };
            UserGender[] userGenders = { UserGender.Female, UserGender.Male };

            StudentInfo userInfo = new StudentInfo()
            {
                userID = random.Next(1, 100000),
                gender = userGenders[random.Next(0, userGenders.Length)],
                name = names[random.Next(0, names.Length)],
                userType = UserType.Student,
                phone = "12345678910",
            };

            return userInfo;
        }
    }
}
