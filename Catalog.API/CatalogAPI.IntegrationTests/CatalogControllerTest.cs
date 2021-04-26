using AutoMapper;
using Catalog.API.Controllers;
using Catalog.API.Data;
using Catalog.API.DTO;
using Catalog.API.Profiles;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace CatalogAPI.IntegrationTests
{
    [TestClass]
    public class CatalogControllerTest
    {
        private CatalogContext _conext;
        private IProductRepository _repository;
        private CatalogController _catalogController;
        private IMapper _mapper;
       
        [TestInitialize]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CatalogProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            DbContextOptions<CatalogContext> options;
            var builder = new DbContextOptionsBuilder<CatalogContext>();
            builder.UseInMemoryDatabase("CatalogDBTest");
            options = builder.Options;
            _conext = new CatalogContext(options);
            _repository = new ProductRepository(_conext);

            _catalogController = new CatalogController(_repository, _mapper);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void SaveProduct()
        {
            ProductDto product = new ProductDto { Name="Test product", Category="Test", Description="Test", ImageFile="", Price=20, Summary="Test"};
            var productsResult = ((ObjectResult)_catalogController.CreateProduct(product).Result.Result);
            var createdProduct = productsResult.Value as ProductDto;
            Assert.IsTrue(createdProduct.Id > 0);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void GetProduct()
        {
            var productsResult = ((ObjectResult)_catalogController.GetProductById(1).Result.Result);
            var createdProduct = productsResult.Value as ProductDto;
            Assert.IsTrue(createdProduct.Id == 1);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public async Task UpdateProduct()
        {
            ProductDto productDto = new ProductDto { Id = 1, Category = "UpdateTest", Description = "Test", Name = "Test", Price = 20, Summary = "Test" };
            await _catalogController.UpdateProduct(productDto);

            var productsResult = ((ObjectResult)_catalogController.GetProductById(1).Result.Result);
            var listedProduct = productsResult.Value as ProductDto;

            Assert.IsTrue(listedProduct.Name == "Test");
        }

        [TestCategory("Integration")]
        [TestMethod]
        public async Task DeleteProduct()
        {
            ProductDto productDto = new ProductDto { Id = 2, Category = "UpdateTest", Description = "Test", Name = "Test", Price = 20, Summary = "Test" };
            await _catalogController.DeleteProductById(productDto.Id);
            var productsResult = _catalogController.GetProductById(2).Result.Result;
            Assert.IsTrue(productsResult is NotFoundResult);
        }
    }
}
