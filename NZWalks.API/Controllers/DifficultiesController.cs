using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultiesController : ControllerBase
    {
        private readonly IDifficultyRepository difficultyRepository;
        private readonly IMapper mapper;

        public DifficultiesController(
            IDifficultyRepository difficultyRepository,
            IMapper mapper)
        {
            this.difficultyRepository = difficultyRepository;
            this.mapper = mapper;
        }

        // GET ALL 
        // GET: https://localhost:7273/api/difficulties
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            var difficultiesDomain = await difficultyRepository.GetAllAsync();

            return Ok(mapper.Map<List<DifficultyDto>>(difficultiesDomain));
        }
    }
}
