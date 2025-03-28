﻿using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.UnitsOfWork.Implementations
{
    public class ProductsUnitOfWork : GenericUnitsOfWork<Product>, IProductsUnitOfWork
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsUnitOfWork(IGenericRepository<Product> repository, IProductsRepository productsRepository) : base(repository)
        {
            _productsRepository = productsRepository;
        }

        public override async Task<ActionResponse<Product>> DeleteAsync(int Id) => await _productsRepository.DeleteAsync(Id);
        public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination) => await _productsRepository.GetAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination) => await _productsRepository.GetTotalRecordAsync(pagination);

        public override async Task<ActionResponse<Product>> GetAsync(int id) => await _productsRepository.GetAsync(id);

        public async Task<ActionResponse<Product>> AddFullAsync(ProductDTO productDTO) => await _productsRepository.AddFullAsync(productDTO);

        public async Task<ActionResponse<Product>> UpdateFullAsync(ProductDTO productDTO) => await _productsRepository.UpdateFullAsync(productDTO);

        public async Task<ActionResponse<ImageDTO>> AddImageAsync(ImageDTO imageDTO) => await _productsRepository.AddImageAsync(imageDTO);
        public async Task<ActionResponse<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO) => await _productsRepository.RemoveLastImageAsync(imageDTO);

        public override async Task<ActionResponse<IEnumerable<Product>>> GetPaginationAsync(PaginationDTO pagination) => await _productsRepository.GetPaginationAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _productsRepository.GetTotalPagesAsync(pagination);

    }
}