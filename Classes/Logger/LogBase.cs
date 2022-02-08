using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace COT.API.Logger
{
    [ExcludeFromCodeCoverage]
    public abstract class LogBase
    {
        public abstract string Log(string Message, bool logSwitch = true);
    }
}
