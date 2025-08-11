using System.Text.Json;
using election_calculator_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace election_calculator_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SejmikController : ControllerBase
    {
        [HttpGet("load")]
        public async Task<ActionResult<List<object>>> GetSejmikSummaries()
        {
            var sejmikJson = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Directory.GetCurrentDirectory(), "Data", "Sejmiki.json")
            );
            var sejmikDistricts = JsonSerializer.Deserialize<List<SejmikDistrict>>(
                sejmikJson,
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
                sejmikDistricts == null
                || counties == null
                || territories == null
                || municipalities == null
            )
                return BadRequest("Deserialization failed or data is null.");

            var filteredTerritories = territories.Where(t => t.SejmikID != null).ToList();

            var result = sejmikDistricts
                .Select(sejmikDistrict =>
                {
                    var relatedCounties = counties
                        .Where(c => c.SejmikID == sejmikDistrict.Id)
                        .ToList();
                    var relatedTerritories = filteredTerritories
                        .Where(t => t.SejmikID == sejmikDistrict.Id)
                        .ToList();
                    var relatedMunicipalities = municipalities.Where(m =>
                        relatedCounties.Any(c => c.Id == m.CountyID)
                    );

                    int totalVotes =
                        relatedMunicipalities.Sum(m => m.Total)
                        + relatedTerritories.Sum(t => t.Total);
                    int votesForLEW =
                        relatedMunicipalities.Sum(m => m.LEW)
                        + relatedTerritories.Sum(t => t.LEW);
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
                        + relatedTerritories.Sum(t => t.MN ?? 0);
                    int votesForPSL = relatedMunicipalities.Sum(m => m.PSL)
                        + relatedTerritories.Sum(t => t.PSL);
                    int votesForOthers = relatedMunicipalities.Sum(m => m.Others)
                        + relatedTerritories.Sum(t => t.Others);

                    return new
                    {
                        Id = sejmikDistrict.Id,
                        Name = sejmikDistrict.Name,
                        Number = sejmikDistrict.Number,
                        VoivodeshipID = sejmikDistrict.VoivodeshipID,
                        Seats = sejmikDistrict.Seats,
                        TotalVotes = totalVotes,
                        Votes = new
                        {
                            LEW = votesForLEW,
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
