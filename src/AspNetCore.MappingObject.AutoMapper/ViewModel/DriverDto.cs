using System.Collections.ObjectModel;

namespace AspNetCore.MappingObject.AutoMapper.ViewModel
{
    public class DriverDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Status { get; set; }
        public EmployeeDTO Employee { get; set; }
        public Collection<AddressDTO> Addresses { get; set; }
    }
}
