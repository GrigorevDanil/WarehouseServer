using Microsoft.AspNetCore.Mvc;
using WarehouseServer.API.Contracts.Product;
using WarehouseServer.API.Contracts.ProductWarehouse;
using WarehouseServer.API.Contracts.Warehouse;
using WarehouseServer.Application.Interfaces;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService warehouseService;
        private readonly IProductService productService;
        private readonly IUnitOfWork unitOfWork;
        public WarehouseController(IWarehouseService warehouseService, IProductService productService, IUnitOfWork unitOfWork)
        {
            this.warehouseService = warehouseService;
            this.productService = productService;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получает список всех складов без деталей
        /// </summary>
        [HttpGet("List")]
        public async Task<ActionResult<WarehouseResponse[]>> GetWarehouses()
        {
            var result = await warehouseService.GetWarehouses();

            if (result.IsFailure)
                return NotFound(result.Error);

            var warehouses = result.Value;
            var response = warehouses.Select(w => new WarehouseResponse(w.Id, w.Title, []));

            return Ok(response);

        }

        /// <summary>
        /// Получает список всех складов с товарами
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<WarehouseResponse[]>> GetWarehousesWithProducts()
        {
            var result = await warehouseService.GetWarehousesWithProducts();

            if (result.IsFailure)
                return NotFound(result.Error);

            var warehouses = result.Value;

            var response = warehouses.Select(w => new WarehouseResponse(
                       w.Id,
                       w.Title,
                       w.ProductWarehouses.Select(pw => new ProductWarehouseResponse(
                           new ProductResponse(pw.ProductId, pw.Product.Title, pw.Product.Cost, []),
                           pw.Quantity
                       )).ToArray()
                   ));

            return Ok(response);

        }

        /// <summary>
        /// Получает склад по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseResponse>> GetWarehouseById(Guid id)
        {
            var result = await warehouseService.GetWarehouseByIdWithProducts(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var warehouse = result.Value;

            var response = new WarehouseResponse(
                       warehouse.Id,
                       warehouse.Title,
                       warehouse.ProductWarehouses.Select(pw => new ProductWarehouseResponse(
                           new ProductResponse(pw.ProductId, pw.Product.Title, pw.Product.Cost, []),
                           pw.Quantity
                       )).ToArray()
                   );

            return Ok(response);
        }

        /// <summary>
        /// Добавляет склад
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> AddWarehouse([FromBody] WarehouseRequest request)
        {
            var result = Warehouse.Create(request.Title, [], []);

            if (result.IsFailure)
                return BadRequest(result.Error);

            var warehouse = result.Value;

            var warehouseId = await warehouseService.AddWarehouse(warehouse);

            await unitOfWork.SaveChanges();

            return Ok(warehouseId);
        }

        /// <summary>
        /// Обновляет склад по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateWarehouse(Guid id, [FromBody] WarehouseRequest request)
        {
            var result = await warehouseService.GetWarehouseById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var warehouse = result.Value;

            warehouse.UpdateInfo(request.Title);

            var warehouseId = warehouseService.SaveWarehouse(warehouse);

            await unitOfWork.SaveChanges();

            return Ok(warehouseId);

        }

        /// <summary>
        /// Удаляет склад по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteWarehouse(Guid id)
        {
            var result = await warehouseService.GetWarehouseById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var warehouse = result.Value;

            var deletedWarehouseId = warehouseService.DeleteWarehouse(warehouse);

            await unitOfWork.SaveChanges();

            return Ok(deletedWarehouseId);

        }

        /// <summary>
        /// Добавляет товар на склад
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        [HttpPost("{id:guid}/Products")]
        public async Task<ActionResult<Guid>> AddProductToWarehouse(Guid id, [FromBody] ProductWarehouseRequest request)
        {
            var warehouseResult = await warehouseService.GetWarehouseByIdWithProducts(id);

            if (warehouseResult.IsFailure)
                return NotFound(warehouseResult.Error);

            var warehouse = warehouseResult.Value;

            var productResult = await productService.GetProductById(request.ProductId);

            if (productResult.IsFailure)
                return NotFound(productResult.Error);

            var product = productResult.Value;

            var isDublicate = warehouse.ProductWarehouses.Any(pr => pr.ProductId == product.Id);

            if (isDublicate) return BadRequest("Запись уже существует");

            var productWarehouseResult = ProductWarehouse.Create(product, warehouse, request.Quantity);

            if (productWarehouseResult.IsFailure)
                return BadRequest(productWarehouseResult.Error);

            var productWarehouse = productWarehouseResult.Value;

            warehouse.AddProductWarehouse(productWarehouse);

            warehouseService.SaveWarehouse(warehouse);

            await unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Обновляет количество товара на складе
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        /// <param name="productId">Идентификатор товара</param>
        [HttpPut("{id:guid}/Products/{productId:guid}")]
        public async Task<ActionResult<Guid>> UpdateProductToWarehouse(Guid id, Guid productId, [FromBody] UpdateProductWarehouseRequest request)
        {
            var warehouseResult = await warehouseService.GetWarehouseByIdWithProducts(id);

            if (warehouseResult.IsFailure)
                return NotFound(warehouseResult.Error);

            var warehouse = warehouseResult.Value;

            var productResult = await productService.GetProductById(productId);

            if (productResult.IsFailure)
                return NotFound(productResult.Error);

            var product = productResult.Value;

            var productWarehouse = warehouse.ProductWarehouses.FirstOrDefault(pw => pw.ProductId == product.Id);

            if (productWarehouse is null)
                return NotFound("Запись не найдена");

            productWarehouse.UpdateInfo(request.Quantity);

            await unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Удаляет связь между товаром и складом
        /// </summary>
        /// <param name="id">Идентификатор склада</param>
        /// <param name="productId">Идентификатор товара</param>
        [HttpDelete("{id:guid}/Products/{productId:guid}")]
        public async Task<ActionResult<Guid>> DeleteProductToWarehouse(Guid id, Guid productId)
        {
            var warehouseResult = await warehouseService.GetWarehouseByIdWithProducts(id);

            if (warehouseResult.IsFailure)
                return NotFound(warehouseResult.Error);

            var warehouse = warehouseResult.Value;

            var productResult = await productService.GetProductById(productId);

            if (productResult.IsFailure)
                return NotFound(productResult.Error);

            var product = productResult.Value;

            var productWarehouse = warehouse.ProductWarehouses.FirstOrDefault(pw => pw.ProductId == product.Id);

            if (productWarehouse is null)
                return NotFound("Запись не найдена");

            warehouse.DeleteProductWarehouse(productWarehouse);

            await unitOfWork.SaveChanges();

            return Ok();
        }
    }
}
