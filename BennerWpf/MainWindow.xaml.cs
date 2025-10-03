using BennerWpf.Views;
using System.Windows;

namespace BennerWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void AbrirPessoas_Click(object sender, RoutedEventArgs e)
        {
            var pessoas = new PessoaView();
            pessoas.ShowDialog();
            this.Close();

        }

        private void AbrirProdutos_Click(object sender, RoutedEventArgs e)
        {
            var produtos = new ProdutoView();
            this.Close();
            produtos.ShowDialog();

        }

        private void AbrirPedidos_Click(object sender, RoutedEventArgs e)
        {
            var pedidos = new PedidoView();
            this.Close(); 
            pedidos.ShowDialog();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
