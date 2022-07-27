using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstUser
    {  
        public int ActionFlag { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string Signature { get; set; }
        public string Warehouse { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserDataBase { get; set; }
        public int Buyer { get; set; }
        public int SalesPerson { get; set; } 
        public int WebAccess { get; set; }
        public int AmicsUser { get; set; } = 0;
        public int EmpList { get; set; } = 0;
        public int InvTrans { get; set; } = 0;
        public string Forgotpwdans { get; set; }
        public string Createdby { get; set; }
    }
}
