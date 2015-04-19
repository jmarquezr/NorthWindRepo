using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthWind.Entity;

namespace NorthWind.DAO
{
    public class TbCategoriaDAO
    {
        public static List<TbCategoriaBE> SelectAll()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["NorthWind"].ToString();
            string sql = "SELECT CategoryID, CategoryName FROM Categories";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    List<TbCategoriaBE> Productos = new List<TbCategoriaBE>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TbCategoriaBE objTbProducto = new TbCategoriaBE();
                            objTbProducto.CodCategoria = reader.GetInt32(0);
                            objTbProducto.NomCategoria = reader.GetString(1);                            
                            Productos.Add(objTbProducto);
                        }
                    }
                    connection.Close();
                    return Productos;
                }
            }
        }
    }
}
