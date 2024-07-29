using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text.Json;
using System.Text;

namespace NZWalks.UI.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public WalksController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<WalkDto> response = new List<WalkDto>();

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7273/api/walks");
                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<WalkDto>>());
            }
            catch (Exception error)
            {
                // log error
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Difficulties = new List<DifficultyDto>();
            ViewBag.Regions = new List<RegionDto>();

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7273/api/difficulties");
                httpResponseMessage.EnsureSuccessStatusCode();

                ViewBag.Difficulties.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DifficultyDto>>());
            }
            catch (Exception error)
            {
                // log error
            }

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7273/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();

                ViewBag.Regions.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception error)
            {
                // log error
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddWalkViewModel model)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7273/api/walks"),
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<WalkDto>();

                if (response is not null)
                {
                    return RedirectToAction("Index", "Walks");
                }
            }
            catch (Exception error)
            {
                // log error
            }

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<WalkDto>($"https://localhost:7273/api/walks/{id.ToString()}");

            ViewBag.Difficulties = new List<DifficultyDto>();
            ViewBag.Regions = new List<RegionDto>();

            try
            {
                var httpResponseMessage = await client.GetAsync("https://localhost:7273/api/difficulties");
                httpResponseMessage.EnsureSuccessStatusCode();

                ViewBag.Difficulties.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DifficultyDto>>());
            }
            catch (Exception error)
            {
                // log error
            }

            try
            {
                var httpResponseMessage = await client.GetAsync("https://localhost:7273/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();

                ViewBag.Regions.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception error)
            {
                // log error
            }

            if (response is not null)
            {
                return View(response);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WalkDto request)
        {
            UpdateWalkRequestDto updateWalkRequestDto = new UpdateWalkRequestDto()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                LengthInKm = request.LengthInKm,
                WalkImageUrl = request.WalkImageUrl,
                DifficultyId = request.Difficulty.Id,
                RegionId = request.Region.Id
            };

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7273/api/walks/{updateWalkRequestDto.Id}"),
                    Content = new StringContent(JsonSerializer.Serialize(updateWalkRequestDto), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<WalkDto>();

                if (response is not null)
                {
                    return RedirectToAction("Index", "Walks");
                }
            }
            catch (Exception error)
            {
                // log error
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(WalkDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7273/api/walks/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Walks");
            }
            catch (Exception error)
            {
                // console.log
            }

            return View("Edit");
        }
    }
}
