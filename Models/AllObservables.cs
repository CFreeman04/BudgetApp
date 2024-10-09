using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace BudgetApp.Models
{
    internal class AllObservables
    {
        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();
        public ObservableCollection<string> TestCategories { get; set; } = new ObservableCollection<string>();

        public void LoadObservables()
        {
            LoadTransactions();
            LoadCategories();
        }
        public void LoadTransactions()
        {
            Transactions.Clear();

            using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
                conn.Open();

                // Select the last 20 transactions to display
                string query = "SELECT TOP 20 *" +
                               "FROM Transactions " +
                               "ORDER BY ID DESC ";

                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Transactions.Add(new Transaction
                            {
                                Id = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                Amount = reader.GetDecimal(2),
                                Category = reader.GetString(3),
                                Date = reader.GetDateTime(4),
                                IsIncome = reader.GetBoolean(5)
                            });
                        }
                    }
                }
                conn.Close();
            }
            LoadCategories();
        }
        public void LoadCategories()
        {
            TestCategories.Clear();

            using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
                conn.Open();

                // Get a list of Categories from Categories tabley
                string query = "SELECT CategoryName " +
                               "FROM Categories " +
                               "ORDER BY CategoryName ";

                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            TestCategories.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}
