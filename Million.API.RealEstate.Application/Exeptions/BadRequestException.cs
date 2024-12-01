using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.API.RealEstate.Application.Exeptions
{
    public class BadRequestException(string message) : ApplicationException(message)
    {
    }
}
