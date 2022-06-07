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

                    cmd.ExecuteNonQuery();

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

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

    }
}
