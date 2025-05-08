using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Logger
{
    public interface ICustomDbLogger
    {
        public void LogAsync(Guid PersonId, string FullName, LogTypeEnum LogType, string Action);
    }

}

