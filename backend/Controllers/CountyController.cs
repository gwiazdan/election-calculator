using System.Text.Json;
using election_calculator_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace election_calculator_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountyController : ControllerBase
    {
        private readonly string jsonFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "Counties.json"
        );

        [HttpGet("load")]
        public async Task<ActionResult<List<object>>> GetCountySummaries()
        {
            var countiesJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Counties.json")
            );
            var counties = JsonSerializer.Deserialize<List<County>>(
                countiesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var municipalitiesJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Municipalities.json")
            );
            var municipalities = JsonSerializer.Deserialize<List<Municipality>>(
                municipalitiesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (counties == null || municipalities == null)
                return BadRequest("Deserialization failed or data is null.");

            var result = counties
                .Select(county =>
                {
                    var relatedMunicipalities = municipalities.Where(m => m.CountyID == county.Id);

                    return new
                    {
                        Id = county.Id,
                        Name = county.Name,
                        VoivodeshipID = county.VoivodeshipID,
                        TotalVotes = relatedMunicipalities.Sum(m => m.Total),
                        Votes = new
                        {
                            NL = relatedMunicipalities.Sum(m => m.NL),
                            KKP = relatedMunicipalities.Sum(m => m.KKP),
                            Pl2050 = relatedMunicipalities.Sum(m => m.Pl2050),
                            Konfederacja = relatedMunicipalities.Sum(m => m.Konf),
                            PIS = relatedMunicipalities.Sum(m => m.PIS),
                            KO = relatedMunicipalities.Sum(m => m.KO),
                            Razem = relatedMunicipalities.Sum(m => m.Razem),
                            MN = relatedMunicipalities.Sum(m => m.MN),
                            PSL = relatedMunicipalities.Sum(m => m.PSL),
                            Others = relatedMunicipalities.Sum(m => m.Others),
                        },
                    };
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("API działa");
    }
}
