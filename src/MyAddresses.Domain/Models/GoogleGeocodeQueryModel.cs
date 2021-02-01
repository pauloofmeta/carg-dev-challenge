using Refit;

namespace MyAddresses.Domain.Models
{
    public class GoogleGeocodeQueryModel
    {
        [AliasAs("components")]
        public string Components => "country:BR";
        
        [AliasAs("key")]
        public string Key { get; set; }
        
        [AliasAs("address")]
        public string Address { get; set; }  
    }
}