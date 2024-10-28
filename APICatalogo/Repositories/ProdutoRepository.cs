using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

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

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdenados;
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = GetAll().AsQueryable();

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
            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroParams.PageNumber,
                                                                                                  produtosFiltroParams.PageSize);
            return produtosFiltrados;
        }

        public IEnumerable<Produto> GetProdutosPorCategorias(int id)
        {
            return GetAll().Where(c=> c.CategoriaId == id);
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
