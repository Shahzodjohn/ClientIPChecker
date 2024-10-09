using ClientIPChecker.Dtos;
using ClientIPChecker.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ClientIPChecker.Repositories
{
    public interface IUserService
    {
        Task AddUserConnectionAsync(long userId, string ipAddress);
        Task<IEnumerable<UserDto>> SearchUsersByIpAsync(string ipPart);
        Task<IEnumerable<string>> GetUserIpAddressesAsync(long userId);
        Task<UserLastConnectionDto> GetUserLastConnectionAsync(long userId);
    }
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserConnectionAsync(long userId, string ipAddress)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                user = new User { Id = userId };
                _context.Users.Add(user);

                Log.Information("User with Id = {Id} is saved! ", userId);
            }

            var userIp = new UserIpAddress
            {
                UserId = userId,
                IpAddress = ipAddress,
                ConnectedAt = DateTime.UtcNow
            };

            Log.Information("Setted IP Address = {ipAddress} for UserId = {userId}", ipAddress, userId);

            _context.UserIpAddresses.Add(userIp);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> SearchUsersByIpAsync(string ipPart)
        {
            var users = await _context.Users
                .Include(u => u.IpAddresses)
                .Where(u => u.IpAddresses.Any(ip => ip.IpAddress.Contains(ipPart)))
                .Select(u => new UserDto
                {
                    UserId = u.Id,
                    IpAddresses = u.IpAddresses.Select(ip => ip.IpAddress).ToList()
                })
                .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<string>> GetUserIpAddressesAsync(long userId)
        {
            var ipAddresses = await _context.UserIpAddresses
                .Where(ip => ip.UserId == userId)
                .Select(ip => ip.IpAddress)
                .Distinct()
                .ToListAsync();

            return ipAddresses;
        }

        public async Task<UserLastConnectionDto> GetUserLastConnectionAsync(long userId)
        {
            var lastConnection = await _context.UserIpAddresses
                .Where(ip => ip.UserId == userId)
                .OrderByDescending(ip => ip.ConnectedAt)
                .FirstOrDefaultAsync();

            if (lastConnection == null)
            {
                Log.Warning("Last connection is null!");
                return null;
            }
                

            return new UserLastConnectionDto
            {
                UserId = userId,
                LastIpAddress = lastConnection.IpAddress,
                LastConnectedAt = lastConnection.ConnectedAt
            };
        }
    }

}
