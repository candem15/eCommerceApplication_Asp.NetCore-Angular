using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Features.Commands.Product.CreateProduct;
using eCommerceAPI.Application.Features.Commands.Product.DeleteProduct;
using eCommerceAPI.Application.Features.Commands.Product.UpdateProduct;
using eCommerceAPI.Application.Features.Commands.ProductImage.UploadProductImage;
using eCommerceAPI.Application.Features.Queries.Product.GetAllProducts;
using eCommerceAPI.Application.Features.Queries.Product.GetProductById;
using eCommerceAPI.Application.Features.Queries.ProductImage.GetProductImages;
using eCommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace eCommerceAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ProductsController : ControllerBase
    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnviroment; // Api üzerindeki enviroment üzerindeki static fileslara kadar erişebilmemizi sağlayan bir servistir.
        private readonly IStorageService _storageService;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IMediator _mediatR;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnviroment, IStorageService storageService, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IMediator mediatR)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnviroment = webHostEnviroment;
            _storageService = storageService;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _mediatR = mediatR;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse getAllProductsQueryResponse = await _mediatR.Send(getAllProductsQueryRequest);
            return Ok(getAllProductsQueryResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse createProductCommandResponse = await _mediatR.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            return Ok(await _mediatR.Send(getProductByIdQueryRequest));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> EditProduct([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            await _mediatR.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest deleteProductCommandRequest)
        {
            await _mediatR.Send(deleteProductCommandRequest);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediatR.Send(uploadProductImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            List<GetProductImagesQueryResponse> GetProductImagesQueryResponse = await _mediatR.Send(getProductImagesQueryRequest);
            return Ok(GetProductImagesQueryResponse);
        }

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteImage([FromQuery] DeleteProductCommandRequest deleteProductCommandRequest)
        {
            await _mediatR.Send(deleteProductCommandRequest);
            return Ok();
        }
    }
}
