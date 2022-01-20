using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WalletService.Application.Models.GoogleModel;

namespace WalletService.Application.Contracts.Persistence
{
    public interface IGoogleRecaptchaV3Service
    {
        HttpClient _httpClient { get; set; }
        GRequestModel Request { get; set; }
        GResponseModel Response { get; set; }
        void InitializeRequest(GRequestModel request);
        Task<bool> Execute();
    }
}
