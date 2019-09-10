using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToApi.V1.Models;

namespace TalkToApi.V1.Repositories.Contracts
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
