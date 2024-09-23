using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.Json
{
	public class RootMenu
	{
		public List<MenuJson> MenuJson { get; set; }
	}

	public class MenuJson
	{
		public string Object { get; set; }
		public string Kind { get; set; }
		public string DisplayName { get; set; }
		public List<ListMenu> ListMenu { get; set; }

	}
	public class ListMenu
	{
		public string ParentMenu { get; set; }
		public string Description { get; set; }
		public List<ChildrenMenu> ChildrenMenu { get; set; }
	}

	public class ChildrenMenu
	{
		public string ChildrenName { get; set; }
		public List<MenuItems> MenuItems { get; set; }
		
	}

	public class MenuItems
	{
		public string Namespace { get; set; }
		public string NameUsercontrol { get; set; }
		public string Header { get; set; }
		public string Icon { get; set; }
	}
}
