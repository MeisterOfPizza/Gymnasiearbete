using System;

namespace ArenaShooter.Extensions
{

    static class UserUtils
    {

        private static Guid userId;

        static UserUtils()
        {
            userId = Guid.NewGuid();
        }

        public static string GetUsername()
        {
            return "Test username";
        }

        public static string GetUserId()
        {
            return userId.ToString();
        }

    }

}
