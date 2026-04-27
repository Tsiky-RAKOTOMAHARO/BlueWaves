using Avalonia.Controls;
using Avalonia.Interactivity;
using Core.Services;
using UI.Views;

namespace UI
{
    public partial class MainWindow : Window
    {
        private readonly ProduitServices _produitServices;

        public MainWindow(ProduitServices produitServices)
        {
            InitializeComponent();
            _produitServices = produitServices;
            NavigateTo("Dashboard");
        }

        private void NavClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string viewName)
            {
                if (btn.Parent is Panel parentPanel)
                {
                    foreach (var child in parentPanel.Children)
                    {
                        if (child is Button b)
                            b.Classes.Remove("active");
                    }
                }

                btn.Classes.Add("active");
                NavigateTo(viewName);
            }
        }

        private void NavigateTo(string viewName)
        {
            UserControl view = viewName switch
            {
                "Dashboard" => new DashboardView(),
                "Produit"   => new ProduitListView(),
                "Purchase"  => new PurchaseOrderView(),
                "Stock"     => new StockInventoryView(),
                "Directory" => new DirectoryView(),
                _           => new DashboardView()
            };

            MainContentPresenter.Content = view;
        }
    }
}