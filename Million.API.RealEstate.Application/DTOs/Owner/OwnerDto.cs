using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.API.RealEstate.Application.DTOs.Owner
{
    public class OwnerDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string? Photo { get; set; }

        public DateTime BirthDay { get; set; }
    }
}
