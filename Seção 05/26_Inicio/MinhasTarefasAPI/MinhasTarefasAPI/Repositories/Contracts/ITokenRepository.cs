using MinhasTarefasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.Repositories.Contracts
{
    public interface ITokenRepository
    {
        //C - R - U.
        void Cadastrar(Token token);
        //Key-Value
        Token Obter(string refreshToken);
        void Atualizar(Token token);
    }
}
