using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Application.RequestParameters.Pagination;
using eCommerceAPI.Application.ViewModels.Products;
using eCommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IStorageService _storageService;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnviroment, IStorageService storageService, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnviroment = webHostEnviroment;
            _storageService = storageService;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
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
        public async Task<IActionResult> Upload(string productId)
        {
            var datas = await _storageService.UploadAsync("product-images", Request.Form.Files);
            Product product = await _productReadRepository.GetByIdAsync(productId);
            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(x => new ProductImageFile()
            {
                FileName = x.fileName,
                Path = x.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());
            await _productImageFileWriteRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return Ok(product.ProductImageFiles.Select(x => new
            {
                x.FileName,
                x.Path,
                x.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            await _productImageFileWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
