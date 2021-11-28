using Audit.Core;
using AuditLib.Grpc;
using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WalletService.API.Handler.RSA;
using WalletService.API.Helper;
using WalletService.API.ViewModels.Product;
using WalletService.Application.Features.Products.Interface;

namespace WalletAPIService
{
    public class ProductService : Product.ProductBase
    {
        private readonly IMapper _mapper;
        private readonly IProductBusinessService _productService;
        private readonly IRSAHandler _rsaHandler;
        public ProductService(IRSAHandler rsaHandler, IProductBusinessService productService, IMapper mapper, AuditCore auditLog)
        {
            _rsaHandler = rsaHandler;
            _mapper = mapper;
            _productService = productService;
        }

        public override async Task<ProductsModel> GetProducts(EmptyMessage emptyMessage, ServerCallContext context) {
            try
            {
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
        public override async Task<ProductModel> CreateProduct(RequestModel requestModel, ServerCallContext context)
        {
            //throw new Exception("This is sample exception 2");
            var requestDecrypt = _rsaHandler.Decrypt(requestModel.Data);
            var request = DeserializeHelper.DeserializeMethod<ProductCreateRequest>(requestDecrypt);

            using (AuditScope.Create(_ => _
            .EventType("Product:Create")
            .Target(() => request)))
            {
                var productEntity = _mapper.Map<WalletService.Domain.Entities.Product>(request);
                var product = await _productService.CreateProduct(productEntity);
                var productCreatedEntity = product != null ? _mapper.Map<ProductModel>(product) : new ProductModel();
                return productCreatedEntity;
            }
        }

        public override async Task<ProductModel> UpdateProduct(RequestModel requestModel, ServerCallContext context)
        {
            var requestDecrypt = _rsaHandler.Decrypt(requestModel.Data);
            var request = DeserializeHelper.DeserializeMethod<ProductUpdateRequest>(requestDecrypt);

            using (AuditScope.Create("Product:Update", () => request))
            {
                var productEntity = _mapper.Map<WalletService.Domain.Entities.Product>(request);
                await _productService.UpdateProduct(productEntity);
                return _mapper.Map<ProductModel>(productEntity);
            }
        }
    }
}
