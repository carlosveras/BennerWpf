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

    private void Inicio_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}
