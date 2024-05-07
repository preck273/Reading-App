﻿using System;
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
        private static string pfp;

        public static string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public static string Pfp
        {
            get { return pfp; }
            set { userName = value; }
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

    }

    public enum userLevel
    {
        EDITOR,
        WRITER,
        READER
    }
}
