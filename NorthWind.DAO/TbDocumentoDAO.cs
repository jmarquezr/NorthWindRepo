using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthWind.Entity;
using System.Data.SqlClient;
using System.Data;

namespace NorthWind.DAO
{
    public class TbDocumentoDAO
    {
        public eEstadoProceso GuardarDocumento(
           DocumentoBE oDocumentoDTO)
        {
            //GUARDA CABECERA
            string codigodocumentogenerado = "";
            var ConnectionString = @"Data Source=.;Initial Catalog=NorthWind;Integrated Security=SSPI";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("GuardarCab", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@codcliente", SqlDbType.Int).Value = oDocumentoDTO.Cabecera.Cliente.CodCliente;
                    cmd.Parameters.Add("@subtotal", SqlDbType.Int).Value = oDocumentoDTO.Cabecera.SubTotal;
                    cmd.Parameters.Add("@igv", SqlDbType.Int).Value = oDocumentoDTO.Cabecera.IGV;
                    cmd.Parameters.Add("@total", SqlDbType.Int, 50).Value = oDocumentoDTO.Cabecera.Total;
                    cmd.Parameters.Add("@fechahora", SqlDbType.SmallDateTime).Value = oDocumentoDTO.Cabecera.FechaHora;
                    cmd.Parameters.Add("@tipodocumento", SqlDbType.NVarChar, 50).Value = oDocumentoDTO.Cabecera.TipoDocumento.ToString();

                    //cmd.ExecuteNonQuery();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            codigodocumentogenerado = reader["CodigoGenerado"].ToString();
                        }
                    }
                }

                //GUARDA DETALLE
                foreach (var itemlista in oDocumentoDTO.Detalle)
                {

                    using (SqlCommand command = new SqlCommand("GuardarDET", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@coddocumento", SqlDbType.Int).Value = codigodocumentogenerado;
                        command.Parameters.Add("@codproducto", SqlDbType.Int).Value = itemlista.Producto.CodProducto;
                        command.Parameters.Add("@precio", SqlDbType.Int).Value = itemlista.Precio;
                        command.Parameters.Add("@cantidad", SqlDbType.Int).Value = itemlista.Cantidad;
                        command.Parameters.Add("@total", SqlDbType.Decimal).Value = itemlista.Total;

                        command.ExecuteNonQuery();
                    }


                }
                conn.Close();
            }

            return eEstadoProceso.Correcto;

            //Si todo esta OK se guarda como Correcto
            //return eEstadoProceso.Correcto;
        }
        public eEstadoProceso GuardarDocumentoTVP(DocumentoBE oDocumentoDTO)
        {
            //oDocumentoDTO.Cabecera.CodDocumento = "22";
            /*
            string codigodocumentogenerado = "";
            var ConnectionString = @"Data Source=.;Initial Catalog=NorthWind;Integrated Security=SSPI";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("GuardarCab", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@codcliente", SqlDbType.Int).Value = oDocumentoDTO.Cabecera.Cliente.CodCliente;
                    cmd.Parameters.Add("@subtotal", SqlDbType.Int).Value = oDocumentoDTO.Cabecera.SubTotal;
                    cmd.Parameters.Add("@igv", SqlDbType.Int).Value = oDocumentoDTO.Cabecera.IGV;
                    cmd.Parameters.Add("@total", SqlDbType.Int, 50).Value = oDocumentoDTO.Cabecera.Total;
                    cmd.Parameters.Add("@fechahora", SqlDbType.SmallDateTime).Value = oDocumentoDTO.Cabecera.FechaHora;
                    cmd.Parameters.Add("@tipodocumento", SqlDbType.NVarChar, 50).Value = oDocumentoDTO.Cabecera.TipoDocumento.ToString();

                    //cmd.ExecuteNonQuery();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            codigodocumentogenerado = reader["CodigoGenerado"].ToString();
                        }
                    }
              
               }
            }
            */
            var headers = new DataTable();
            //headers.Columns.Add("coddocumento", typeof(string));
            headers.Columns.Add("codcliente", typeof(string));
            headers.Columns.Add("subtotal", typeof(decimal));
            headers.Columns.Add("igv", typeof(decimal));
            headers.Columns.Add("total", typeof(decimal));
            headers.Columns.Add("fechahora", typeof(DateTime));
            headers.Columns.Add("tipodocumento", typeof(string));

            var details = new DataTable();
            details.Columns.Add("coddocumento", typeof(string));
            details.Columns.Add("codproducto", typeof(string));
            details.Columns.Add("precio", typeof(decimal));
            details.Columns.Add("cantidad", typeof(int));
            details.Columns.Add("total", typeof(decimal));

            headers.Rows.Add(new object[] 
                { 
                   //oDocumentoDTO.Cabecera.CodDocumento,
                   oDocumentoDTO.Cabecera.Cliente.CodCliente,
                   oDocumentoDTO.Cabecera.SubTotal,
                   oDocumentoDTO.Cabecera.IGV,
                   oDocumentoDTO.Cabecera.Total,
                   oDocumentoDTO.Cabecera.FechaHora,
                   oDocumentoDTO.Cabecera.TipoDocumento
                });


            string codigodocumentogenerado = "";
            var ConnectionString = @"Data Source=.;Initial Catalog=NorthWind;Integrated Security=SSPI";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("InsertaDocumento", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    var headersParam = cmd.Parameters.AddWithValue("@cabecera", headers);
                    headersParam.SqlDbType = SqlDbType.Structured;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            codigodocumentogenerado = reader["CodigoGenerado"].ToString();
                        }
                    }
                }

                conn.Close();
                foreach (ItemBE item in oDocumentoDTO.Detalle)
                {
                    details.Rows.Add(new object[] 
                        { 
                            codigodocumentogenerado,
                            item.Producto.CodProducto,
                            item.Precio,
                            item.Cantidad,
                            item.Total,
                        });
                }

                conn.Open();
                using (var cmd = new SqlCommand("InsertaDetalle", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    var detailsParam = cmd.Parameters.AddWithValue("@detalle", details);
                    detailsParam.SqlDbType = SqlDbType.Structured;

                    cmd.ExecuteNonQuery();
                }
                conn.Close();

                return eEstadoProceso.Correcto;


            }
        }
    }
}

