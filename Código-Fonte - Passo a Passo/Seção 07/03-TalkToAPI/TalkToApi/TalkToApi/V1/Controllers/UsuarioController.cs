using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TalkToApi.V1.Models;
using System.Text;
using System.Security.Claims;
using TalkToApi.V1.Repositories.Contracts;
using TalkToApi.V1.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using TalkToApi.Helpers.Contants;
using Microsoft.AspNetCore.Cors;

namespace TalkToApi.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [EnableCors("AnyOrigin")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsuarioController(IMapper mapper, IUsuarioRepository usuarioRepository, ITokenRepository tokenRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [ApiVersion("1.2")]
        [Authorize]
        [HttpGet("", Name = "UsuarioObterTodos")]
        [DisableCors()]
        public ActionResult ObterTodos([FromHeader(Name = "Accept")]string mediaType)
        {
            var usuariosAppUser = _userManager.Users.ToList();

            if (mediaType == CustomMediaType.Hateoas)
            {
                var listaUsuarioDTO = _mapper.Map<List<ApplicationUser>, List<UsuarioDTO>>(usuariosAppUser);

                foreach (var usuarioDTO in listaUsuarioDTO)
                {
                    usuarioDTO.Links.Add(new LinkDTO("_self", Url.Link("UsuarioObter", new { id = usuarioDTO.Id }), "GET"));
                }

                var lista = new ListaDTO<UsuarioDTO>() { Lista = listaUsuarioDTO };
                lista.Links.Add(new LinkDTO("_self", Url.Link("UsuarioObterTodos", null), "GET"));

                return Ok(lista);
            }
            else
            {
                var usuarioResult = _mapper.Map<List<ApplicationUser>, List<UsuarioDTOSemHyperlink>>(usuariosAppUser);
                return Ok(usuarioResult);
            }   
        }

        [HttpGet("{id}", Name = "UsuarioObter")]
        public ActionResult ObterUsuario(string id, [FromHeader(Name = "Accept")]string mediaType)
        {
            var usuario = _userManager.FindByIdAsync(id).Result;
            if (usuario == null)
                return NotFound();

            if (mediaType == CustomMediaType.Hateoas)
            {
                var usuarioDTOdb = _mapper.Map<ApplicationUser, UsuarioDTO>(usuario);
                usuarioDTOdb.Links.Add(new LinkDTO("_self", Url.Link("UsuarioObter", new { id = usuario.Id }), "GET"));
                usuarioDTOdb.Links.Add(new LinkDTO("_atualizar", Url.Link("UsuarioAtualizar", new { id = usuario.Id }), "PUT"));

                return Ok(usuarioDTOdb);
            }
            else
            {
                var usuarioResult = _mapper.Map<ApplicationUser, UsuarioDTOSemHyperlink>(usuario);
                return Ok(usuarioResult);
            }
                
        }


        [HttpPost("", Name = "UsuarioCadastrar")]
        public ActionResult Cadastrar([FromBody]UsuarioDTO usuarioDTO, [FromHeader(Name = "Accept")]string mediaType)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usuario = _mapper.Map<UsuarioDTO, ApplicationUser>(usuarioDTO);

                var resultado = _userManager.CreateAsync(usuario, usuarioDTO.Senha).Result;

                if (!resultado.Succeeded)
                {
                    List<string> erros = new List<string>();
                    foreach (var erro in resultado.Errors)
                    {
                        erros.Add(erro.Description);
                    }
                    return UnprocessableEntity(erros);
                }
                else
                {
                    if (mediaType == CustomMediaType.Hateoas)
                    {
                        var usuarioDTOdb = _mapper.Map<ApplicationUser, UsuarioDTO>(usuario);
                        usuarioDTOdb.Links.Add(new LinkDTO("_self", Url.Link("UsuarioCadastrar", new { id = usuario.Id }), "POST"));
                        usuarioDTOdb.Links.Add(new LinkDTO("_obter", Url.Link("UsuarioObter", new { id = usuario.Id }), "GET"));
                        usuarioDTOdb.Links.Add(new LinkDTO("_atualizar", Url.Link("UsuarioAtualizar", new { id = usuario.Id }), "PUT"));

                        return Ok(usuarioDTOdb);
                    }
                    else
                    {
                        var usuarioResult = _mapper.Map<ApplicationUser, UsuarioDTOSemHyperlink>(usuario);
                        return Ok(usuarioResult);
                    }
                }

            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        /*
         * api/usuario/{id} -> PUT
         */
        [Authorize]
        [HttpPut("{id}", Name = "UsuarioAtualizar")]
        public ActionResult Atualizar(string id, [FromBody]UsuarioDTO usuarioDTO, [FromHeader(Name = "Accept")]string mediaType)
        {
            ApplicationUser usuario = _userManager.GetUserAsync(HttpContext.User).Result;
            if (usuario.Id != id)
            {
                return Forbid();
            }
            
            if (ModelState.IsValid)
            {
                usuario.FullName = usuarioDTO.Nome;
                usuario.UserName = usuarioDTO.Email;
                usuario.Email = usuarioDTO.Email;
                usuario.Slogan = usuarioDTO.Slogan;
                
                var resultado = _userManager.UpdateAsync(usuario).Result;
                _userManager.RemovePasswordAsync(usuario);
                _userManager.AddPasswordAsync(usuario, usuarioDTO.Senha);

                if (!resultado.Succeeded)
                {
                    List<string> erros = new List<string>();
                    foreach (var erro in resultado.Errors)
                    {
                        erros.Add(erro.Description);
                    }
                    return UnprocessableEntity(erros);
                }
                else
                {
                    if (mediaType == CustomMediaType.Hateoas)
                    {
                        var usuarioDTOdb = _mapper.Map<ApplicationUser, UsuarioDTO>(usuario);
                        usuarioDTOdb.Links.Add(new LinkDTO("_self", Url.Link("UsuarioAtualizar", new { id = usuario.Id }), "PUT"));
                        usuarioDTOdb.Links.Add(new LinkDTO("_obter", Url.Link("UsuarioObter", new { id = usuario.Id }), "GET"));

                        return Ok(usuarioDTOdb);
                    }
                    else
                    {
                        var usuarioResult = _mapper.Map<ApplicationUser, UsuarioDTOSemHyperlink>(usuario);
                        return Ok(usuarioResult);
                    }
                        
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }



        [HttpPost("login")]
        public ActionResult Login([FromBody]UsuarioDTO usuarioDTO)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("ConfirmacaoSenha");

            if (ModelState.IsValid)
            {
                ApplicationUser usuario = _usuarioRepository.Obter(usuarioDTO.Email, usuarioDTO.Senha);
                if (usuario != null)
                {
                    //Login no Identity
                    //_signInManager.SignInAsync(usuario, false);


                    //retorna o Token (JWT)
                    return GerarToken(usuario);
                }
                else
                {
                    return NotFound("Usuário não localizado!");
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }



        [HttpPost("renovar")]
        public ActionResult Renovar([FromBody]TokenDTO tokenDTO)
        {
            var refreshTokenDB = _tokenRepository.Obter(tokenDTO.RefreshToken);

            if (refreshTokenDB == null)
                return NotFound();

            //RefreshToken antigo - Atualizar - Desativar esse refreshToken
            refreshTokenDB.Atualizado = DateTime.Now;
            refreshTokenDB.Utilizado = true;
            _tokenRepository.Atualizar(refreshTokenDB);

            //Gerar um novo Token/Refresh Token - Salvar.
            var usuario = _usuarioRepository.Obter(refreshTokenDB.UsuarioId);

            return GerarToken(usuario);
        }


        private TokenDTO BuildToken(ApplicationUser usuario)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Cm2FxFNb2Rgx1IrPoI8M6cC1IcutDawX")); //Recomendo -> appsettings.json
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: exp,
                signingCredentials: sign
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = Guid.NewGuid().ToString();
            var expRefreshToken = DateTime.UtcNow.AddHours(2);

            var tokenDTO = new TokenDTO { Token = tokenString, Expiration = exp, RefreshToken = refreshToken, ExpirationRefreshToken = expRefreshToken };

            return tokenDTO;
        }

        private ActionResult GerarToken(ApplicationUser usuario)
        {
            var token = BuildToken(usuario);

            //Salvar o Token no Banco
            var tokenModel = new Token()
            {
                RefreshToken = token.RefreshToken,
                ExpirationToken = token.Expiration,
                ExpirationRefreshToken = token.ExpirationRefreshToken,
                Usuario = usuario,
                Criado = DateTime.Now,
                Utilizado = false
            };

            _tokenRepository.Cadastrar(tokenModel);
            return Ok(token);
        }
    }
}