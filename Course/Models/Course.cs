namespace Course.Models
{
    public class Coursess
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
    }
}
