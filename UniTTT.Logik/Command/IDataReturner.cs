﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Command
{
    public interface IDataReturner
    {
        event DataReturnHandler DataReturnEvent;
    }
}
