using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Entities
{
    public class Approval
    {
        public Guid Id { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public byte ApprovalStatus { get; set; }

        public Guid ApproverID { get; set; }
    }
}
