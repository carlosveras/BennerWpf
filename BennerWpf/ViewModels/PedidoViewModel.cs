using BennerWpf.Models;
using BennerWpf.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

#nullable disable

namespace BennerWpf.ViewModels;

public class PedidoViewModel : INotifyPropertyChanged
{
    private readonly DataService<Pessoa> _pessoaService = new("pessoas.json");
    private readonly DataService<Produto> _produtoService = new("produtos.json");
    private readonly DataService<Pedido> _pedidoService = new("pedidos.json");

    public ObservableCollection<Pessoa> Pessoas { get; set; }
    public ObservableCollection<Produto> Produtos { get; set; }
    public ObservableCollection<ItemPedido> ItensPedido { get; set; } = new();

    public Pessoa PessoaSelecionada { get; set; }
    public Produto ProdutoSelecionado { get; set; }
    public int QuantidadeProduto { get; set; } = 1;

    public FormaPagamento FormaPagamentoSelecionada { get; set; }
    public decimal ValorTotal => ItensPedido.Sum(i => i.Subtotal);

    public ICommand AdicionarProdutoCommand { get; }
    public ICommand FinalizarPedidoCommand { get; }
   
   

    public PedidoViewModel()
    {
        Pessoas = new ObservableCollection<Pessoa>(_pessoaService.Load());
        Produtos = new ObservableCollection<Produto>(_produtoService.Load());

        AdicionarProdutoCommand = new RelayCommand(_ => AdicionarProduto());
        FinalizarPedidoCommand = new RelayCommand(_ => FinalizarPedido());
    }

    private void AdicionarProduto()
    {
        if (ProdutoSelecionado != null && QuantidadeProduto > 0)
        {
            ItensPedido.Add(new ItemPedido
            {
                Produto = ProdutoSelecionado,
                Quantidade = QuantidadeProduto
            });
            OnPropertyChanged(nameof(ValorTotal));
        }
    }

    private void FinalizarPedido()
    {
        if (PessoaSelecionada == null || ItensPedido.Count == 0)
            return;

        var pedido = new Pedido
        {
            Pessoa = PessoaSelecionada,
            Itens = ItensPedido.ToList(),
            FormaPagamento = FormaPagamentoSelecionada,
            Status = StatusPedido.Pendente
        };

        var pedidos = _pedidoService.Load();
        pedidos.Add(pedido);
        _pedidoService.Save(pedidos);

        ItensPedido.Clear();
        OnPropertyChanged(nameof(ValorTotal));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}


