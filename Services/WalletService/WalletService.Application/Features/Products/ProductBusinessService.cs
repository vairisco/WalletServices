using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletService.Application.Contracts.Persistence;
using WalletService.Application.Features.Products.Interface;
using WalletService.Contracts.Persistence;
using WalletService.Domain.Entities;

namespace WalletService.Application.Features.Products
{
    public class ProductBusinessService : IProductBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductBusinessService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<Product> CreateProduct(Product productEntity)
        {
            _unitOfWork.BeginTransaction();
            await _productRepository.AddAsync(productEntity);
            var saved = await _unitOfWork.CommitAsync();
            return saved > 0 ? productEntity : null;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<bool> UpdateProduct(Product productEntity)
        {
            _unitOfWork.BeginTransaction();
            await _productRepository.UpdateAsync(productEntity);
            var saved = await _unitOfWork.CommitAsync();
            return saved > 0;
        }
    }
}
