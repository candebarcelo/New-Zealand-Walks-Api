using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.JsonWebTokens;

namespace NZWalks.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public string JwtToken;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterRequestDto registerRequest)
        {
            try
            {
                registerRequest.Roles = new string[] { "Reader", "Writer" };

                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7273/api/auth/register"),
                    Content = new StringContent(JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return View("SignIn");
                }
            }
            catch (Exception error)
            {
                // log error
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginRequestDto loginRequest)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7273/api/auth/login"),
                    Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDto>();

                if (response is not null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Set to true if using HTTPS
                    };
                    HttpContext.Response.Cookies.Append("jwtToken", response.JwtToken, cookieOptions);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception error)
            {
                // log error
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
