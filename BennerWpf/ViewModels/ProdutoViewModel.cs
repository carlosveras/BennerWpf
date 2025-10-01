using BennerWpf.Models;
using BennerWpf.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

#nullable disable

namespace BennerWpf.ViewModels;

public class ProdutoViewModel : INotifyPropertyChanged
{
    private readonly DataService<Produto> _service = new("produtos.json");

    public ObservableCollection<Produto> Produtos { get; set; } = new();
    public Produto ProdutoSelecionado { get; set; } = new();

    public string FiltroNome { get; set; }
    public string FiltroCodigo { get; set; }
    public decimal? ValorMinimo { get; set; }
    public decimal? ValorMaximo { get; set; }

    public ICommand IncluirCommand { get; }
    public ICommand SalvarCommand { get; }
    public ICommand ExcluirCommand { get; }

    public ProdutoViewModel()
    {
        CarregarDados();

        IncluirCommand = new RelayCommand(_ => Incluir());
        SalvarCommand = new RelayCommand(_ => Salvar());
        ExcluirCommand = new RelayCommand(_ => Excluir());
    }

    private void CarregarDados()
    {
        var lista = _service.Load();
        Produtos = new ObservableCollection<Produto>(lista);
        OnPropertyChanged(nameof(Produtos));
    }

    public void Filtrar()
    {
        var lista = _service.Load().Where(p =>
            (string.IsNullOrWhiteSpace(FiltroNome) || p.Nome.Contains(FiltroNome)) &&
            (string.IsNullOrWhiteSpace(FiltroCodigo) || p.Codigo.Contains(FiltroCodigo)) &&
            (!ValorMinimo.HasValue || p.Valor >= ValorMinimo.Value) &&
            (!ValorMaximo.HasValue || p.Valor <= ValorMaximo.Value)
        ).ToList();

        Produtos = new ObservableCollection<Produto>(lista);
        OnPropertyChanged(nameof(Produtos));
    }

    private void Incluir()
    {
        ProdutoSelecionado = new Produto();
        OnPropertyChanged(nameof(ProdutoSelecionado));
    }

    private void Salvar()
    {
        var lista = _service.Load();
        var existente = lista.FirstOrDefault(p => p.Id == ProdutoSelecionado.Id);

        if (existente != null)
        {
            var index = lista.IndexOf(existente);
            lista[index] = ProdutoSelecionado;
        }
        else
        {
            lista.Add(ProdutoSelecionado);
        }

        _service.Save(lista);
        CarregarDados();
    }

    private void Excluir()
    {
        var lista = _service.Load();
        lista.RemoveAll(p => p.Id == ProdutoSelecionado.Id);
        _service.Save(lista);
        CarregarDados();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
