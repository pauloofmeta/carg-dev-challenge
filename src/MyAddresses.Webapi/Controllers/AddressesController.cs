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
    public class AddressesController : ControllerBase
    {
        private readonly IAddressesService _service;
        private readonly IMapper _mapper;

        public AddressesController(IAddressesService service,
            IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddressModel model)
        {
            var result = await _service.AddByUserAsync(_mapper.Map<Address>(model), model.Username);
            if (result == default || result.Id == default)
                return BadRequest();
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, AddressModel model)
        {
            var address = _mapper.Map<Address>(model);
            address.Id = id;
            var result = await _service.UpdateByUserAsync(address, model.Username);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == default)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUser([FromQuery]string username)
        {
            var result = await _service.GetAllByUser(username);
            if (result == default)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, [FromQuery]string username)
        {
            await _service.DeleteByUserAsync(id, username);
            return Ok();
        }
    }
}