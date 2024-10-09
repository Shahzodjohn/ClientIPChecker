namespace ClientIPChecker.Dtos
{
    public class UserLastConnectionDto
    {
        public long UserId { get; set; }
        public string LastIpAddress { get; set; }
        public DateTime LastConnectedAt { get; set; }
    }
}
