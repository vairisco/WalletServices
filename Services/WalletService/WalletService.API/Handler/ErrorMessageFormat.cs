using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletAPIService;

namespace WalletService.API.Handler
{
    public static class ErrorMessageFormat
    {
        public static void SuccessMessageHandler(ref ErrorModel errorModel)
        {
            errorModel.Error = 0;
            errorModel.Message = "Success";
        }

        public static void FailMessageHandler(ref ErrorModel errorModel)
        {
            errorModel.Error = 1;
            errorModel.Message = "Fail";
        }
    }
}
