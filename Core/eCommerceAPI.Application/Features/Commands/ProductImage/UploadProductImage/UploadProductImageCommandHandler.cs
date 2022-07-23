using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Domain.Entities;
using MediatR;

namespace eCommerceAPI.Application.Features.Commands.ProductImage.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IStorageService _storageService;
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _storageService = storageService;
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }
        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var datas = await _storageService.UploadAsync("product-images", request.Files);

            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.productId);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(x => new ProductImageFile()
            {
                FileName = x.fileName,
                Path = x.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Domain.Entities.Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveChangesAsync();

            return new();
        }
    }
}
