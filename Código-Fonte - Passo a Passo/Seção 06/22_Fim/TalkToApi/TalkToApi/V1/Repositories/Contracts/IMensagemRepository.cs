using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToApi.V1.Models;

namespace TalkToApi.V1.Repositories.Contracts
{
    public interface IMensagemRepository
    {
        Mensagem Obter(int id);
        List<Mensagem> ObterMensagens(string usuarioUmId, string usuarioDoisId);

        void Cadastrar(Mensagem mensagem);

        void Atualizar(Mensagem mensagem);
    }
}