//Guardar Factura aprpvechando caracteristicas de SQL 2008
//Usando Table-Valued Parameters (TVP)
//http://blog.sqlauthority.com/2008/08/31/sql-server-table-valued-parameters-in-sql-server-2008/
//http://www.sqlteam.com/article/sql-server-2008-table-valued-parameters
//http://lennilobel.wordpress.com/2009/07/29/sql-server-2008-table-valued-parameters-and-c-custom-iterators-a-match-made-in-heaven/
//http://www.adathedev.co.uk/2010/02/sql-server-2008-table-valued-parameters.html

//public eEstadoProceso GuardarDocumento(DocumentoBE oDocumentoDTO)
//{
//    oDocumentoDTO.Cabecera.CodDocumento = "22";
//    var headers = new DataTable();
//    headers.Columns.Add("coddocumento", typeof(string));
//    headers.Columns.Add("codcliente", typeof(string));
//    headers.Columns.Add("subtotal", typeof(decimal));
//    headers.Columns.Add("igv", typeof(decimal));
//    headers.Columns.Add("total", typeof(decimal));
//    headers.Columns.Add("fechahora", typeof(DateTime));
//    headers.Columns.Add("tipodocumento", typeof(string));

//    var details = new DataTable();
//    details.Columns.Add("coddocumento", typeof(string));
//    details.Columns.Add("codproducto", typeof(string));
//    details.Columns.Add("precio", typeof(decimal));
//    details.Columns.Add("cantidad", typeof(int));
//    details.Columns.Add("total", typeof(decimal));

//    headers.Rows.Add(new object[] 
//    { 
//       oDocumentoDTO.Cabecera.CodDocumento,
//       oDocumentoDTO.Cabecera.Cliente.CodCliente,
//       oDocumentoDTO.Cabecera.Subtotal,
//       oDocumentoDTO.Cabecera.IGV,
//       oDocumentoDTO.Cabecera.Total,
//       oDocumentoDTO.Cabecera.FechaHora,
//       oDocumentoDTO.Cabecera.TipoDocumento
//    });

//    foreach (ItemBE item in oDocumentoDTO.Detalle)
//    {
//        details.Rows.Add(new object[] 
//        { 
//            item.Producto.CodProducto,
//            item.Precio,
//            item.Cantidad,
//            item.Total,
//        });
//    }

//    var ConnectionString = @"Data Source=.;Initial Catalog=NorthWind;Integrated Security=SSPI";
//    using (var conn = new SqlConnection(ConnectionString))
//    {
//        conn.Open();
//        using (var cmd = new SqlCommand("InsertaDocumento", conn))
//        {
//            cmd.CommandType = CommandType.StoredProcedure;

//            var headersParam = cmd.Parameters.AddWithValue("@cabecera", headers);
//            var detailsParam = cmd.Parameters.AddWithValue("@detalle", details);

//            headersParam.SqlDbType = SqlDbType.Structured;
//            detailsParam.SqlDbType = SqlDbType.Structured;
//            cmd.ExecuteNonQuery();
//        }
//        conn.Close();
//    }
//    return eEstadoProceso.Correctamente;
//}
