using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tikects.Models
{
    public class User
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string TicketId { get; set; }

        public string TicketCode { get; set; }

        public string DateTime { get; set; }

        public string FullName { get; set; }

        //public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
    }
}
