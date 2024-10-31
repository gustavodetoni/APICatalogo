using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [EnableCors("OrigensComAcessoPermitdo")]
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedwindow")]
    [ApiExplorerSettings(IgnoreApi = true)]
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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriaDto = categorias.ToCategoriaDTOList();

            return Ok(categoriaDto);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPrecoAsync([FromQuery] ProdutosFiltroPreco
                                                                                    produtosFilterParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFilterParameters);
            return ObterProdutos(produtos);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageCount,
                produtos.TotalItemCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
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
            var categorias = _uof.CategoriaRepository.GetAllAsync();
            return Ok(categorias);
        }

        [DisableCors] 
        [Authorize]
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
        {

            _logger.LogInformation("api/categorias/produtos");

            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias is null || !categorias.Any())
                return NotFound("Não existem categorias...");

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
        {
            _logger.LogInformation("==================== api/categorias/{id} ======================");
            var categoria = await _uof.CategoriaRepository.GetAsync(c=> c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Dados invalidos...");
                return NotFound();
            }

            var categoriaDto = categoria.ToCategoriaDto(); 
            return Ok(categoriaDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostAsync(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados invalidos...");
                return BadRequest("Dados invalidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDto();

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut]
        public async Task<ActionResult<CategoriaDTO>> PutAsync(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning($"Dados invalidos...");
                return BadRequest("Dados invalidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDto();

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoriaDTO>> DeleteAsync(int id)
        {
            var categorias = await _uof.CategoriaRepository.GetAsync(c=> c.CategoriaId == id);

            if (categorias is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrado...");
                return BadRequest($"Categoria com id = {id} não encontrado...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categorias);
            await _uof.CommitAsync();

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDto();

            return Ok(categoriaExcluidaDto);
        }
    }
}
