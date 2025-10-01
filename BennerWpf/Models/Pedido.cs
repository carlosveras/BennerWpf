#nullable disable

namespace BennerWpf.Models;

public class Pedido
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Pessoa Pessoa { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
    public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
    public DateTime DataVenda { get; set; } = DateTime.Now;
    public FormaPagamento FormaPagamento { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Pendente;
}