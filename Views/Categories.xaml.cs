using Microsoft.Data.SqlClient;

namespace BudgetApp.Views;

public partial class Categories : ContentPage
{
	public Categories()
	{
		InitializeComponent();
		//BindingContext = new Models.AllCategories();
        BindingContext = new Models.AllObservables();

    }
	protected override void OnAppearing()
	{
        //((Models.AllCategories)BindingContext).LoadCategories();
        ((Models.AllObservables)BindingContext).LoadObservables();
        CategoryEntry.Text = "";
    }
    public async void AddCategory_Clicked(object sender, EventArgs e)
    {
        await using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
            conn.Open();

            // Add category if category doesnt already exist
            string query = "IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = @Category) " +
                           "BEGIN " +
                           "    INSERT INTO Categories (CategoryName) " +
                           "    VALUES (@Category); " +
                           "END;";

            using (SqlCommand cmd = new SqlCommand(query, conn)) {
                cmd.Parameters.AddWithValue("@Category", CategoryEntry.Text);
                cmd.ExecuteNonQuery();
            }
            
            conn.Close();
        }

        //await Shell.Current.GoToAsync("..");
        //CategoryEntry.Text = "";
        OnAppearing();
    }
    public async void DeleteCategory_Clicked(object sender, EventArgs e)
    {
        await using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
            conn.Open();

            // Delete category if category doesnt already exist
            string query = "IF EXISTS (SELECT 1 FROM Categories WHERE CategoryName = @Category) " +
                           "BEGIN " +
                           "    DELETE FROM Categories " +
                           "    WHERE CategoryName = @Category " +
                           "END;";

            using (SqlCommand cmd = new SqlCommand(query, conn)) {
                cmd.Parameters.AddWithValue("@Category", CategoryEntry.Text);
                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        //await Shell.Current.GoToAsync("..");
        //CategoryEntry.Text 
        OnAppearing();
    }
    public async void Cancel_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
    public void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0) { 
            var category = e.CurrentSelection[0];

            CategoryEntry.Text = category.ToString();
        }
    }
}