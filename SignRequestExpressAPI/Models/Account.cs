using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public class Account : Resource
    {
        public string AccountName { get; set; }

        public DateTime AddedDate { get; set; }

        public string LogoURI { get; set; }

        public string WebsiteURL { get; set; }

        public Guid AssociateFK { get; set; }

        public Guid AccountContactFK { get; set; }
    }
}
