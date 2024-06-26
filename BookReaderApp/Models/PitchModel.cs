﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.Models
{
    public class PitchModel
    {
        public string Pid { get; set; }
        public string Userid { get; set; }
        public string PitchContent { get; set; }
        public bool IsPublished { get; set; }
        public bool IsReviewed { get; set; }
        public bool Published { get; set; }
    }
}
