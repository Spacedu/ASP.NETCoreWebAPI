using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToApi.V1.Models
{
    public class Mensagem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("DeId")]
        public ApplicationUser De { get; set; }
        public string DeId { get; set; }
        [ForeignKey("ParaId")]
        public ApplicationUser Para { get; set; }
        public string ParaId { get; set; }
        public string Texto { get; set; }
        public DateTime Criado { get; set; }

    }
}
