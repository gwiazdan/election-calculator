using System.Text.Json;
using election_calculator_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace election_calculator_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SejmController : ControllerBase
    {
        [HttpGet("load")]
        public async Task<ActionResult<List<object>>> GetSejmSummaries()
        {
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

            var sejmJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Sejm.json")
            );
            var sejmDistricts = JsonSerializer.Deserialize<List<SejmDistrict>>(
                sejmJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (
                counties == null
                || territories == null
                || municipalities == null
                || sejmDistricts == null
            )
                return BadRequest("Deserialization failed or data is null.");

            // Uwzględnij tylko terytoria z przypisanym SejmID
            var filteredTerritories = territories.Where(t => t.SejmID != null).ToList();

            // Łączymy powiaty i terytoria (SejmID w powiatach jest int, w terytoriach int?)

            var result = sejmDistricts
                .Select(sejmDistrict =>
                {
                    // Powiaty i terytoria przypisane do tego okręgu
                    var relatedCounties = counties.Where(c => c.SejmID == sejmDistrict.Id).ToList();
                    var relatedTerritories = filteredTerritories
                        .Where(t => t.SejmID == sejmDistrict.Id)
                        .ToList();

                    // Gminy przypisane do powiatów tego okręgu
                    var relatedMunicipalities = municipalities.Where(m =>
                        relatedCounties.Any(c => c.Id == m.CountyID)
                    );

                    // Sumowanie głosów: gminy + terytoria
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
                    int votesForMN = relatedMunicipalities.Sum(m => m.VotesForMN ?? 0); // tylko gminy mają MN

                    return new
                    {
                        Id = sejmDistrict.Id,
                        Name = sejmDistrict.Name,
                        Seats = sejmDistrict.Seats,
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
