using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToApi.V1.Models.DTO
{
    public class ListaDTO<T> : BaseDTO
    {
        public List<T> Lista { get; set; }
    }
}
