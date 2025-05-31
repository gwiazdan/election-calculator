using System.Text.Json;
using election_calculator_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace election_calculator_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoivodeshipController : ControllerBase
    {
        private readonly string jsonFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "Voivodeships.json"
        );

        [HttpGet("load")]
        public async Task<ActionResult<List<object>>> GetVoivodeshipSummaries(){

            var voivodeshipsJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Voivodeships.json")
            );
            var voivodeships = JsonSerializer.Deserialize<List<Voivodeship>>(
                voivodeshipsJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );


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

            if (voivodeships == null || counties == null || municipalities == null)
                return BadRequest("Deserialization failed or data is null.");

            var result = voivodeships
                .Select(voivodeship =>
                {
                    var relatedCounties = counties.Where(c => c.VoivodeshipID == voivodeship.Id).ToList();
                    var relatedMunicipalities = municipalities.Where(m => relatedCounties.Any(c => c.Id == m.CountyID));

                    return new
                    {
                        Id = voivodeship.Id,
                        Name = voivodeship.Name,
                        TotalVotes = relatedMunicipalities.Sum(m => m.NumberOfVotes),
                        Votes = new
                        {
                            NL = relatedMunicipalities.Sum(m => m.VotesForNL),
                            KKP = relatedMunicipalities.Sum(m => m.VotesForKKP),
                            TD = relatedMunicipalities.Sum(m => m.VotesForTD),
                            Konfederacja = relatedMunicipalities.Sum(m => m.VotesForKonfederacja),
                            PIS = relatedMunicipalities.Sum(m => m.VotesForPIS),
                            KO = relatedMunicipalities.Sum(m => m.VotesForKO),
                            Razem = relatedMunicipalities.Sum(m => m.VotesForRazem),
                            MN = relatedMunicipalities.Sum(m => m.VotesForMN ?? 0),
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