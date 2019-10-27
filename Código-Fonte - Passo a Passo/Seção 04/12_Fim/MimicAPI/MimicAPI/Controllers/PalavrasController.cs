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
            var item = _banco.Palavras.AsQueryable();
            if (query.Data.HasValue)
            {
                item = item.Where(a => a.Criado > query.Data.Value || a.Atualizado > query.Data.Value);
            }

            if (query.PagNumero.HasValue)
            {
                var quantidadeTotalRegistros = item.Count();
                item = item.Skip((query.PagNumero.Value - 1) * query.PagRegistro.Value).Take(query.PagRegistro.Value);

                var paginacao = new Paginacao();
                paginacao.NumeroPagina = query.PagNumero.Value;
                paginacao.RegistroPorPagina = query.PagRegistro.Value;
                paginacao.TotalRegistros = quantidadeTotalRegistros;
                paginacao.TotalPaginas = (int) Math.Ceiling( (double)quantidadeTotalRegistros / query.PagRegistro.Value );

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject( paginacao ));

                if (query.PagNumero > paginacao.TotalPaginas)
                {
                    return NotFound();
                }

            }
            return Ok(item);
        }


        

        //WEB -- /api/palavras/1        
        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(int id)
        {
            var obj = _banco.Palavras.Find(id);

            if (obj == null)
                return NotFound();

            return Ok();
        }

        // -- /api/palavras(POST: id, nome, ativo, pontuacao, criacao)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();

            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        // -- /api/palavras/1 (PUT: id, nome, ativo, pontuacao, criacao)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody]Palavra palavra)
        {
            var obj = _banco.Palavras.AsNoTracking().FirstOrDefault(a=>a.Id == id);

            if (obj == null)
                return NotFound();

            palavra.Id = id;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

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
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

            return NoContent();
        }
    }
}
