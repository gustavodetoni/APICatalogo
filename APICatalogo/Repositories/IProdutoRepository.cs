using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtoParams);
        PagedList<Produto> GetProdutos(ProdutosParameters produtoParams);
        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPreco);
        IEnumerable<Produto> GetProdutosPorCategorias(int id);
    }
}
