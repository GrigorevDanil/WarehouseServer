using Microsoft.AspNetCore.Mvc;
using WarehouseServer.API.Contracts.Resource;
using WarehouseServer.Application.Interfaces;
using WarehouseServer.Domain.Entities;
using WarehouseServer.Domain.Interfaces.Services;

namespace WarehouseServer.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService resourceService;
        private readonly IUnitOfWork unitOfWork;

        public ResourceController(IResourceService resourceService, IUnitOfWork unitOfWork)
        {
            this.resourceService = resourceService;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получает список всех ресурсов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ResourceResponse[]>> GetResources()
        {
            var result = await resourceService.GetResources();

            if (result.IsFailure)
                return NotFound(result.Error);

            var resources = result.Value;
            var response = resources.Select(res => new ResourceResponse(res.Id, res.Title, res.Unit));

            return Ok(response);
        }

        /// <summary>
        /// Получает ресурс по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор ресурса</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceResponse>> GetResourceById(Guid id)
        {
            var result = await resourceService.GetResourceById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var resource = result.Value;
            var response = new ResourceResponse(resource.Id, resource.Title, resource.Unit);

            return Ok(response);
        }

        /// <summary>
        /// Добавляет ресурс
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> AddResource([FromBody] ResourceRequest request)
        {
            var result = Resource.Create(request.Title, request.Unit, []);

            if (result.IsFailure)
                return BadRequest(result.Error);

            var resource = result.Value;

            var resourceId = await resourceService.AddResource(resource);

            await unitOfWork.SaveChanges();

            return Ok(resourceId);
        }

        /// <summary>
        /// Обновляет ресурс по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор ресурса</param>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateResource(Guid id, [FromBody] ResourceRequest request)
        {
            var result = await resourceService.GetResourceById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var resource = result.Value;

            resource.UpdateInfo(request.Title, request.Unit);

            var resourceId = resourceService.SaveResource(resource);

            await unitOfWork.SaveChanges();

            return Ok(resourceId);
        }

        /// <summary>
        /// Удаляет ресурс по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор ресурса</param>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteResource(Guid id)
        {
            var result = await resourceService.GetResourceById(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            var resource = result.Value;

            var deletedResourceId = resourceService.DeleteResource(resource);

            await unitOfWork.SaveChanges();

            return Ok(deletedResourceId);
        }
    }
}