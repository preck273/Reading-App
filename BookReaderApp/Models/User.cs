using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.Models
{
    public sealed class User
    {
        private static readonly object locker = new object(); 
        private static User instance = null; 

        public User() { }

        public static string UserName { get; set; }
        public static string Image { get; set; }
        public static userLevel Level { get; set; }
        public static string UserId { get; set; }
        public static string Email { get; set; }


        //Locking for thread safety on Singleton
        public static User Instance()
        {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new User();
                    }
                    return instance;
                }
        }
    }

    public enum userLevel
    {
        EDITOR,
        WRITER,
        READER
    }
}
