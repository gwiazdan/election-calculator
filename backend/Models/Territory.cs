namespace election_calculator_backend.Models
{
    public class Territory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Total { get; set; }
        public int LEW { get; set; }
        public int KKP { get; set; }
        public int Pl2050 { get; set; }
        public int Konf { get; set; }
        public int PIS { get; set; }
        public int KO { get; set; }
        public int Razem { get; set; }
        public int Others { get; set; }
        public int PSL { get; set; }
        public int? MN { get; set; }
        public int? SenateID { get; set; }
        public int? SejmID { get; set; }
        public int? EuroparlamentID { get; set; }
        public int? SejmikID { get; set; }
    }
}
