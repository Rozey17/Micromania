﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micromania.Console
{
    public class StarStatusReached : IDomainEvent
    {
        public Client Client { get; set; }
    }
}
