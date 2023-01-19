using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Assets.Enum.Client
{
    public enum EnumAssets
    {
        Unknown = 0,
        NewContact = 1,
        MessageRecieved = 5,
        MessageDeletionRequest = 6,
        ContactDisconnection = 10,
        WaitingForMessage = 77,
        ServerShutdown = byte.MaxValue
    }
}
