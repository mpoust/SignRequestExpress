﻿// comment

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Entities
{
    public class Request_AccountEntity
    {
        public Guid RequestFK { get; set; }

        public Guid AccountFK { get; set; }
    }
}
