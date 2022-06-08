using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;

namespace CapaDatos
{
    public class CapaDato_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idCorrelativo = 0;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Guardamos la consulta en una variable
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select count(*) + 1 from COMPRA");

                    //Ejecutamos la sentancia sql a la base
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    //Le decimos que es un texto ya que "query" es un string
                    cmd.CommandType = CommandType.Text;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Tomamos el valor que nos da la consulta
                    idCorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    //En caso de error
                    idCorrelativo = 0;
                }
            }

            return idCorrelativo;
        }
        
        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Ejecutamos la sentancia sql a la base
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCompra", conexion);

                    cmd.Parameters.AddWithValue("IdUsuario",obj.oUsuario.idUsuario);
                    cmd.Parameters.AddWithValue("IdProveedor", obj.oProveedor.idProveedor);
                    cmd.Parameters.AddWithValue("TipoDocumento",obj.tipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento",obj.numeroDocumento);
                    cmd.Parameters.AddWithValue("MontoTotal",obj.montoTotal);
                    cmd.Parameters.AddWithValue("DetalleCompra",DetalleCompra);
                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Le decimos que es un texto ya que "query" es un string
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();
                    
                    cmd.ExecuteNonQuery();

                    //Obtenemos los valores de los paramatros de salida despues de la ejecucion
                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    //En caso de error
                    Respuesta = false;
                    Mensaje = ex.Message;
                }
            }

            return Respuesta;
        }

        public Compra ObtenerCompra(string numero)
        {
            Compra obj = new Compra();

            //Se conecta a la base de datos
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Guardamos la consulta en una variable
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select c.IdCompra,u.Apellido,u.Nombre,pr.Documento,pr.RazonSocial,");
                    query.AppendLine("c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,convert(char(10), c.FechaRegistro, 103)[FechaRegistro]");
                    query.AppendLine("from COMPRA c");
                    query.AppendLine("inner join USUARIO u on u.IdUsuario = c.IdUsuario");
                    query.AppendLine("inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor");
                    query.AppendLine("where c.NumeroDocumento = @numero");

                    //Ejecutamos la sentancia sql a la base
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@numero", numero);

                    //Le decimos que es un texto ya que "query" es un string
                    cmd.CommandType = CommandType.Text;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Se lee el comando sql
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Cada vez que lee lo guarda en la lista
                        while (dr.Read())
                        {
                            obj = new Compra()
                            {
                                idCompra = Convert.ToInt32(dr["IdCompra"]),
                                oUsuario = new Usuario() { apellido = dr["Apellido"].ToString(), nombre = dr["Nombre"].ToString() },
                                oProveedor = new Proveedor() { documento = dr["Documento"].ToString(), razonSocial = dr["RazonSocial"].ToString() },
                                tipoDocumento = dr["TipoDocumento"].ToString(),
                                numeroDocumento = dr["NumeroDocumento"].ToString(),
                                montoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                                fechaRegistro = dr["FechaRegistro"].ToString()
                            };
                        }
                    }

                }
                catch (Exception ex)
                {
                    //En caso de error
                    obj = new Compra();
                }
            }

            return obj;
        }

    }
}
