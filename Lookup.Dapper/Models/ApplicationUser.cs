using System;
using System.Collections.Generic;
using System.Text;

namespace Amics.Lookup.Models
{
    // reference to LstUsersList from ngc
    public class ApplicationUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
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
        public int AmicsUser { get; set; }
        public int EmpList { get; set; }
        public int InvTrans { get; set; }
        public string ForgotPwdAns { get; set; }

    }
}
