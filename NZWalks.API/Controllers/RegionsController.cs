using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // endpoint: https://localhost:7273/api/regions
    [Route("api/[controller]")]
    [ApiController]
    // Authorize is added to make sure only authenticated users can access the api.
    // but we remove it so as to add role-based authorization for each method.
    // [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        // inject the dbContext we created
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // GET ALL REGIONS
        // GET: https://localhost:7273/api/regions
        [HttpGet]
        // only authorize users with the Reader role. if multiple roles should be allowed,
        // separate by commas inside the string like "Reader,Writer".
        [Authorize(Roles = "Reader")]
        // Task for async functions. if not async, it's just the IActionResult.
        public async Task<IActionResult> GetAll()
        {
            // get data from database (domain models)
            var regionsDomain = await regionRepository.GetAllAsync();

            // map domain models to DTOs (convert) and
            // return DTOs. this is the best practice, to ensure separation of concerns,
            // security, performance and easier versioning. because a domain model is the one to
            // communicate between the app and the db, and the dto is the one returned to
            // the client, and it may have less properties than the one from the db.
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }


        // GET SINGLE REGION BY ID
        // GET: https://localhost:7273/api/regions/{id}
        [HttpGet]
        // in order to get a param from the url, use the [Route] here and the [FromRoute]
        // in front of each param. the property you put in the route should match the
        // param in the function. including the type is optional, but it will do a type
        // validation and send a 404 if the received type doesn't match the one in the code.
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                // 404
                return NotFound();
            }

            // 200
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        // POST TO CREATE NEW REGION
        // POST: https://localhost:7273/api/regions
        [HttpPost]
        // ValidateModel validates that the data received in the body is valid, 
        // meeting all the requirements we set in the addRegionRequestDto.cs file.
        // It's using the function we overrode in ValidateModelAttributes.cs, and
        // checking if the ModelState.IsValid. This approach helps to keep the
        // controller code clean and DRY. 
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        // the params are in the body, not url.
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // convert DTO to domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // use domain model to create region in db
            await regionRepository.CreateAsync(regionDomainModel);

            // map domain model back to DTO in order to send to client again
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            // 201 created. params: it'll search it by Id and give its location as part of
            // the headers, + the value of the Dto of the item created.
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


        // UPDATE REGION
        // PUT: https://localhost:7273/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // another way of validating the body instead of using the [ValidateModel] decorator:
            // use ModelState to check if the data received in the body is valid, 
            // meeting all the requirements we set in the addRegionRequestDto.cs file.
            if (ModelState.IsValid)
            {
                // map DTO to domain model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // check if region exists
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                // convert domain model to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                // another way of doing it without autoMapper: 
                // var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //}

                return Ok(regionDto);
            }
            else
            {
                return BadRequest(ModelState); 
            }
        }

        // DELETE REGION
        // DELETE: https://localhost:7273/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // return deleted region back
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
