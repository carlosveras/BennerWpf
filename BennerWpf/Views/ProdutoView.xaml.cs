using BennerWpf.ViewModels;
using System.Windows;

namespace BennerWpf.Views
{
    public partial class ProdutoView : Window
    {
        public ProdutoView()
        {
            InitializeComponent();
            DataContext = new ProdutoViewModel();
            WindowState = WindowState.Maximized;
        }

        private void Filtrar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProdutoViewModel vm)
                vm.Filtrar();
        }

    }
}
