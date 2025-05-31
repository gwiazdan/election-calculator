namespace election_calculator_backend.Models
{
    public class Territory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int NumberOfVotes { get; set; }
        public int VotesForPIS { get; set; }
        public int VotesForKO { get; set; }
        public int VotesForKonfederacja { get; set; }
        public int VotesForKKP { get; set; }
        public int VotesForTD { get; set; }
        public int VotesForRazem { get; set; }
        public int VotesForNL { get; set; }
        public int? SenateID { get; set; }
        public int? SejmID { get; set; }
        public int? EuroparlamentID { get; set; }
        public int? SejmikID { get; set; }
    }
}
