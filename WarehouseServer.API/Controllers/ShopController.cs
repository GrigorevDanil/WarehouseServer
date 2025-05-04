using Microsoft.AspNetCore.Mvc;
using WarehouseServer.API.Contracts.Distance;
using WarehouseServer.API.Contracts.Shop;
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
        private readonly IUnitOfWork unitOfWork;
        public ShopController(IShopService shopService, IWarehouseService warehouseService, IUnitOfWork unitOfWork)
        {
            this.shopService = shopService;
            this.warehouseService = warehouseService;
            this.unitOfWork = unitOfWork;
        }

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
    }
}
