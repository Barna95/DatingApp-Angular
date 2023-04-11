using System.Security.Claims;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberAsync(username);
            return user;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //to recognize the user we have to use ClaimTypes.NameIdentifier, not the JwtRegisteredClaimNames.NameId part even after registering with that
            var user = await _userRepository.GetUserByNameAsync(username);

            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);
            //above the users properties are getting overwritten by the mapper, to the found user

            if (await _userRepository.SaveAllAsync()) return NoContent();
            //if everything goes well, return 204 

            return BadRequest("Failed to update user");
        }
    }
}
