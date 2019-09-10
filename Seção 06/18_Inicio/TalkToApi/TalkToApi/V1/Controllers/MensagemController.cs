using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TalkToApi.V1.Models;
using TalkToApi.V1.Models.DTO;
using TalkToApi.V1.Repositories.Contracts;

namespace TalkToApi.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MensagemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMensagemRepository _mensagemRepository;
        public MensagemController(IMapper mapper, IMensagemRepository mensagemRepository)
        {
            _mapper = mapper;
            _mensagemRepository = mensagemRepository;
        }

        [Authorize]
        [HttpGet("{usuarioUmId}/{usuarioDoisId}", Name ="Obter")]
        public ActionResult Obter(string usuarioUmId, string usuarioDoisId) {

            if (usuarioUmId == usuarioDoisId)
            {
                return UnprocessableEntity();
            }

            var mensagens = _mensagemRepository.ObterMensagens(usuarioUmId, usuarioDoisId);
            var listaMsg = _mapper.Map<List<Mensagem>, List<MensagemDTO>>(mensagens);

            var lista = new ListaDTO<MensagemDTO>() { Lista = listaMsg };
            lista.Links.Add(new LinkDTO("_self", Url.Link("Obter", new { usuarioUmId = usuarioUmId, usuarioDoisId = usuarioDoisId }), "GET"));

            return Ok(lista);
        }

        [Authorize]
        [HttpPost("", Name = "Cadastrar")]
        public ActionResult Cadastrar([FromBody]Mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _mensagemRepository.Cadastrar(mensagem);

                    var mensagemDTO = _mapper.Map<Mensagem, MensagemDTO>(mensagem);
                    mensagemDTO.Links.Add(new LinkDTO("_self", Url.Link("Cadastrar", null), "POST"));
                    mensagemDTO.Links.Add(new LinkDTO("_atualizacaoParcial", Url.Link("AtualizacaoParcial", new { id = mensagem.Id }), "PATCH"));

                    return Ok(mensagemDTO);
                }
                catch (Exception e)
                {
                    return UnprocessableEntity(e);
                }

            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        [Authorize]
        [HttpPatch("{id}", Name = "AtualizacaoParcial")]
        public ActionResult AtualizacaoParcial(int id, [FromBody]JsonPatchDocument<Mensagem> jsonPatch)
        {
            if(jsonPatch == null)
            {
                return BadRequest();
            }
            /*
             * JSONPatch - [{ "op": "add|remove|replace", "path": "texto", "value": "Mensagem substuida!"}, { "op": "add|remove|replace", "path": "exluido", "value": true}]             *
             */

            var mensagem = _mensagemRepository.Obter(id);

            jsonPatch.ApplyTo(mensagem);
            mensagem.Atualizado = DateTime.UtcNow;

            _mensagemRepository.Atualizar(mensagem);


            var mensagemDTO = _mapper.Map<Mensagem, MensagemDTO>(mensagem);
            mensagemDTO.Links.Add(new LinkDTO("_self", Url.Link("AtualizacaoParcial", new { id = mensagem.Id }), "PATCH"));

            return Ok(mensagemDTO);
        }
    }
}