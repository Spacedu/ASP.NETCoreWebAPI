using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly MimicContext _banco;
        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }

        //APP -- /api/palavras
        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas([FromQuery]PalavraUrlQuery query)
        {
            if (query.PagNumero > paginacao.TotalPaginas)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginacao));

            return Ok(item.ToList());
        }


        

        //WEB -- /api/palavras/1        
        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(int id)
        {
            var obj = 

            if (obj == null)
                return NotFound();

            return Ok();
        }

        // -- /api/palavras(POST: id, nome, ativo, pontuacao, criacao)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            

            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        // -- /api/palavras/1 (PUT: id, nome, ativo, pontuacao, criacao)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody]Palavra palavra)
        {
            var obj = 

            if (obj == null)
                return NotFound();

            palavra.Id = id;
            

            return Ok();
        }

        // -- /api/palavras/1 (DELETE)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            var palavra = _banco.Palavras.Find(id);

            if (palavra == null)
                return NotFound();

            palavra.Ativo = false;
            

            return NoContent();
        }
    }
}
