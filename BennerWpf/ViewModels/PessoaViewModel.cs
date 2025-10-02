using BennerWpf.Models;
using BennerWpf.Services;
using BennerWpf.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

#nullable disable

namespace BennerWpf.ViewModels;

public class PessoaViewModel : INotifyPropertyChanged
{
    private readonly DataService<Pessoa> _service = new("pessoas.json");
    private readonly DataService<Pedido> _pedidoService = new("pedidos.json");

    public ObservableCollection<Pessoa> Pessoas { get; set; } = new();

    private Pessoa _pessoaSelecionada;
    public Pessoa PessoaSelecionada
    {
        get => _pessoaSelecionada;
        set
        {
            if (_pessoaSelecionada != value)
            {
                _pessoaSelecionada = value;
                OnPropertyChanged();
                FiltrarPedidos();
            }
        }
    }

    private ObservableCollection<Pedido> _pedidosDaPessoa = new();
    public ObservableCollection<Pedido> PedidosDaPessoa
    {
        get => _pedidosDaPessoa;
        set
        {
            _pedidosDaPessoa = value;
            OnPropertyChanged();
        }
    }

    private bool _mostrarEntregues, _mostrarPagos, _mostrarPendentes;

    public bool MostrarEntregues
    {
        get => _mostrarEntregues;
        set { _mostrarEntregues = value; FiltrarPedidos(); OnPropertyChanged(); }
    }
    public bool MostrarPagos
    {
        get => _mostrarPagos;
        set { _mostrarPagos = value; FiltrarPedidos(); OnPropertyChanged(); }
    }
    public bool MostrarPendentes
    {
        get => _mostrarPendentes;
        set { _mostrarPendentes = value; FiltrarPedidos(); OnPropertyChanged(); }
    }

    private string _filtroNome;
    public string FiltroNome
    {
        get => _filtroNome;
        set { _filtroNome = value; Filtrar(); OnPropertyChanged(); }
    }

    private string _filtroCPF;
    public string FiltroCPF
    {
        get => _filtroCPF;
        set { _filtroCPF = value; Filtrar(); OnPropertyChanged(); }
    }

    public ICommand IncluirCommand { get; }
    public ICommand SalvarCommand { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand IncluirPedidoCommand => new RelayCommand(_ => AbrirTelaPedido(), _ => PessoaSelecionada != null);

    public ICommand MarcarPagoCommand => new RelayCommand(p => AtualizarStatus(p, StatusPedido.Pago));
    public ICommand MarcarEnviadoCommand => new RelayCommand(p => AtualizarStatus(p, StatusPedido.Enviado));
    public ICommand MarcarRecebidoCommand => new RelayCommand(p => AtualizarStatus(p, StatusPedido.Recebido));

    public PessoaViewModel()
    {
        CarregarDados();
        IncluirCommand = new RelayCommand(_ => Incluir());
        SalvarCommand = new RelayCommand(_ => Salvar());
        ExcluirCommand = new RelayCommand(_ => Excluir());
    }

    public void Inicializar(Pessoa pessoa)
    {
        var pessoasCarregadas = _service.Load();
        Pessoas = new ObservableCollection<Pessoa>(pessoasCarregadas);
        OnPropertyChanged(nameof(Pessoas));

        var pessoaNaLista = Pessoas.FirstOrDefault(p => p.Id == pessoa.Id);
        PessoaSelecionada = pessoaNaLista;
        OnPropertyChanged(nameof(PessoaSelecionada));

        FiltrarPedidos();
    }

    private void AbrirTelaPedido()
    {
        var janela = new PedidoView();
        janela.ShowDialog();
        FiltrarPedidos();
    }

    private void CarregarDados()
    {
        var lista = _service.Load();
        Pessoas = new ObservableCollection<Pessoa>(lista);
        OnPropertyChanged(nameof(Pessoas));
    }

    private void Filtrar()
    {
        var lista = _service.Load().Where(p =>
            (string.IsNullOrWhiteSpace(FiltroNome) || p.Nome.Contains(FiltroNome)) &&
            (string.IsNullOrWhiteSpace(FiltroCPF) || p.CPF.Contains(FiltroCPF))
        ).ToList();

        Pessoas.Clear();
        foreach (var pessoa in lista)
            Pessoas.Add(pessoa);
    }

    public void FiltrarPedidos()
    {
        if (PessoaSelecionada == null)
        {
            PedidosDaPessoa = new ObservableCollection<Pedido>();
            return;
        }

        var todos = _pedidoService.Load().Where(p => p.Pessoa.Id == PessoaSelecionada.Id);

        var filtrados = todos.Where(p =>
            (!MostrarEntregues || p.Status == StatusPedido.Recebido) &&
            (!MostrarPagos || p.Status == StatusPedido.Pago) &&
            (!MostrarPendentes || p.Status == StatusPedido.Pendente)
        );

        PedidosDaPessoa = new ObservableCollection<Pedido>(filtrados);
    }

    private void Incluir()
    {
        PessoaSelecionada = new Pessoa();
        OnPropertyChanged(nameof(PessoaSelecionada));
    }

    private void Salvar()
    {
        if (PessoaSelecionada == null)
        {
            MessageBox.Show("Nenhuma pessoa selecionada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!CpfValidator.IsValid(PessoaSelecionada.CPF))
        {
            MessageBox.Show("CPF inválido!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var lista = _service.Load();
        var existente = lista.FirstOrDefault(p => p.Id == PessoaSelecionada.Id);

        if (existente != null)
        {
            var index = lista.IndexOf(existente);
            lista[index] = PessoaSelecionada;
        }
        else
        {
            lista.Add(PessoaSelecionada);
        }

        _service.Save(lista);
        CarregarDados();
    }

    private void Excluir()
    {
        if (PessoaSelecionada == null)
        {
            MessageBox.Show("Nenhuma pessoa selecionada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var lista = _service.Load();
        lista.RemoveAll(p => p.Id == PessoaSelecionada.Id);
        _service.Save(lista);
        CarregarDados();
    }

    private void AtualizarStatus(object pedidoObj, StatusPedido novoStatus)
    {
        if (pedidoObj is Pedido pedido)
        {
            var pedidos = _pedidoService.Load();
            var atual = pedidos.FirstOrDefault(p => p.Id == pedido.Id);
            if (atual != null)
            {
                atual.Status = novoStatus;
                _pedidoService.Save(pedidos);
                FiltrarPedidos();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
