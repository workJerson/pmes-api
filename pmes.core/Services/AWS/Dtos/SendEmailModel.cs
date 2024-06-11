using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.AWS.Dtos
{
    public class SendEmailModel
    {
        //public List<string> To { get; set; }
        public string To { get; set; }
        public List<string> Bcc { get; set; }
        public List<string> Cc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
