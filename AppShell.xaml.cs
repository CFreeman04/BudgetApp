namespace BudgetApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.TransactionPage), typeof(Views.TransactionPage));
            Routing.RegisterRoute(nameof(Views.Categories), typeof(Views.Categories));
        }
    }
}
