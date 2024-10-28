using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtoParams);
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtoParams);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriasAsync(int id);
    }
}
