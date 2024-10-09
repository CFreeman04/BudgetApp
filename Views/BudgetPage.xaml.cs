using Microsoft.Data.SqlClient;

namespace BudgetApp.Views
{
    public partial class BudgetPage : ContentPage
    {
        public BudgetPage()
        {
            InitializeComponent();            
            BindingContext = new Models.AllObservables();     
        }
        protected override void OnAppearing()
        {
            // Refresh balance and latest transactions on page
            ((Models.AllObservables)BindingContext).LoadObservables();
            GetBalance();
        }
        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TransactionPage));
        }
        private async void Categories_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Categories));
        }
        private void OnAddTransactionClicked(object sender, EventArgs e)
        {
            if (DescriptionEntry.Text == null) {
                DisplayAlert("Error", "Description cannot be blank", "OK");
                return;
            }
            
            string description = DescriptionEntry.Text;
            decimal amount;

            if (!decimal.TryParse(AmountEntry.Text, out amount)) {
                DisplayAlert("Error", "Please enter a valid amount", "OK");
                return;
            }

            if (TypePicker.SelectedIndex  == 0) {
                DisplayAlert("Error", "Select a transaction type", "OK");
                return;
            }

            bool isIncome = TypePicker.SelectedItem.ToString() == "Income";

            amount = isIncome ? amount : -1 * amount;

            AddTransaction(description, amount, isIncome);
            //LoadTransactions();
        }
        private void AddTransaction(string description, decimal amount, bool isIncome)
        {
            using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
                conn.Open();
                string query = "INSERT INTO Transactions (Description, Amount, Category, Date, IsIncome) " +
                               "VALUES (@Description, @Amount, @Category, @Date, @IsIncome)";

                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@Category", "Home");
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IsIncome", isIncome);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            OnAppearing();
        }

        private async void Budget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count != 0) {
                // Get the Transaction model
                var currentTransaction = (Models.Transaction)e.CurrentSelection[0];

                
                await Navigation.PushAsync(new TransactionPage(currentTransaction));
            }
        }
        private void GetBalance()
        {
            decimal balance = 0.0m;

            using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
                conn.Open();
                
                string query = "SELECT " +
                               "   SUM(Amount) " +
                               "FROM Transactions";

                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            balance = reader.GetDecimal(0);
                        }
                    }
                }
                conn.Close();
            }

            BalanceLabel.Text = $"{balance:C2}";
        }
    }
}

