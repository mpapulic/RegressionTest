using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginLibrary.Models
{
        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Teams { get; set; }
            public string DesignatedApprover { get; set; }
            public string CanSubmitToCH { get; set; }
            public string CanApproveOwnJob { get; set; }
            public string Active { get; set; }
            public AdminUser Admin { get; set; }
            public string User_Guid { get; set; }
            public EditUser EditEnabled { get; set; }
            public DeleteUser DeleteEnabled { get; set; }

        }

        public class AdminUser
        {
            public bool isAdmin { get; set; }
            public bool isOrgAdmin { get; set; }
        }
        public class EditUser
        {
            public bool locked { get; set; }
            public string email { get; set; }
        }
        public class DeleteUser
        {
            public bool canDelete { get; set; }
            public string userGuid { get; set; }
        }
}
