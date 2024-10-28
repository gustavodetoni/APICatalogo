using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork uof, IMapper mapper, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriaDto = categorias.ToCategoriaDTOList();

            return Ok(categoriaDto);
        }

        [HttpGet("filter/preco/pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco
                                                                                    produtosFilterParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosFiltroPreco(produtosFilterParameters);
            return ObterProdutos(produtos);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }


        //[HttpGet("LerArquivoConfiguracao")]
        //public string GetValores()
        //{
        //    var valor1 = _configuration["chave1"];
        //    var valor2 = _configuration["chave2"];

        //    var secao1 = _configuration["secao1:chave2"];

        //  return $"Chave 1 = {valor1} \nChave2 = {valor2} \nSecao1 => Chave2 = {secao1}";
        //}

        //[HttpGet("UsandoFromServices/{nome}")]
        // public ActionResult<string> GetSaudacaoFromService([FromServices] IMeuServico meuServico, string nome)
        //{
        //    return meuServico.Saudacao(nome);
        //}

        //[HttpGet("SemUsarFromServices/{nome}")]
        //public ActionResult<string> GetSaudacaoSemFromService(IMeuServico meuServico, string nome)
        //{
        //    return meuServico.Saudacao(nome);
        //}


        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {

            _logger.LogInformation("api/categorias/produtos");

            var categorias = _uof.CategoriaRepository.GetAll();

            if (categorias is null || !categorias.Any())
                return NotFound("Não existem categorias...");

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            _logger.LogInformation("==================== api/categorias/{id} ======================");
            var categoria = _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Dados invalidos...");
                return NotFound();
            }

            var categoriaDto = categoria.ToCategoriaDto(); 
            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados invalidos...");
                return BadRequest("Dados invalidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDto();

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning($"Dados invalidos...");
                return BadRequest("Dados invalidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDto();

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categorias = _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);

            if (categorias is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrado...");
                return BadRequest($"Categoria com id = {id} não encontrado...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categorias);
            _uof.Commit();

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDto();

            return Ok(categoriaExcluidaDto);
        }
    }
}
