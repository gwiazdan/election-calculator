namespace election_calculator_backend.Models
{
    public class Municipality
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int Total { get; set; }
		public int NL { get; set; }
		public int KKP { get; set; }
		public int Pl2050 { get; set; }
		public int Konf { get; set; }
		public int PIS { get; set; }
		public int KO { get; set; }
		public int Razem { get; set; }
		public int Others { get; set; }
		public int PSL { get; set; }
		public int MN { get; set; }
		public int CountyID { get; set; }
	}
}
