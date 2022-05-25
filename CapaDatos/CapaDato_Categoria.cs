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
    public class CapaDato_Categoria
    {
        //Metodo que lista todos las Categorias en la base de datos
        public List<Categoria> listar()
        {
            //devuelve la lista de Categorias
            List<Categoria> lista = new List<Categoria>();

            //Se conecta a la base de datos
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Guardamos la consulta en una variable
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select IdCategoria, Descripcion, Estado from CATEGORIA");
                    

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
                            lista.Add(new Categoria()
                            {
                                idCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                descripcion = dr["Descripcion"].ToString(),
                                estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    //En caso de error
                    lista = new List<Categoria>();
                }
            }

            return lista;
        }


        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idCategoriaGenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCategoria", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("Descripcion", obj.descripcion);
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
                    idCategoriaGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                idCategoriaGenerado = 0;
                Mensaje = ex.Message;
            }

            return idCategoriaGenerado;
        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_EditarCategoria", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdCategoria", obj.idCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", obj.descripcion);
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

        public bool Eliminar(Categoria obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_EliminarCategoria", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdCategoria", obj.idCategoria);

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
