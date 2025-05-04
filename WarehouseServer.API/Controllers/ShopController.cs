using Microsoft.AspNetCore.Mvc;
using WarehouseServer.API.Contracts.Distance;
using WarehouseServer.API.Contracts.Shop;
using WarehouseServer.API.Contracts.TransportMethod;
using WarehouseServer.API.Contracts.Warehouse;
using WarehouseServer.Application.Interfaces;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService shopService;
        private readonly IWarehouseService warehouseService;
        private readonly IProductService productService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITransportMethodService transportMethodService;
        public ShopController(IShopService shopService, IWarehouseService warehouseService, IProductService productService, IUnitOfWork unitOfWork, ITransportMethodService transportMethodService)
        {
            this.shopService = shopService;
            this.warehouseService = warehouseService;
            this.productService = productService;
            this.unitOfWork = unitOfWork;
            this.transportMethodService = transportMethodService;
        }

        /// <summary>
        /// Получает список всех магазинов без деталей
        /// </summary>
        [HttpGet("List")]
        public async Task<ActionResult<ShopResponse[]>> GetShops()
        {
            var result = await shopService.GetShops();

            if (result.IsFailure)
                return NotFound(result.Error);

            var shops = result.Value;
            var response = shops.Select(s => new ShopResponse(s.Id, s.Title, []));

            return Ok(response);

        }

        /// <summary>
        /// Получает список всех магазинов с дистанциями
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ShopResponse[]>> GetShopsWithDistances()
        {
            var result = await shopService.GetShopsWithDistances();

            if (result.IsFailure)
                return NotFound(result.Error);

            var shops = result.Value;

            var response = shops.Select(s => new ShopResponse(
                       s.Id,
                       s.Title,
                       s.Distances.Select(d => new DistanceResponse(
                           new WarehouseResponse(d.Warehouse.Id, d.Warehouse.Title, []),
                           d.Length
                       )).ToArray()
                   ));

            return Ok(response);

        }

        /// <summary>
        /// Получает магазин по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор магазина</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopResponse>> GetShopById(Guid id)
        {
            var result = await shopService.GetShopById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var shop = result.Value;
            var response = new ShopResponse(
                       shop.Id,
                       shop.Title,
                       shop.Distances.Select(d => new DistanceResponse(
                           new WarehouseResponse(d.Warehouse.Id, d.Warehouse.Title, []),
                           d.Length
                       )).ToArray()
                   );

            return Ok(response);
        }

        /// <summary>
        /// Добавляет магазин
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> AddShop([FromBody] ShopRequest request)
        {
            var result = Shop.Create(request.Title, []);

            if (result.IsFailure)
                return BadRequest(result.Error);

            var shop = result.Value;

            var shopId = await shopService.AddShop(shop);

            await unitOfWork.SaveChanges();

            return Ok(shopId);
        }

        /// <summary>
        /// Обновляет магазин по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор магазина</param>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateShop(Guid id, [FromBody] ShopRequest request)
        {
            var result = await shopService.GetShopById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var shop = result.Value;

            shop.UpdateInfo(request.Title);

            var shopId = shopService.SaveShop(shop);

            await unitOfWork.SaveChanges();

            return Ok(shopId);

        }

        /// <summary>
        /// Удаляет магазин по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор магазина</param>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteShop(Guid id)
        {
            var result = await shopService.GetShopById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var shop = result.Value;

            var deletedShopId = shopService.DeleteShop(shop);

            await unitOfWork.SaveChanges();

            return Ok(deletedShopId);

        }

        /// <summary>
        /// Добавляет дистанцию к магазину
        /// </summary>
        /// <param name="id">Идентификатор магазина</param>
        [HttpPost("{id:guid}/Distances")]
        public async Task<ActionResult<Guid>> AddDistanceToShop(Guid id, [FromBody] DistanceRequest request)
        {
            var shopResult = await shopService.GetShopByIdWithDistances(id);

            if (shopResult.IsFailure)
                return NotFound(shopResult.Error);

            var shop = shopResult.Value;

            var warehouseResult = await warehouseService.GetWarehouseById(request.WarehouseId);

            if (warehouseResult.IsFailure)
                return NotFound(warehouseResult.Error);

            var warehouse = warehouseResult.Value;

            var isDublicate = shop.Distances.Any(d => d.WarehouseId == warehouse.Id);

            if (isDublicate) return BadRequest("Запись уже существует");

            var distanceResult = Distance.Create(shop, warehouse, request.Length);

            if (distanceResult.IsFailure)
                return BadRequest(distanceResult.Error);

            var distance = distanceResult.Value;

            shop.AddDistance(distance);

            shopService.SaveShop(shop);

            await unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Обновляет длину дистанции между магазином и складом
        /// </summary>
        /// <param name="id">Идентификатор магазина</param>
        /// <param name="warehouseId">Идентификатор склада</param>
        [HttpPut("{id:guid}/Distances/{warehouseId:guid}")]
        public async Task<ActionResult<Guid>> UpdateDistanceToShop(Guid id, Guid warehouseId, [FromBody] UpdateDistanceRequest request)
        {
            var shopResult = await shopService.GetShopByIdWithDistances(id);

            if (shopResult.IsFailure)
                return NotFound(shopResult.Error);

            var shop = shopResult.Value;

            var warehouseResult = await warehouseService.GetWarehouseById(warehouseId);

            if (warehouseResult.IsFailure)
                return NotFound(warehouseResult.Error);

            var warehouse = warehouseResult.Value;

            var distance = shop.Distances.FirstOrDefault(d => d.WarehouseId == warehouse.Id);

            if (distance is null)
                return NotFound("Запись не найдена");

            distance.UpdateInfo(request.Length);

            await unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Удаляет дистанцию между магазином и складом
        /// </summary>
        /// <param name="id">Идентификатор магазин</param>
        /// <param name="warehouseId">Идентификатор склада</param>
        [HttpDelete("{id:guid}/Distances/{warehouseId:guid}")]
        public async Task<ActionResult<Guid>> DeleteDistanceToShop(Guid id, Guid warehouseId)
        {
            var shopResult = await shopService.GetShopByIdWithDistances(id);

            if (shopResult.IsFailure)
                return NotFound(shopResult.Error);

            var shop = shopResult.Value;

            var warehouseResult = await warehouseService.GetWarehouseById(warehouseId);

            if (warehouseResult.IsFailure)
                return NotFound(warehouseResult.Error);

            var warehouse = warehouseResult.Value;

            var distance = shop.Distances.FirstOrDefault(d => d.WarehouseId == warehouse.Id);

            if (distance is null)
                return NotFound("Запись не найдена");

            shop.DeleteDistance(distance);

            await unitOfWork.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Расчет оптимального плана поставок (Транспортная задача)
        /// </summary>
        [HttpPost("TransportMethod")]
        public async Task<ActionResult<double>> CalculationDelivery([FromBody] TransportMethodRequest request)
        {
            if (request.Warehouses.Length < 2)
                return BadRequest("Количество складов должно быть больше 2-х");

            if (request.Shops.Length != 2)
                return BadRequest("Количество магазинов должно быть больше 2-х");

            var shopsResult = await shopService.GetShopsByIdsWithDistances(request.Shops);
            if (shopsResult.IsFailure)
                return NotFound(shopsResult.Error);
            var shops = shopsResult.Value;

            var warehousesResult = await warehouseService.GetWarehousesByIdsWithProducts(request.Warehouses);
            if (warehousesResult.IsFailure)
                return NotFound(warehousesResult.Error);
            var warehouses = warehousesResult.Value;

            var productsResult = await productService.GetProductsByIds(request.Products);
            if (productsResult.IsFailure)
                return NotFound(productsResult.Error);
            var products = productsResult.Value.ToDictionary(p => p.Id);

            double[] demands = request.Demands;
            double[] suppliers = new double[request.Warehouses.Length];

            for (int i = 0; i < request.Warehouses.Length; i++)
            {
                var warehouse = warehouses.FirstOrDefault(w => w.Id == request.Warehouses[i]);
                var productWarehouse = warehouse?.ProductWarehouses.FirstOrDefault(pw => pw.ProductId == request.Products[i]);
                suppliers[i] = productWarehouse?.Quantity ?? 0;
            }

            // Матрица стоимостей (цена продукта * расстояние)
            double[,] costMatrix = new double[request.Demands.Length, request.Warehouses.Length];

            for (int shopIdx = 0; shopIdx < shops.Count; shopIdx++)
            {
                var shop = shops[shopIdx];

                for (int warehouseIdx = 0; warehouseIdx < warehouses.Count; warehouseIdx++)
                {
                    var warehouse = warehouses[warehouseIdx];
                    var distance = shop.Distances.FirstOrDefault(d => d.WarehouseId == warehouse.Id);

                    if (distance != null && products.TryGetValue(request.Products[warehouseIdx], out var product))
                    {
                        costMatrix[shopIdx, warehouseIdx] = product.Cost * distance.Length;
                    }
                    else
                    {
                        return BadRequest("Между складом и магазином не указана длина");
                    }
                }
            }

            double totalCost = transportMethodService.Calculate(costMatrix, suppliers, demands);

            return Ok(totalCost);
        }
    }
}
