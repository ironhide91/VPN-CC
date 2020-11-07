using System;

namespace Vpn.BusinessLogic
{
    [Serializable]
    public class UnknownApiException : Exception
    {
        public UnknownApiException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}