using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using usuario.Model;
using usuario.Repository;

namespace usuario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository repository;

        public UsuarioController(IUsuarioRepository repository) 
        {
            this.repository = repository;
        }


        [HttpGet]
        public async Task <IActionResult> Get (){

            var usuarios = await this.repository.BuscaUsuarios();
            return usuarios.Any () 
                    ? Ok(usuarios)
                    : NoContent();
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> GetById (int id){

            var usuario = await this.repository.BuscaUsuario(id);
            return usuario != null
                    ? Ok(usuario)
                    : NotFound("Usuário não encontrado");
        }

         [HttpPost]
        public async Task <IActionResult> Post (Usuario usuario){
            this.repository.AdicionaUsuario(usuario);
            return await this.repository.SaveChangesAsync()
                    ? Ok("usuário adicionado com sucesso")
                    : BadRequest ("Erro ao salvar o usuario");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put (int  id, Usuario usuario) 
        {
            var usuarioBanco = await this.repository.BuscaUsuario(id);   
            if (usuario == null) return NotFound("Usuário não encontrado");

            usuarioBanco.Nome = usuario.Nome ?? usuarioBanco.Nome;
            usuarioBanco.DataNascimento = usuario.DataNascimento != new DateTime()
            ? usuario.DataNascimento : usuarioBanco.DataNascimento;

            this.repository.AtualizaUsuario(usuarioBanco);

            return await this.repository.SaveChangesAsync()
                    ? Ok("usuário atualizado com sucesso")
                    : BadRequest ("Erro ao atualizar o usuario");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var usuarioBanco = await this.repository.BuscaUsuario(id);   
            if (usuarioBanco == null) return NotFound("Usuário não encontrado");

            this.repository.DeletaUsuario(usuarioBanco);

            return await this.repository.SaveChangesAsync()
                    ? Ok("usuário deletado com sucesso")
                    : BadRequest ("Erro ao deletar o usuario");
        }
    }
}