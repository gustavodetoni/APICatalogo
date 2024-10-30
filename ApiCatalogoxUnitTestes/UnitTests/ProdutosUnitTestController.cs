using APICatalogo.Repositories;
using APICatalogo.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using APICatalogo.DTOs.Mappings;


namespace ApiCatalogoxUnitTestes.UnitTests
{
    public class ProdutosUnitTestController
    {
        public IUnitOfWork repository;
        public IMapper mapper;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;DataBase=ApiCatalogoDb;Uid=root;Password=root";

        static ProdutosUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                            .Options;
        }

        public ProdutosUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProdutoDTOMappingProfile());
            });

            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);
            repository = new UnitOfWork(context);
        }
    }
}
