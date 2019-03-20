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
    public class ProductRepository : IRepository<Product>
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private string tableName = $"[dbo].[products]"; 

        public void Add(Product product, out string message)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string insertSql = $"INSERT INTO {tableName} ([Name],[PriceUSD]) VALUES(@productName, @productPriceUsd)";
                SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.AddWithValue("@productName", product.Name);
                command.Parameters.AddWithValue("@productPriceUsd", product.PriceUsd);
                command.ExecuteNonQuery();
            }
            message = $"Товар {product.Name} добавлен в базу данных";
        }

        public void Delete(int id, out string message)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteSql = $"DELETE FROM {tableName} WHERE [Id]=@id";
                SqlCommand command = new SqlCommand(deleteSql, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            message = $"Товар №{id} удален из базы данных";
        }

        public Product Read(int id)
        {
            Product product = null;

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string selectSql = $"SELECT * FROM {tableName} WHERE [Id]=@id";
                SqlCommand command = new SqlCommand(selectSql, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    object pId = reader.GetValue(0);
                    object pName = reader.GetValue(1);
                    object pPrice = reader.GetValue(2);

                    product = new Product
                    {
                        Id = Int32.Parse(pId.ToString()),
                        Name = pName.ToString(),
                        PriceUsd = Double.Parse(pPrice.ToString())
                    };
                }
            }            
            return product;
        }

        public IEnumerable<Product> ReadAll()
        {            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectSql = $"SELECT * FROM {tableName}";
                SqlCommand command = new SqlCommand(selectSql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object pId = reader.GetValue(0);
                        object pName = reader.GetValue(1);
                        object pPrice = reader.GetValue(2);
                        
                        yield return new Product
                        {
                            Id = Int32.Parse(pId.ToString()),
                            Name = pName.ToString(),
                            PriceUsd = Double.Parse(pPrice.ToString())
                        };
                    }                    
                }
            }
        }

        public void Update(int id, Product updated, out string message)
        {
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();
                string updateSql = $"UPDATE {tableName} SET [Name]=@productName, [PriceUSD]=@productPriceUsd WHERE [Id]=@id";
                SqlCommand command = new SqlCommand(updateSql, connection);
                command.Parameters.AddWithValue("@productName", updated.Name);
                command.Parameters.AddWithValue("@productPriceUsd", updated.PriceUsd);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            message = $"Параметры товара №{id} в базе данных обновлены";
        }
    }
}
