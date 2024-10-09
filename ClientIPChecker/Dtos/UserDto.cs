namespace ClientIPChecker.Dtos
{
    public class UserDto
    {
        public long UserId { get; set; }
        public List<string> IpAddresses { get; set; }
    }
}
