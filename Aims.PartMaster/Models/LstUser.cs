using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Aims.Core.Models
{
    public class LstUser
    {
        [Key]
        public string userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string password { get; set; }
        public string signature { get; set; }
        public string warehouse { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string userDataBase { get; set; }
        public int buyer { get; set; }
        public int salesPerson { get; set; }
        public int webAccess { get; set; }
        public int amicsUser { get; set; }
        public int EmpList { get; set; }
        public int InvTrans { get; set; }
        public string forgotpwdans { get; set; }
    }
}
