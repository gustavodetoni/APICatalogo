using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context) : base (context) 
        {
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync (CategoriasParameters categoriasParameters)
        {
            var categorias = _context.Categorias.AsQueryable();

            var categoriasOrdenadas =  categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            //var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParameters)
        {
            var categorias = _context.Categorias.AsQueryable();

            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c=> c.Nome.Contains(categoriasParameters.Nome));
            }

            //var categoriasFiltradas = await  IPagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasParams.PageNumber, categoriasParams.PageSize);

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasFiltradas; 
        }

        //public IEnumerable<Categoria> GetCategorias()
        //{
        //    return _context.Categorias.ToList();

        //}
        //public Categoria GetCategoria(int id)
        //{
        //    return _context.Categorias.FirstOrDefault(c=> c.CategoriaId == id);
        //}
        //public Categoria Create(Categoria categoria)
        //{
        //   if(categoria is null)
        //        throw new ArgumentException(nameof(categoria));

        //    _context.Categorias.Add(categoria);
        //    _context.SaveChanges();

        //    return categoria;
        //}
        //public Categoria Update(Categoria categoria)
        //{
        //    if (categoria is null)
        //        throw new ArgumentException(nameof(categoria));

        //    _context.Entry(categoria).State = EntityState.Modified;
        //    _context.SaveChanges();

        //    return categoria;
        //}
        //public Categoria Delete(int id)
        //{
        //    var categoria = _context.Categorias.Find(id);

        //    if (categoria is null)
        //        throw new ArgumentException(nameof(categoria));

        //    _context.Categorias.Remove(categoria);
        //    _context.SaveChanges();

        //    return categoria;
        //}

    }
}
