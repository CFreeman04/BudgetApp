//using Microsoft.Data.SqlClient;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BudgetAppDemo.Models
//{
//    internal class AllCategories
//    {
//        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();

//        public string CategoriesAsString => string.Join(", ", Categories);
//        public void LoadCategories()
//        {
//            Categories.Clear();

//            using (SqlConnection conn = new SqlConnection(App.ConnectionString)) {
//                conn.Open();

//                // Select the last 10 transactions to display
//                string query = "SELECT CategoryName " +
//                               "FROM Categories " +
//                               "ORDER BY CategoryName ";

//                using (SqlCommand cmd = new SqlCommand(query, conn)) {
//                    using (SqlDataReader reader = cmd.ExecuteReader()) {
//                        while (reader.Read()) { 
//                            Categories.Add(reader.GetString(0));
//                        }
//                    }
//                }
//                conn.Close();
//            }
//        }
//    }
//}
