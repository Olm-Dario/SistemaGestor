using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CapaDato_Proveedor
    {

        //Metodo que lista todos los Proveedors en la base de datos
        public List<Proveedor> listar()
        {
            //devuelve la lista de Proveedors
            List<Proveedor> lista = new List<Proveedor>();

            //Se conecta a la base de datos
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Guardamos la consulta en una variable
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select IdProveedor,Documento,RazonSocial,Correo,Telefono,Estado from PROVEEDOR");

                    //Ejecutamos la sentancia sql a la base
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

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
                            lista.Add(new Proveedor()
                            {
                                idProveedor = Convert.ToInt32(dr["IdProveedor"]),
                                documento = dr["Documento"].ToString(),
                                razonSocial = dr["RazonSocial"].ToString(),
                                correo = dr["Correo"].ToString(),
                                telefono = dr["Telefono"].ToString(),
                                estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    //En caso de error
                    lista = new List<Proveedor>();
                }
            }

            return lista;
        }


        public int Registrar(Proveedor obj, out string Mensaje)
        {
            int idProveedorGenerado = 0;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProveedor", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("Documento", obj.documento);
                    cmd.Parameters.AddWithValue("RazonSocial", obj.razonSocial);
                    cmd.Parameters.AddWithValue("Correo", obj.correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.estado);

                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Le que el tipo de comando es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Ejecutamos el comando
                    cmd.ExecuteNonQuery();

                    //Obtenemos los valores de los paramatros de salida despues de la ejecucion
                    idProveedorGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                idProveedorGenerado = 0;
                Mensaje = ex.Message;
            }

            return idProveedorGenerado;
        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("sp_ModificarProveedor", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdProveedor", obj.idProveedor);
                    cmd.Parameters.AddWithValue("Documento", obj.documento);
                    cmd.Parameters.AddWithValue("RazonSocial", obj.razonSocial);
                    cmd.Parameters.AddWithValue("Correo", obj.correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.estado);

                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Le que el tipo de comando es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Ejecutamos el comando
                    cmd.ExecuteNonQuery();

                    //Obtenemos los valores de los paramatros de salida despues de la ejecucion
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }

        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("sp_EliminarProveedor", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdProveedor", obj.idProveedor);

                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //Le que el tipo de comando es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Ejecutamos el comando
                    cmd.ExecuteNonQuery();

                    //Obtenemos los valores de los paramatros de salida despues de la ejecucion
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }

    }
}
