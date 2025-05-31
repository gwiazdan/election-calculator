namespace election_calculator_backend.Models
{
    public class County
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int VoivodeshipID { get; set; }
        public int? SenateID { get; set; }
        public int SejmID { get; set; }
        public int SejmikID { get; set; }
        public int EuroparlamentID { get; set; }
    }
}
