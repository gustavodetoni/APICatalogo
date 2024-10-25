using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
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
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
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
            return _context.Categorias.Include(p=> p.Produtos).ToList();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {

            _logger.LogInformation("==================== api/categorias/produtos ======================");

            try
            {
                var categorias = _context.Categorias.ToList();

                if (!categorias.Any())
                {
                    return NotFound($"Categorias não encontrados...");
                }

                return Ok(categorias);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                "Ocorreu um problema ao tratar a sua solicitação.");
                
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            _logger.LogInformation("==================== api/categorias/{id} ======================");
            var categorias = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if (categorias == null)
            {
                return NotFound();
            }
            return categorias;
        }

        [HttpPost]
        public ActionResult Post(Categoria categorias)
        {
            if (categorias is null)
                return BadRequest();
            _context.Categorias.Add(categorias);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categorias.CategoriaId }, categorias);
        }

        [HttpPut]
        public ActionResult Put(int id, Categoria categorias)
        {
            if (id != categorias.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categorias).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categorias);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categorias = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categorias == null)
            {
                return NotFound("Categoria não localizado");
            }

            _context.Categorias.Remove(categorias);
            _context.SaveChanges();

            return Ok(categorias);
        }
    }
}
