namespace election_calculator_backend.DTOs
{
    public class MunicipalityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalVotes { get; set; }
        public int CountyID { get; set; }
        public VotesDto Votes { get; set; } = new();
    }
}
