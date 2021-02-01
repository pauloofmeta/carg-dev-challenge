using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAddresses.Domain.Entities;
using MyAddresses.Domain.Models;
using MyAddresses.Services.Abstractions;

namespace MyAddresses.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly ICrudService<User> _service;
        private readonly IMapper _mapper;

        public UsersController(ICrudService<User> service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserModel model)
        {
            var result = await _service.AddAsync(_mapper.Map<User>(model));
            if (result == default || result.Id == default)
                return BadRequest();

            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id },
                _mapper.Map<UserModel>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == default)
                return NotFound();
            return Ok(_mapper.Map<UserModel>(result));
        }
    }
}