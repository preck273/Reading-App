using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.Models
{
    internal class User
    {
        private static string userName;
        private static userLevel level;
        private static string userId;
        private static string image;
        private static string email;

        public static string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public static string Image
        {
            get { return image; }
            set { image = value; }
        }
        public static userLevel Level
        {
            get { return level; }
            set { level = value; }
        }
        public static string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public static string Email
        {
            get { return email; }
            set { email = value; }
        }

    }

    public enum userLevel
    {
        EDITOR,
        WRITER,
        READER
    }
}
