using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryHelper.Models.HRM
{
    public class CompanyStructure
    {
        private int? iD;
        private string code;
        private string name;
        private int? parentID;
        private int? managerID;
        private int? flowPICID;
        private int? isActive;
        private int? branchID;
        private int? orderID;

        public int? ID { get => iD; set => iD = value; }
        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public int? ParentID { get => parentID; set => parentID = value; }
        public int? ManagerID { get => managerID; set => managerID = value; }
        public int? FlowPICID { get => flowPICID; set => flowPICID = value; }
        public int? IsActive { get => isActive; set => isActive = value; }
        public int? BranchID { get => branchID; set => branchID = value; }
        public int? OrderID { get => orderID; set => orderID = value; }

        public CompanyStructure(DataRow row)
        {
            this.ID = row.Field<int?>("ID");
            this.Code = row.Field<string>("Code");
            this.Name = row.Field<string>("Name");
            this.ParentID = row.Field<int?>("ParentID");
            this.ManagerID = row.Field<int?>("ManagerID");
            this.FlowPICID = row.Field<int?>("FlowPICID");
            this.IsActive = row.Field<int?>("IsActive");
            this.BranchID = row.Field<int?>("BranchID");
            this.OrderID = row.Field<int?>("OrderID");
        }
    }
}
