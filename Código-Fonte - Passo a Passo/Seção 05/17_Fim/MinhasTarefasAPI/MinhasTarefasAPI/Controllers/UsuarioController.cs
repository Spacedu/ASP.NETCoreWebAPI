using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinhasTarefasAPI.Models;
using MinhasTarefasAPI.Repositories.Contracts;

namespace MinhasTarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsuarioController(IUsuarioRepository usuarioRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
            _userManager = userManager;
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
                    _signInManager.SignInAsync(usuario, false);

                    //retorna o Token (JWT)
                    return Ok(BuildToken(usuario));
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

        [HttpPost("")]
        public ActionResult Cadastrar([FromBody]UsuarioDTO usuarioDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usuario = new ApplicationUser();
                usuario.FullName = usuarioDTO.Nome;
                usuario.UserName = usuarioDTO.Email;
                usuario.Email = usuarioDTO.Email;

                var resultado = _userManager.CreateAsync(usuario, usuarioDTO.Senha).Result;

                if (!resultado.Succeeded){
                    List<string> erros = new List<string>();
                    foreach (var erro in resultado.Errors)
                    {
                        erros.Add(erro.Description);
                    }
                    return UnprocessableEntity(erros);
                }
                else
                {
                    return Ok(usuario);
                }

            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        public object BuildToken(ApplicationUser usuario)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-api-jwt-minhas-tarefas")); //Recomendo -> appsettings.json
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires:exp,
                signingCredentials: sign
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new { token = tokenString, expiration = exp };
        }
    }
}