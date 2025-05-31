using System.Text.Json;
using election_calculator_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace election_calculator_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SenateController : ControllerBase
    {
        [HttpGet("load")]
        public async Task<ActionResult<List<object>>> GetSenateSummaries()
        {
            var senateJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Senate.json")
            );
            var senateDistricts = JsonSerializer.Deserialize<List<SenateDistrict>>(
                senateJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var countiesJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Counties.json")
            );
            var counties = JsonSerializer.Deserialize<List<County>>(
                countiesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var territoriesJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Territories.json")
            );
            var territories = JsonSerializer.Deserialize<List<Territory>>(
                territoriesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var municipalitiesJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Municipalities.json")
            );
            var municipalities = JsonSerializer.Deserialize<List<Municipality>>(
                municipalitiesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (
                senateDistricts == null
                || counties == null
                || territories == null
                || municipalities == null
            )
                return BadRequest("Deserialization failed or data is null.");

            var filteredTerritories = territories.Where(t => t.SenateID != null).ToList();

            var result = senateDistricts
                .Select(senateDistrict =>
                {
                    var relatedCounties = counties
                        .Where(c => c.SenateID == senateDistrict.Id)
                        .ToList();
                    var relatedTerritories = filteredTerritories
                        .Where(t => t.SenateID == senateDistrict.Id)
                        .ToList();
                    var relatedMunicipalities = municipalities.Where(m =>
                        relatedCounties.Any(c => c.Id == m.CountyID)
                    );

                    int totalVotes =
                        relatedMunicipalities.Sum(m => m.NumberOfVotes)
                        + relatedTerritories.Sum(t => t.NumberOfVotes);
                    int votesForNL =
                        relatedMunicipalities.Sum(m => m.VotesForNL)
                        + relatedTerritories.Sum(t => t.VotesForNL);
                    int votesForKKP =
                        relatedMunicipalities.Sum(m => m.VotesForKKP)
                        + relatedTerritories.Sum(t => t.VotesForKKP);
                    int votesForTD =
                        relatedMunicipalities.Sum(m => m.VotesForTD)
                        + relatedTerritories.Sum(t => t.VotesForTD);
                    int votesForKonf =
                        relatedMunicipalities.Sum(m => m.VotesForKonfederacja)
                        + relatedTerritories.Sum(t => t.VotesForKonfederacja);
                    int votesForPIS =
                        relatedMunicipalities.Sum(m => m.VotesForPIS)
                        + relatedTerritories.Sum(t => t.VotesForPIS);
                    int votesForKO =
                        relatedMunicipalities.Sum(m => m.VotesForKO)
                        + relatedTerritories.Sum(t => t.VotesForKO);
                    int votesForRazem =
                        relatedMunicipalities.Sum(m => m.VotesForRazem)
                        + relatedTerritories.Sum(t => t.VotesForRazem);
                    int votesForMN = relatedMunicipalities.Sum(m => m.VotesForMN ?? 0);

                    return new
                    {
                        Id = senateDistrict.Id,
                        Name = senateDistrict.Name,
                        TotalVotes = totalVotes,
                        Votes = new
                        {
                            NL = votesForNL,
                            KKP = votesForKKP,
                            TD = votesForTD,
                            Konfederacja = votesForKonf,
                            PIS = votesForPIS,
                            KO = votesForKO,
                            Razem = votesForRazem,
                            MN = votesForMN,
                        },
                    };
                })
                .ToList();

            return Ok(result);
        }
    }
}
