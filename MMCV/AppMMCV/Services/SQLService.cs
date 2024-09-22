using LibraryHelper.Methord;
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
        private SERVER sV46_Record;
        private SERVER sV69_Record;
        private SERVER sV68_PIMV;
        private SERVER sV68_HRM;
        private SERVER sV48_PIMV;
        public SERVER SV46_Record { get { if (sV46_Record == null) sV46_Record = new SERVER("10.80.1.46", "Record", "pim", "pimpass"); return sV46_Record; } }

        public SERVER SV69_Record { get { if (sV69_Record == null) sV69_Record = new SERVER("10.80.1.69", "Record", "pim", "pimpass"); return sV69_Record; } }

        public SERVER SV68_PIMV { get { if (sV68_PIMV == null) sV68_PIMV = new SERVER("10.80.1.68", "PIMV", "pim", "pimpass"); return sV68_PIMV; } }
        public SERVER SV68_HRM { get { if (sV68_HRM == null) sV68_HRM = new SERVER("10.80.1.68", "HRM", "pim", "pimpass"); return sV68_HRM; } }
        public SERVER SV48_PIMV { get { if (sV48_PIMV == null) sV48_PIMV = new SERVER("10.80.1.48", "PIMV", "pim", "pimpass"); return sV48_PIMV; } }
    }
}
