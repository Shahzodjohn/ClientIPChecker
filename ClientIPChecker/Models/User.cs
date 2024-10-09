namespace ClientIPChecker.Models
{
    public class User
    {
        public long Id { get; set; }
        public ICollection<UserIpAddress> IpAddresses { get; set; }
    }
}
