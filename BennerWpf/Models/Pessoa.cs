#nullable disable

namespace BennerWpf.Models;
public class Pessoa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string CPF { get; set; }
    public string Endereco { get; set; }

    public override string ToString() => $"{Nome} ({CPF})";
}
