namespace BudgetApp
{
    public partial class App : Application
    {
        public static string ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=BudgetAppDB;Integrated Security=True;Trust Server Certificate=True";
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
