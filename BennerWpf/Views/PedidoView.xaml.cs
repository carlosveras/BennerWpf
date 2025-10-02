using BennerWpf.ViewModels;
using System.Windows;

namespace BennerWpf.Views;

public partial class PedidoView : Window
{
    public PedidoView()
    {
        InitializeComponent();
        DataContext = new PedidoViewModel();
        WindowState = WindowState.Maximized;
    }

}
