using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }
    [Required(ErrorMessage ="O nome é obrigatorio")]
    [StringLength(20, ErrorMessage ="O nome deve conter de 5 a 20 caracteres", MinimumLength = 5)]
    [PrimeiraLetraMaiscula]
    public string? Nome { get; set; }
    [Required]
    [StringLength(10, ErrorMessage ="A descrição deve ter no máximo {1} caracteres")]
    [MaxLength(255)]
    public string? Descricao { get; set; }
    [Required]
    [Range(1, 1000, ErrorMessage ="O preço deve estar entre {1} e {2}")]
    public decimal Preco { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    public int CategoriaId { get; set; }
    [JsonIgnore]
    public Categoria? Categoria { get; set; }


    //segundo tipo de abordagem que pode usar para validar 
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new
                    ValidationResult("A primeira letra do nome deve ser maiscula", new[] { nameof(this.Nome) });
            }
        }
        if(this.Estoque <= 0)
        {
            yield return new
                   ValidationResult("O estoque deve ser maior que zero", new[] { nameof(this.Estoque) });
        }
    }
}
