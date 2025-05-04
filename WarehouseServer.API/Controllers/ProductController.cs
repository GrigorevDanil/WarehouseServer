using Microsoft.AspNetCore.Mvc;
using WarehouseServer.API.Contracts.Product;
using WarehouseServer.API.Contracts.ProductResource;
using WarehouseServer.API.Contracts.Resource;
using WarehouseServer.Application.Interfaces;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IResourceService resourceService;
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IProductService productService, IResourceService resourceService, IUnitOfWork unitOfWork)
        {
            this.productService = productService;
            this.resourceService = resourceService;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получает список всех товаров без деталей
        /// </summary>
        [HttpGet("List")]
        public async Task<ActionResult<ProductResponse[]>> GetProducts()
        {
            var result = await productService.GetProducts();

            if (result.IsFailure)
                return NotFound(result.Error);

            var products = result.Value;
            var response = products.Select(pro => new ProductResponse(pro.Id, pro.Title, pro.Cost, []));

            return Ok(response);

        }

        /// <summary>
        /// Получает список всех товаров с ресурсами
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ProductResponse[]>> GetProductsWithResources()
        {
            var result = await productService.GetProductsWithResources();

            if (result.IsFailure)
                return NotFound(result.Error);

            var products = result.Value;

            var response = products.Select(pro => new ProductResponse(
                       pro.Id,
                       pro.Title,
                       pro.Cost,
                       pro.ProductResources.Select(pr => new ProductResourceResponse(
                           new ResourceResponse(pr.Resource.Id, pr.Resource.Title, pr.Resource.Unit),
                           pr.Quantity
                       )).ToArray()
                   ));

            return Ok(response);

        }

        /// <summary>
        /// Получает товар по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
        {
            var result = await productService.GetProductByIdWithResources(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var product = result.Value;
            var response = new ProductResponse(
                       product.Id,
                       product.Title,
                       product.Cost,
                       product.ProductResources.Select(pr => new ProductResourceResponse(
                           new ResourceResponse(pr.Resource.Id, pr.Resource.Title, pr.Resource.Unit),
                           pr.Quantity
                       )).ToArray()
                   );

            return Ok(response);
        }

        /// <summary>
        /// Добавляет товар
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> AddProduct([FromBody] ProductRequest request)
        {
            var result = Product.Create(request.Title, request.Cost, [], []);

            if (result.IsFailure)
                return BadRequest(result.Error);

            var product = result.Value;

            var productId = await productService.AddProduct(product);

            await unitOfWork.SaveChanges();

            return Ok(productId);
        }

        /// <summary>
        /// Обновляет товар по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateProduct(Guid id, [FromBody] ProductRequest request)
        {
            var result = await productService.GetProductById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var product = result.Value;

            product.UpdateInfo(request.Title, request.Cost);

            var productId = productService.SaveProduct(product);

            await unitOfWork.SaveChanges();

            return Ok(productId);

        }

        /// <summary>
        /// Удаляет товар по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteProduct(Guid id)
        {
            var result = await productService.GetProductById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var product = result.Value;

            var deletedProductId = productService.DeleteProduct(product);

            await unitOfWork.SaveChanges();

            return Ok(deletedProductId);

        }

        /// <summary>
        /// Добавляет ресурс к товару
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        [HttpPost("{id:guid}/Resources")]
        public async Task<ActionResult<Guid>> AddResourceToProduct(Guid id, [FromBody] ProductResourceRequest request)
        {
            var productResult = await productService.GetProductByIdWithResources(id);

            if (productResult.IsFailure)
                return NotFound(productResult.Error);
            var product = productResult.Value;

            var resourceResult = await resourceService.GetResourceById(request.ResourceId);

            if (resourceResult.IsFailure)
                return NotFound(resourceResult.Error);

            var resource = resourceResult.Value;

            var isDublicate = product.ProductResources.Any(pr => pr.ResourceId == resource.Id);

            if (isDublicate) return BadRequest("Запись уже существует");

            var productResourceResult = ProductResource.Create(product, resource, request.Quantity);

            if (productResourceResult.IsFailure)
                return BadRequest(productResourceResult.Error);

            var productResource = productResourceResult.Value;

            product.AddProductResource(productResource);

            productService.SaveProduct(product);

            await unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Обновляет количество ресурса в товаре
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <param name="resourceId">Идентификатор ресурса</param>
        [HttpPut("{id:guid}/Resources/{resourceId:guid}")]
        public async Task<ActionResult<Guid>> UpdateResourceToProduct(Guid id, Guid resourceId, [FromBody] UpdateProductResourceRequest request)
        {
            var productResult = await productService.GetProductByIdWithResources(id);

            if (productResult.IsFailure)
                return NotFound(productResult.Error);
            var product = productResult.Value;

            var resourceResult = await resourceService.GetResourceById(resourceId);

            if (resourceResult.IsFailure)
                return NotFound(resourceResult.Error);

            var resource = resourceResult.Value;

            var productResource = product.ProductResources.FirstOrDefault(pr => pr.ResourceId == resource.Id);

            if (productResource is null)
                return NotFound("Запись не найдена");

            productResource.UpdateInfo(request.Quantity);

            await unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Удаляет связь между товаром и ресурсом
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <param name="resourceId">Идентификатор ресурса</param>
        [HttpDelete("{id:guid}/Resources/{resourceId:guid}")]
        public async Task<ActionResult<Guid>> DeleteResourceToProduct(Guid id, Guid resourceId)
        {
            var productResult = await productService.GetProductByIdWithResources(id);

            if (productResult.IsFailure)
                return NotFound(productResult.Error);
            var product = productResult.Value;

            var resourceResult = await resourceService.GetResourceById(resourceId);

            if (resourceResult.IsFailure)
                return NotFound(resourceResult.Error);

            var resource = resourceResult.Value;

            var productResource = product.ProductResources.FirstOrDefault(pr => pr.ResourceId == resource.Id);

            if (productResource is null)
                return NotFound("Запись не найдена");

            product.DeleteProductResource(productResource);

            await unitOfWork.SaveChanges();

            return Ok();
        }


    }
}
