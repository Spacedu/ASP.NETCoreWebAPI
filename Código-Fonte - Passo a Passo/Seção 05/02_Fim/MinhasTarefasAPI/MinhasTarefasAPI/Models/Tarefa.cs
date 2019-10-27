using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataHora { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public bool Concluido { get; set; }
        public DateTime Criado { get; set; }
        public DateTime Atualizado { get; set; }

        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        
        public virtual ApplicationUser Usuario { get; set; }
    }
}
