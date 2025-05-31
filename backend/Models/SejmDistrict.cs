namespace election_calculator_backend.Models
{
    public class SejmDistrict
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public required int Seats { get; set; }
    }
}
