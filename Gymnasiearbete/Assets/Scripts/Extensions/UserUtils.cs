using ArenaShooter.Player;
using System;

namespace ArenaShooter.Extensions
{

    static class UserUtils
    {

        public const int MAX_USERNAME_LENGTH = 20;
        public const int MIN_USERNAME_LENGTH = 3;

        private static readonly Guid userId;

        static UserUtils()
        {
            userId = Guid.NewGuid();
        }

        public static string GetUsername()
        {
            return string.IsNullOrWhiteSpace(Profile.Username) ? Profile.DEFAULT_USERNAME : Profile.Username;
        }

        public static string SetUsername(string username)
        {
            username = username.ToASCII();
            username = username.Clamp(MIN_USERNAME_LENGTH, MAX_USERNAME_LENGTH, "_");

            Profile.Username = username;

            return username;
        }

        public static string GetUserId()
        {
            return userId.ToString();
        }

    }

}
