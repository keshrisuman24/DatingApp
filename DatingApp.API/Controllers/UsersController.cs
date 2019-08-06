using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usertoreturn=_mapper.Map<IEnumerable<UserListDtos>>(users);
            return Ok(usertoreturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int ID)
        {
            var users = await _repo.GetUser(ID);
            var usertoreturn=_mapper.Map<UserForDetailDto>(users);
            return Ok(usertoreturn);
        }
    }
}