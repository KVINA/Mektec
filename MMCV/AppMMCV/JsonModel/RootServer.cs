using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.Json
{
	public class RootServer
	{
		public List<Server> Server {  get; set; }
	}

	public class Server
	{
		public string ServerName { get; set; }
		public string DataSource { get; set; }
		public string InitialCatalog { get; set; }
		public string UserId { get; set; }
		public string Password { get; set; }
	}
}
