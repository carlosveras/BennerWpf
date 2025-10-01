#nullable disable

namespace BennerWpf.Models;
public class Produto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Codigo { get; set; }
    public decimal Valor { get; set; }

    public override string ToString() => $"{Nome} - R$ {Valor:F2}";
}

