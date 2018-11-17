using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpress.Models.PatchModels
{
    public class StatusPatch
    {
        public string Op { get; set; }

        public string Path { get; set; }

        public string Value { get; set; }
    }
}
