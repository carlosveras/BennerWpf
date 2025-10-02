using BennerWpf.ViewModels;
using System.Windows;

#nullable disable

namespace BennerWpf.Views;

public partial class PessoaView : Window
{
    public PessoaView()
    {
        InitializeComponent();
        WindowState = WindowState.Maximized;
        DataContext = new PessoaViewModel();
    }

    private void AbrirPedido_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is PessoaViewModel vm && vm.PessoaSelecionada != null)
        {
            var pedidoWindow = new PedidoView();
            var pedidoVM = pedidoWindow.DataContext as PedidoViewModel;

            pedidoVM.Inicializar(vm.PessoaSelecionada, true);

            pedidoWindow.ShowDialog();
            vm.FiltrarPedidos();
        }
    }
}
