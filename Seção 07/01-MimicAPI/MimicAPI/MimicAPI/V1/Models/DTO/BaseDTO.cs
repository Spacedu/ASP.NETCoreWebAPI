using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Models.DTO
{
    public abstract class BaseDTO
    {
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
