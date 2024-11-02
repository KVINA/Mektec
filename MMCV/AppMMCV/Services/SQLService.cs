using AppMMCV.Json;
using LibraryHelper.Methord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMMCV.Services
{
	internal class SQLService
	{
		private static SQLProvider method;
		public static SQLProvider Method { get { if (method == null) method = new SQLProvider(); return method; } }

		public static ServerDF Server { get { if (server == null) server = new ServerDF(); return server; } }

		private static ServerDF server;

	}

	public class ServerDF
	{
		private Server readJson()
		{
			//string Path_Json = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json\\Server_KVINA.json");
			string Path_Json = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json\\Server_MMCV.json");
			string json = System.IO.File.ReadAllText(Path_Json);
			List<Server> servers = JsonConvert.DeserializeObject<RootServer>(json).Server;
			var svHRM = servers.Where(s => s.ServerName == "SV68_HRM").First();
			return svHRM;
		}


		private SERVER sV46_Record;
		private SERVER sV69_Record;
		private SERVER sV68_PIMV;
		private SERVER sV68_HRM;
		private SERVER sV48_PIMV;
		private SERVER sV91_PIMV;
		public SERVER SV46_Record { get { if (sV46_Record == null) sV46_Record = new SERVER("10.80.1.46", "Record", "pim", "pimpass"); return sV46_Record; } }

		public SERVER SV69_Record { get { if (sV69_Record == null) sV69_Record = new SERVER("10.80.1.69", "Record", "pim", "pimpass"); return sV69_Record; } }

		public SERVER SV68_PIMV { get { if (sV68_PIMV == null) sV68_PIMV = new SERVER("10.80.1.68", "PIMV", "pim", "pimpass"); return sV68_PIMV; } }
		public SERVER SV68_HRM
		{
			get
			{
				if (sV68_HRM == null)
				{
					var svCF = readJson();
					if (svCF == null) return null;
					sV68_HRM = new SERVER(svCF.DataSource, svCF.InitialCatalog, svCF.UserId, svCF.Password);
				}
				return sV68_HRM;
			}
		}
		public SERVER SV48_PIMV { get { if (sV48_PIMV == null) sV48_PIMV = new SERVER("10.80.1.48", "PIMV", "pim", "pimpass"); return sV48_PIMV; } }
	}
}
