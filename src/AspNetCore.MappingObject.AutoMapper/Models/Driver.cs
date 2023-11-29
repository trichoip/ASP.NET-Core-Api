using System.Collections.ObjectModel;

namespace AspNetCore.MappingObject.AutoMapper.Models
{
    public class Driver
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Status { get; set; }
        public Employee Employee { get; set; }
        public Collection<Address> Addresses { get; set; }
    }
}
