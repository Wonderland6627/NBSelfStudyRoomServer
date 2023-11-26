using NBSSR.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Test
{
    internal class MockData
    {
        public static UserInfo GetMockUserInfo()
        {
            return new UserInfo()
            {
                userID = 1,
                userGender = UserGender.Male,
                userName = "Test",
                userType = UserType.Student,
            };
        }

        public static UserInfo GetRandomMockUserInfo()
        {
            Random random = new Random();
            string[] names = { "Amelia", "Benjamin", "Chloe", "Daniel", "Emma", "Freya", "George", "Hannah", "Isaac", "Jessica",
                            "Lily", "Matthew", "Natalie", "Oliver", "Penelope", "Quentin", "Ruby", "Samuel", "Taylor", "Victoria",
                            "William", "Xavier", "Yasmine", "Zachary" };
            UserType[] userTypes = { UserType.Admin, UserType.Student };
            UserGender[] userGenders = { UserGender.Female, UserGender.Male };

            UserInfo userInfo = new UserInfo()
            {
                userID = random.Next(1, 100000),
                userName = names[random.Next(0, names.Length)],
                userType = userTypes[random.Next(0, userTypes.Length)],
                userGender = userGenders[random.Next(0, userGenders.Length)],
            };

            return userInfo;
        }
    }
}
