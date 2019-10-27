using MinhasTarefasAPI.Database;
using MinhasTarefasAPI.Models;
using MinhasTarefasAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly MinhasTarefasContext _banco;
        public TarefaRepository(MinhasTarefasContext banco)
        {
            _banco = banco;
        }

        public List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dataUltimaSincronizacao)
        {
            var query = _banco.Tarefas.Where(a=>a.UsuarioId == usuario.Id).AsQueryable();

            if (dataUltimaSincronizacao != null)
            {
                query.Where(a => a.Criado >= dataUltimaSincronizacao || a.Atualizado >= dataUltimaSincronizacao);
            }

            return query.ToList<Tarefa>();
        }

        public void Sincronizacao(List<Tarefa> tarefas)
        {
            // Cadastrar novos registros

            // Atualização de registro (Excluido)
            throw new NotImplementedException();
        }
    }
}
