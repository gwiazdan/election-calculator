using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using election_calculator_backend.Models;

namespace election_calculator_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipalityController : ControllerBase
    {
        private readonly string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Municipalities.json");

        [HttpGet("load")]
        public async Task<ActionResult<List<Municipality>>> LoadJson()
        {
            if (!System.IO.File.Exists(jsonFilePath))
            {
                return NotFound("JSON file not found.");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(jsonFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var dataModel = JsonSerializer.Deserialize<List<Municipality>>(jsonData, options);

            return Ok(dataModel);
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("API działa");
    }
}