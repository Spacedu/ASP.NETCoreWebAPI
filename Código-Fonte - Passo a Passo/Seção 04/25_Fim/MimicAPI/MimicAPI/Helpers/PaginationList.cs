using MimicAPI.V1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class PaginationList<T>
    {
        public List<T> Results { get; set; } = new List<T>();
        public Paginacao Paginacao { get; set; }
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
