using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpress.Models.ResponseModels
{
    public class SalesAccountsResponse : Resource
    {
        // rel?

        public List<Dictionary<string, object>> Value { get; set; }
    }
}
