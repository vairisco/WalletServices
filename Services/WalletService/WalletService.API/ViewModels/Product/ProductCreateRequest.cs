namespace WalletService.API.ViewModels.Product
{
    public class ProductCreateRequest
    {
        public string Name { get; set; } = "";
        public string RecaptchaToken { get; set; }
    }
}
