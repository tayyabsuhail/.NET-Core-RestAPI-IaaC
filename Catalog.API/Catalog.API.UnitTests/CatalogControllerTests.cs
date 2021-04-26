using AutoMapper;
using Catalog.API.Controllers;
using Catalog.API.Data;
using Catalog.API.DTO;
using Catalog.API.Entities;
using Catalog.API.Profiles;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.UnitTests
{
    [TestClass]
    public class CatalogControllerTest
    {
        private Mock<IProductRepository> repository;
        private CatalogController catalogController;
        private IMapper _mapper;
        private Product product;
        private ProductDto productDto;

        [TestInitialize]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CatalogProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            repository = new Mock<IProductRepository>();
            List<Product> products = (List<Product>) CatalogContextSeed.GetPreConfiguredProducts();
            productDto = new ProductDto { Id=1 , Name= "Test Product" };
            product = _mapper.Map<Product>(productDto); 
            repository.Setup(m => m.GetAllAsync()).ReturnsAsync(products);
            repository.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(product);
            repository.Setup(m => m.UpdateAsync(It.IsAny<Product>())).Returns
                (Task.FromResult(1));
            repository.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(product);
            repository.Setup(m => m.DeleteAsync(It.IsAny<Product>())).Returns(Task.FromResult(1));
            catalogController = new CatalogController(repository.Object, _mapper);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GetAllProducts()
        {
            var productsResult = ((ObjectResult)catalogController.GetProducts().Result.Result);
            var productsList = productsResult.Value as List<ProductDto>;
            Assert.IsTrue(productsList.Count == 6);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GetProductByIdNotFound()
        {
            var productsResult = catalogController.GetProductById(-1).Result.Result;
            Assert.IsTrue(productsResult is NotFoundResult);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GetProductByIdOk()
        {
            var productsResult = catalogController.GetProductById(1).Result.Result;
            Assert.IsTrue(productsResult is OkObjectResult);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void UpdateProductNotFound()
        {
            var productsResult = catalogController.UpdateProduct(new ProductDto { Id=int.MaxValue}).Result;
            Assert.IsTrue(productsResult is NotFoundResult);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void UpdateProductNoContent()
        {
            var productsResult = catalogController.UpdateProduct(productDto).Result;
            Assert.IsTrue(productsResult is NoContentResult);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void CreateProductOK()
        {
            var productsResult = catalogController.CreateProduct(productDto).Result.Result;
            Assert.IsTrue(productsResult is OkObjectResult);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void DeleteProductNoContent()
        {
            var productsResult = catalogController.DeleteProductById(1).Result;
            Assert.IsTrue(productsResult is NoContentResult);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void DeleteProductNotFoundt()
        {
            var productsResult = catalogController.DeleteProductById(-1).Result;
            Assert.IsTrue(productsResult is NotFoundResult);
        }

    }
}
