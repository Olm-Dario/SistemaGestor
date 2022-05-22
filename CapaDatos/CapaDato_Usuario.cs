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
    public class CapaDato_Usuario
    {
        //Metodo que lista todos los usuarios en la base de datos
        public List<Usuario> listar()
        {
            //devuelve la lista de usuarios
            List<Usuario> lista = new List<Usuario>();

            //Se conecta a la base de datos
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Guardamos la consulta en una variable
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select u.IdUsuario,u.Documento,u.Nombre,u.Apellido,u.Correo,u.Clave,u.Estado,r.IdRol,r.Descripcion from usuario u");
                    query.AppendLine("inner join rol r on r.IdRol = u.IdRol");


                    //Ejecutamos la sentancia sql a la base
                    SqlCommand cmd = new SqlCommand(query.ToString(),conexion);

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
                            lista.Add(new Usuario()
                            {
                                idUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                documento = dr["Documento"].ToString(),
                                nombre = dr["Nombre"].ToString(),
                                apellido = dr["Apellido"].ToString(),
                                correo = dr["Correo"].ToString(),
                                clave = dr["Clave"].ToString(),
                                estado = Convert.ToBoolean(dr["Estado"]),
                                oRol = new Rol() { idRol = Convert.ToInt32(dr["IdRol"]), descripcion = dr["Descripcion"].ToString() }
                            });
                        }
                    }
                
                }
                catch (Exception ex)
                {
                    //En caso de error
                    lista = new List<Usuario>();
                }
            }

            return lista;
        }


        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idUsuarioGenerado = 0;
            Mensaje = string.Empty;

            try{

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARUSURARIO", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("Documento", obj.documento);
                    cmd.Parameters.AddWithValue("Nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("Apellido", obj.apellido);
                    cmd.Parameters.AddWithValue("Correo", obj.correo);
                    cmd.Parameters.AddWithValue("Clave", obj.clave);
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.idRol);
                    cmd.Parameters.AddWithValue("Estado", obj.estado);

                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    //Le que el tipo de comando es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Ejecutamos el comando
                    cmd.ExecuteNonQuery();

                    //Obtenemos los valores de los paramatros de salida despues de la ejecucion
                    idUsuarioGenerado = Convert.ToInt32(cmd.Parameters["IdUsuarioResultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                idUsuarioGenerado = 0;
                Mensaje = ex.Message;
            }

            return idUsuarioGenerado;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdUsuario", obj.idUsuario);
                    cmd.Parameters.AddWithValue("Documento", obj.documento);
                    cmd.Parameters.AddWithValue("Nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("Apellido", obj.apellido);
                    cmd.Parameters.AddWithValue("Correo", obj.correo);
                    cmd.Parameters.AddWithValue("Clave", obj.clave);
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.idRol);
                    cmd.Parameters.AddWithValue("Estado", obj.estado);

                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    //Le que el tipo de comando es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Ejecutamos el comando
                    cmd.ExecuteNonQuery();

                    //Obtenemos los valores de los paramatros de salida despues de la ejecucion
                    respuesta = Convert.ToBoolean(cmd.Parameters["IdUsuarioResultado"].Value);
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

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARUSUARIO", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdUsuario", obj.idUsuario);

                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    //Le que el tipo de comando es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Abrimos la cadena de conexion para que se ejecute el comando
                    conexion.Open();

                    //Ejecutamos el comando
                    cmd.ExecuteNonQuery();

                    //Obtenemos los valores de los paramatros de salida despues de la ejecucion
                    respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
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
