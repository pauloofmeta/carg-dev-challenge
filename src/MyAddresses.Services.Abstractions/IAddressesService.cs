using System.Collections.Generic;
using System.Threading.Tasks;
using MyAddresses.Domain.Entities;

namespace MyAddresses.Services.Abstractions
{
    public interface IAddressesService: ICrudService<Address>
    {
         Task<Address> AddByUserAsync(Address model, string username);
         Task<Address> UpdateByUserAsync(Address model, string username);
         Task DeleteByUserAsync(int id, string username);
         Task<IEnumerable<Address>> GetAllByUser(string username);
    }
}