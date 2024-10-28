using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;
using X.PagedList.EF;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtoParams)
        //{
        //    return GetAll()
        //        .OrderBy(p => p.Nome)
        //        .Skip((produtoParams.PageNumber - 1) * produtoParams.PageSize)
        //        .Take(produtoParams.PageSize).ToList();
        //}

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = _context.Produtos.AsQueryable();
            var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();
            var resultados = await produtosOrdenados.ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);

            return resultados;
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = _context.Produtos.AsQueryable();

            if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
            }
            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);

            return produtosFiltrados;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriasAsync(int id)
        {
            var produtos = await GetAllAsync();
            var produtosCategorias = produtos.Where(c => c.CategoriaId == id);
            return produtosCategorias;
        }

        //public IQueryable<Produto> GetProdutos()
        //{
        //    return _context.Produtos;
        //}
        //public Produto GetProduto(int id)
        //{
        //    var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        //    if (produto is null)
        //        throw new InvalidOperationException("Produto e null");
                    
        //    return produto;
        //}
        //public Produto Create(Produto produto)
        //{
        //    if (produto is null)
        //        throw new InvalidOperationException("Produto e null");

        //    _context.Produtos.Add(produto);
        //    _context.SaveChanges();

        //    return produto;
        //}
        //public bool Update(Produto produto)
        //{
        //    if (produto is null)
        //        throw new InvalidOperationException("Produto e null");

        //    if (_context.Produtos.Any(p=> p.ProdutoId == produto.ProdutoId))
        //    {
        //        _context.Produtos.Update(produto);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
        //public bool Delete(int id)
        //{
        //    var produto = _context.Produtos.Find(id);

        //    if(produto is not null)
        //    {
        //        _context.Produtos.Remove(produto);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
    }
}
