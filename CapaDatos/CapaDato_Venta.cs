using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CapaDato_Venta
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) + 1 from VENTA");
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    idcorrelativo = 0;
                }
            }
            return idcorrelativo;
        }

        public bool RestarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update producto set stock = stock - @cantidad where idproducto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool SumarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update producto set stock = stock + @cantidad where idproducto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", conexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.idUsuario);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.tipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.numeroDocumento);
                    cmd.Parameters.AddWithValue("DocumentoCliente", obj.documentoCliente);
                    cmd.Parameters.AddWithValue("NombreCliente", obj.nombreCliente);
                    cmd.Parameters.AddWithValue("MontoPago", obj.montoPago);
                    cmd.Parameters.AddWithValue("MontoCambio", obj.montoCambio);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.montoTotal);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Respuesta = false;
                Mensaje = ex.Message;
            }

            return Respuesta;
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta obj = new Venta();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select v.IdVenta,u.Apellido,u.Nombre,");
                    query.AppendLine("v.DocumentoCliente,v.NombreCliente,");
                    query.AppendLine("v.TipoDocumento, v.NumeroDocumento,");
                    query.AppendLine("v.MontoPago, v.MontoCambio, v.MontoTotal,");
                    query.AppendLine("convert(char(10),v.FechaRegistro,103)[FechaRegistro]");
                    query.AppendLine("from VENTA v");
                    query.AppendLine("inner join USUARIO u on u.IdUsuario = v.IdUsuario");
                    query.AppendLine("where v.NumeroDocumento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Venta()
                            {
                                idVenta = int.Parse(dr["IdVenta"].ToString()),
                                oUsuario = new Usuario() { apellido = dr["Apellido"].ToString(), nombre = dr["Nombre"].ToString()  },
                                documentoCliente = dr["DocumentoCliente"].ToString(),
                                nombreCliente = dr["NombreCliente"].ToString(),
                                tipoDocumento = dr["TipoDocumento"].ToString(),
                                numeroDocumento = dr["NumeroDocumento"].ToString(),
                                montoPago = Convert.ToDecimal(dr["MontoPago"].ToString()),
                                montoCambio = Convert.ToDecimal(dr["MontoCambio"].ToString()),
                                montoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                                fechaRegistro = dr["FechaRegistro"].ToString()
                            };
                        }
                    }

                }
                catch (Exception ex)
                {
                    obj = new Venta();
                }
            }
                return obj;
        }

        public List<Detalle_Venta> ObtenerDetalleVenta(int idVenta)
        {
            List<Detalle_Venta> oLista = new List<Detalle_Venta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select p.Nombre, dv.PrecioVenta, dv.Cantidad, dv.SubTotal");
                    query.AppendLine("from DETALLE_VENTA dv");
                    query.AppendLine("inner join PRODUCTO p on p.IdProducto = dv.IdProducto");
                    query.AppendLine("where dv.IdVenta = @idventa");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idventa", idVenta);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Venta()
                            {
                                oProducto = new Producto() { nombre = dr["Nombre"].ToString() },
                                precioVenta = Convert.ToDecimal(dr["PrecioVenta"].ToString()),
                                cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                subTotal = Convert.ToDecimal(dr["SubTotal"].ToString())
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    oLista = new List<Detalle_Venta>();
                }
            }

            return oLista;
        }

    }
}
