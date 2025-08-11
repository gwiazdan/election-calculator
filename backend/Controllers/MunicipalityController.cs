using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using election_calculator_backend.DTOs;
using election_calculator_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace election_calculator_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipalityController : ControllerBase
    {
        private readonly string jsonFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "Municipalities.json"
        );

        [HttpGet("load")]
        public async Task<ActionResult<List<MunicipalityDto>>> LoadJson()
        {
            if (!System.IO.File.Exists(jsonFilePath))
            {
                return NotFound("JSON file not found.");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(jsonFilePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dataModel = JsonSerializer.Deserialize<List<Municipality>>(jsonData, options);

            if (dataModel == null)
                return BadRequest("Deserialization failed or data is null.");

            var result = dataModel
				.Select(m => new MunicipalityDto
				{
					Id = m.Id,
					Name = m.Name,
					TotalVotes = m.Total,
					CountyID = m.CountyID,
					Votes = new VotesDto
					{
						LEW = m.LEW,
						KKP = m.KKP,
						Pl2050 = m.Pl2050,
						Konfederacja = m.Konf,
						PIS = m.PIS,
						KO = m.KO,
						Razem = m.Razem,
						Others = m.Others,
						PSL = m.PSL,
						MN = m.MN
					}
				})
				.ToList();

            return Ok(result);
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("API działa");
    }
}
