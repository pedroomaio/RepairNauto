using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Models
{
    public class SendEmailViewModel
    {
        public string Username { get; set; }
        public string SendGmail { get; set; }
        public string Message { get; set; }
    }
}
