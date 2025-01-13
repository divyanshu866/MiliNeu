namespace MiliNeu.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime SubscribedAt { get; set; }
        public bool IsActive { get; set; }

    }
}
