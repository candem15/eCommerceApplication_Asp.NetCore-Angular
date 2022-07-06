using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Application.RequestParameters.Pagination;
using eCommerceAPI.Application.Services;
using eCommerceAPI.Application.ViewModels.Products;
using eCommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace eCommerceAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnviroment; // Api üzerindeki enviroment üzerindeki static fileslara kadar erişebilmemizi sağlayan bir servistir.
        private readonly IFileService _fileService;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnviroment, IFileService fileService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnviroment = webHostEnviroment;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            var totalProductsCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size);

            return Ok(
                new
                {
                    totalProductsCount,
                    products
                });
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_CreateProduct model)
        {
            Product product = new Product
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price,
            };
            await _productWriteRepository.AddAsync(product);
            await _productWriteRepository.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id, false));
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_UpdateProduct model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Stock = model.Stock;
            product.Price = model.Price;
            await _productWriteRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            await _fileService.UploadAsync("product-images", Request.Form.Files);
            return Ok();
        }
    }
}
