using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.Application.Exceptions
{
    public class CaptchaRequestException : Exception
    {
        public CaptchaRequestException()
        {
        }
        public CaptchaRequestException(string message)
            : base(message)
        {
        }
        public CaptchaRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
