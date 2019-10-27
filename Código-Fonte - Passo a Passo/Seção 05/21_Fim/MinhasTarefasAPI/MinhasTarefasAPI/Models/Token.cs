using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; }
        public ApplicationUser Usuario { get; set; }
        public bool Utilizado { get; set; }
        public DateTime ExpirationToken { get; set; }
        public DateTime ExpirationRefreshToken { get; set; }
        public DateTime Criado { get; set; }
        public DateTime? Atualizado { get; set; }
    }
}
