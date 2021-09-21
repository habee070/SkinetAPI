using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkinetAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenerateRepo<Product> _productRepo;
        private readonly IGenerateRepo<ProductBrand> _brandRepo;
        private readonly IGenerateRepo<ProductType> _typeRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenerateRepo<Product> productRepo,
            IGenerateRepo<ProductBrand> brandRepo,
            IGenerateRepo<ProductType> typeRepo,
            IMapper mapper
            )
        {
            _productRepo = productRepo;
            _brandRepo = brandRepo;
            _typeRepo = typeRepo;
            _mapper = mapper;
        }

        [HttpGet]
       public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepo.ListAsync(spec);

            //var products = await _productRepo.ListAllAsync();  //-> v1
            // return Ok(products); 
            //return products.Select(product => new ProductToReturnDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    PictureUrl = product.PictureUrl,
            //    Price = product.Price,
            //    ProductBrand = product.ProductBrand.Name,
            //    ProductType = product.ProductType.Name
            //}).ToList();  // -> v3
            return Ok(_mapper.Map<IReadOnlyList<Product> ,IReadOnlyList<ProductToReturnDto>>(products));
        }

        // note of you make same httpget swagger it error 
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id) // -> v1 ,v2 public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            // return Ok(await _productRepo.GetByIdAsync(id));  // -> v1
            // return Ok(await _productRepo.GetEntityWithSpec(spec)); // v2
            var product = await _productRepo.GetEntityWithSpec(spec);
            //return new ProductToReturnDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    PictureUrl = product.PictureUrl,
            //    Price = product.Price,
            //    ProductBrand = product.ProductBrand.Name,
            //    ProductType = product.ProductType.Name

            //};   // ->v3

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok( await _brandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _typeRepo.ListAllAsync());
        }
    }
}
