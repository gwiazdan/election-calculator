namespace election_calculator_backend.Models
{
    public class SejmikDistrict
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required int Number { get; set; }

        public required int VoivodeshipID { get; set; }

        public required int Seats { get; set; }
    }
}
