using LibraryHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.Services
{
    internal class DataService
    {
        public static Users User { get; set; }
        public static bool IsLogin { get; set; } = false;
    }
}
