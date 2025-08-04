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
                        relatedMunicipalities.Sum(m => m.Total)
                        + relatedTerritories.Sum(t => t.Total);
                    int votesForNL =
                        relatedMunicipalities.Sum(m => m.NL)
                        + relatedTerritories.Sum(t => t.NL);
                    int votesForKKP =
                        relatedMunicipalities.Sum(m => m.KKP)
                        + relatedTerritories.Sum(t => t.KKP);
                    int votesForPl2050 =
                        relatedMunicipalities.Sum(m => m.Pl2050)
                        + relatedTerritories.Sum(t => t.Pl2050);
                    int votesForKonf =
                        relatedMunicipalities.Sum(m => m.Konf)
                        + relatedTerritories.Sum(t => t.Konf);
                    int votesForPIS =
                        relatedMunicipalities.Sum(m => m.PIS)
                        + relatedTerritories.Sum(t => t.PIS);
                    int votesForKO =
                        relatedMunicipalities.Sum(m => m.KO)
                        + relatedTerritories.Sum(t => t.KO);
                    int votesForRazem =
                        relatedMunicipalities.Sum(m => m.Razem)
                        + relatedTerritories.Sum(t => t.Razem);
                    int votesForMN = relatedMunicipalities.Sum(m => m.MN) 
                        + relatedTerritories.Sum(t => t.MN ?? 0); // terytoria też mogą mieć MN (nullable)
                    int votesForPSL = relatedMunicipalities.Sum(m => m.PSL)
                        + relatedTerritories.Sum(t => t.PSL);
                    int votesForOthers = relatedMunicipalities.Sum(m => m.Others)
                        + relatedTerritories.Sum(t => t.Others);

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
                            Pl2050 = votesForPl2050,
                            Konfederacja = votesForKonf,
                            PIS = votesForPIS,
                            KO = votesForKO,
                            Razem = votesForRazem,
                            MN = votesForMN,
                            PSL = votesForPSL,
                            Others = votesForOthers,
                        },
                    };
                })
                .ToList();

            return Ok(result);
        }
    }
}
