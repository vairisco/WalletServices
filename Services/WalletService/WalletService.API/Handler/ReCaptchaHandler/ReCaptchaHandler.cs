using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Application.Contracts.Infrastructure;
using WalletService.Application.Models.GoogleModel;

namespace WalletService.API.Handler.ReCaptchaHandler
{
    public class ReCaptchaHandler : IReCaptchaHandler
    {
        private readonly IGoogleRecaptchaV3Service _gService;
        private readonly IConfiguration _configuration;
        public ReCaptchaHandler(IConfiguration configuration, IGoogleRecaptchaV3Service gService)
        {
            _configuration = configuration;
            _gService = gService;
        }
        public async Task<bool> CheckReCaptcha(string recaptchaToken)
        {
            GRequestModel rm = new GRequestModel(
                recaptchaToken,
                _configuration.GetValue<string>("GoogleRecaptchaV2:Secret"),
                _configuration.GetValue<string>("GoogleRecaptchaV2:ApiUrl"));

            _gService.InitializeRequest(rm);

            if (!await _gService.Execute())
            {
                return false;
            }
            return true;
        }
    }
}
