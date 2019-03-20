using AdoQRCode.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoQRCode.Repositories
{
    public class PurchaseRepository : IRepository<Purchase>
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private string tableName = $"[dbo].[purchases]";

        public void Add(Purchase purchase, out string message)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string insertSql = $"INSERT INTO {tableName} ([purchaseGUID],[purchaseDate],[productId],[customerName],[purchaseQR],[shippingQR])" +
                    $"VALUES (@purchaseGUID, @purchaseDate, @productId, @customerName, @purchaseQR, @shippingQR)";
                SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.AddWithValue("@purchaseGUID", purchase.PurchaseGuid);
                command.Parameters.AddWithValue("@purchaseDate", purchase.PurchaseDate);
                command.Parameters.AddWithValue("@productId", purchase.ProductId);
                command.Parameters.AddWithValue("@customerName", purchase.CustomerName);
                command.Parameters.AddWithValue("@purchaseQR", purchase.PurchaseQr);
                command.Parameters.AddWithValue("@shippingQR", purchase.ShippingQr);
                command.ExecuteNonQuery();
            }
            message = $"Покупка №{purchase.PurchaseGuid} успешно добавлена в базу данных";
        }

        public void Delete(int purchaseId, out string message)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteSql = $"DELETE FROM {tableName} WHERE [Id]=@id";
                SqlCommand command = new SqlCommand(deleteSql, connection);
                command.Parameters.AddWithValue("@id", purchaseId);
                command.ExecuteNonQuery();
            }
            message = $"Покупка №{purchaseId} успешно удалена из базы данных";
        }

        public Purchase Read(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Purchase> ReadAll()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Purchase updated, out string message)
        {
            throw new NotImplementedException();
        }
    }
}
