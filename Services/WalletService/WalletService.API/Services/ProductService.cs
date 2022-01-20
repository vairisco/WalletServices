using Audit.Core;
using AuditLib.Grpc;
using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.API.Handler;
using WalletService.API.Handler.ReCaptchaHandler;
using WalletService.API.Handler.RSAHandler;
using WalletService.API.Helper;
using WalletService.API.ViewModels.Product;
using WalletService.Application.Contracts.Infrastructure;
using WalletService.Application.Features.Products.Interface;
using WalletService.Application.Models.GoogleModel;

namespace WalletAPIService
{
    public class ProductService : Product.ProductBase
    {
        private readonly IMapper _mapper;
        private readonly IProductBusinessService _productService;
        private readonly IRSAHandler _rsaHandler;
        private readonly IReCaptchaHandler _reCaptchaHandler;
        public ProductService(
            IRSAHandler rsaHandler, 
            IProductBusinessService productService, 
            IMapper mapper, 
            AuditCore auditLog,
            IReCaptchaHandler reCaptchaHandler
            )
        {
            _reCaptchaHandler = reCaptchaHandler;
            _rsaHandler = rsaHandler;
            _mapper = mapper;
            _productService = productService;
        }

        public override async Task<ProductsModel> GetProducts(EmptyMessage emptyMessage, ServerCallContext context) {
            try
            {
                //if (!(await _reCaptchaHandler.CheckReCaptcha(signUpModel.RecaptchaToken))) {
                //    return new ProductsModel();
                //};

                //call Business layer

                var products = await _productService.GetProducts();
                var productsConvertModel = _mapper.Map<IEnumerable<ProductModel>>(products);
                var replyModel = new ProductsModel();
                replyModel.Products.AddRange(productsConvertModel);

                return replyModel;
            }
            catch (Exception ex)
            {
                return new ProductsModel();
            }
        }
        public override async Task<ResultModel> CreateProduct(RequestModel requestModel, ServerCallContext context)
        {
            try
            {

                var requestDecrypt = _rsaHandler.Decrypt(requestModel.Data);
                var request = DeserializeHelper.DeserializeMethod<ProductCreateRequest>(requestDecrypt);

                if (!(await _reCaptchaHandler.CheckReCaptcha(requestModel.RecaptchaToken)))
                {
                    return new ResultModel();
                };


                using (AuditScope.Create(_ => _
                .EventType("Product:Create")
                .Target(() => request)))
                {
                    var productEntity = _mapper.Map<WalletService.Domain.Entities.Product>(request);
                    var product = await _productService.CreateProduct(productEntity);
                    var productCreatedEntity = product != null ? _mapper.Map<ProductModel>(product) : new ProductModel();

                    ErrorModel errorModel = new();
                    ErrorMessageFormat.SuccessMessageHandler(ref errorModel);

                    var resultModel = new ResultModel();
                    resultModel.Data = productCreatedEntity;
                    resultModel.ErrorModel = errorModel;

                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                var resultModel = new ResultModel();
                ProductModel productModel = new ProductModel();
                ErrorModel errorModel = new();
                ErrorMessageFormat.FailMessageHandler(ref errorModel);
                resultModel.ErrorModel = errorModel;
                resultModel.Data = productModel;
                return resultModel;
            }
        }

        //public override async Task<ProductModel> UpdateProduct(RequestModel requestModel, ServerCallContext context)
        //{
        //    try
        //    {
        //        var requestDecrypt = _rsaHandler.Decrypt(requestModel.Data);
        //        var request = DeserializeHelper.DeserializeMethod<ProductUpdateRequest>(requestDecrypt);

        //        using (AuditScope.Create("Product:Update", () => request))
        //        {
        //            var productEntity = _mapper.Map<WalletService.Domain.Entities.Product>(request);
        //            await _productService.UpdateProduct(productEntity);

        //            var productUpdateEntity = _mapper.Map<ProductModel>(productEntity);

        //            ErrorModel errorModel = new();
        //            ErrorMessageFormat.SuccessMessageHandler(ref errorModel);
        //            productUpdateEntity.ErrorModel = errorModel;

        //            return productUpdateEntity;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ProductModel productModel = new ProductModel();
        //        ErrorModel errorModel = new();
        //        ErrorMessageFormat.FailMessageHandler(ref errorModel);
        //        productModel.ErrorModel = errorModel;
        //        return productModel;
        //    }
        //}
    }
}
