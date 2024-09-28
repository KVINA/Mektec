using AppMMCV.ViewModels;
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
		public static Users UserInfo { get; set; }
		public static bool IsLogin { get; set; } = false;
		internal static GlobalVM GlobalVM { get { if (globalVM == null) globalVM = new GlobalVM(); return globalVM; } set => globalVM = value; }

		private static GlobalVM globalVM;

	}
}
