using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork uof, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _uof = uof;
        }

        [HttpGet("LerArquivoConfiguracao")]
        public string GetValores()
        {
            var valor1 = _configuration["chave1"];
            var valor2 = _configuration["chave2"];

            var secao1 = _configuration["secao1:chave2"];

            return $"Chave 1 = {valor1} \nChave2 = {valor2} \nSecao1 => Chave2 = {secao1}";
        }

        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromService([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("SemUsarFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoSemFromService(IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }


        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {

            _logger.LogInformation("==================== api/categorias/produtos ======================");

            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            _logger.LogInformation("==================== api/categorias/{id} ======================");
            var categoria = _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Dados invalidos...");
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning($"Dados invalidos...");
                return BadRequest("Dados invalidos");
            }

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut]
        public ActionResult Put(int id, Categoria categorias)
        {
            if (id != categorias.CategoriaId)
            {
                _logger.LogWarning($"Dados invalidos...");
                return BadRequest("Dados invalidos");
            }

            _uof.CategoriaRepository.Update(categorias);
            _uof.Commit();
            return Ok(categorias);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categorias = _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);

            if (categorias is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrado...");
                return BadRequest($"Categoria com id = {id} não encontrado...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categorias);
            _uof.Commit();
            return Ok(categoriaExcluida);
        }
    }
}
