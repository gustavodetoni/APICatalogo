using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulandoCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) Values ('Bebidas', 'bebida.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) Values ('Lanches', 'lanche.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) Values ('Salgado', 'salgado.jpg')");
            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) Values ('Coca-cola diet', 'Refrigerante de Cola 450ml', 5.45, 'cocacola.jpg', 50, now(),1)");
            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) Values ('Lanche de Atum', 'Lanche de Atum com maionese', 8.5, 'atum.jpg', 10, now(),2)");
            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) Values ('Pudim 100g', 'Pudim de leite condensado', 6.5, 'pudim.jpg', 20, now(),3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
            mb.Sql("Delete from Produtos");
        }
    }
}
