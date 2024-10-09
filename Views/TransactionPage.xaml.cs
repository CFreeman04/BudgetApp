using Microsoft.Data.SqlClient;
using System.ComponentModel;

namespace BudgetApp.Views;

public partial class TransactionPage : ContentPage
{
	public TransactionPage(Models.Transaction singleTransaction)
	{
        InitializeComponent();

        //BindingContext = singleTransaction;        
        BindingContext = new Models.AllObservables();

        IDEntry.Text = $"{ singleTransaction.Id}";
        DescriptionEntry.Text = singleTransaction.Description;
        AmountEntry.Text = $"{singleTransaction.Amount}";
        CategoryEntry.SelectedItem = singleTransaction.Category; // DOESN'T WORK
        DateEntry.Date = singleTransaction.Date;
        
    }
    public TransactionPage()
    {
        InitializeComponent();
        BindingContext = new Models.AllObservables();

        TopName.Text = "Add Transaction";
        AddUpdateButton.Text = "Add";
        DeleteButton.IsVisible = false;
    }
    protected override void OnAppearing()
    {
        ((Models.AllObservables)BindingContext).LoadObservables();
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        bool isIncome = TypePicker.SelectedItem.ToString() == "Income";
        decimal amount = isIncome ? Math.Abs(decimal.Parse(AmountEntry.Text)) : -1 * Math.Abs(decimal.Parse(AmountEntry.Text));
        
        using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
            conn.Open();

            string query = "UPDATE Transactions " +
                           $"SET Description=@Description, Amount=@Amount, Category=@Category, Date=@Date, IsIncome=@IsIncome " +
                           $"WHERE Id=@ID";

            using (SqlCommand cmd = new SqlCommand(query, conn)) {
                cmd.Parameters.AddWithValue("@ID", IDEntry.Text);
                cmd.Parameters.AddWithValue("@Description", DescriptionEntry.Text);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Category", CategoryEntry.SelectedItem);
                cmd.Parameters.AddWithValue("@Date", DateEntry.Date);
                cmd.Parameters.AddWithValue("@IsIncome", TypePicker.SelectedItem.ToString() == "Income");
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
            await Shell.Current.GoToAsync("..");
    }
    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
            conn.Open();

            string query = "DELETE " +
                           "FROM Transactions " +
                           "WHERE Id=@ID;";

            using (SqlCommand cmd = new SqlCommand(query, conn)) {
                cmd.Parameters.AddWithValue("@ID", IDEntry.Text);
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            await Shell.Current.GoToAsync("..");
        }
    }
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
    //private void UpdateDatabase(Models.Transaction transaction, string query)
    //{

        
    //}
}