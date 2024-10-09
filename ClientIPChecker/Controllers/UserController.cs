using ClientIPChecker.Dtos;
using ClientIPChecker.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientIPChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> ConnectUser([FromBody] ConnectUserDto dto)
        {
            await _userService.AddUserConnectionAsync(dto.UserId, dto.IpAddress);
            return Ok();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsersByIp([FromQuery] string ipPart)
        {
            var users = await _userService.SearchUsersByIpAsync(ipPart);
            return Ok(users);
        }

        [HttpGet("{userId}/ips")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserIpAddresses(long userId)
        {
            var ips = await _userService.GetUserIpAddressesAsync(userId);
            return Ok(ips);
        }

        [HttpGet("{userId}/last-connection")]
        public async Task<ActionResult<UserLastConnectionDto>> GetUserLastConnection(long userId)
        {
            var lastConnection = await _userService.GetUserLastConnectionAsync(userId);
            if (lastConnection == null)
                return NotFound();

            return Ok(lastConnection);
        }
    }
}
