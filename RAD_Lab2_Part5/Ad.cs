namespace RAD_Lab2_Part5
{
    public class Ad
    {
        public int Id { get; set; }
        public Seller? Seller { get; set; }
        public string? Description { get; set; }
        public int CategryId { get; set; }
        public Category? Category { get; set; }
    }
}
