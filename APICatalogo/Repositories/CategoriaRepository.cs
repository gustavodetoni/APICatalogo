using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context) : base (context) 
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();
            var categoriaOrdenados = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriaOrdenados;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams)
        {
            var categorias = GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(categoriasParams.Nome))
            {
                categorias = categorias.Where(c=> c.Nome.Contains(categoriasParams.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

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
