namespace ClientIPChecker.Models
{
    public class UserIpAddress
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public string IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; }
    }
}
