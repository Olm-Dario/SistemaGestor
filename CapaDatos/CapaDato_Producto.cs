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
    public class CapaDato_Producto
    {
        //Metodo que lista todos los Productos en la base de datos
        public List<Producto> listar()
        {
            //devuelve la lista de Productos
            List<Producto> lista = new List<Producto>();

            //Se conecta a la base de datos
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Guardamos la consulta en una variable
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select IdProducto, Codigo, Nombre, p.Descripcion, c.IdCategoria, c.Descripcion[DescripcionCategoria], Stock, PrecioCompra,PrecioVenta,p.Estado from PRODUCTO p");
                    query.AppendLine("inner join CATEGORIA c on c.IdCategoria = p.IdCategoria");


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
                            lista.Add(new Producto()
                            {
                                idProducto = Convert.ToInt32(dr["IdProducto"]),
                                codigo = dr["Codigo"].ToString(),
                                nombre = dr["Nombre"].ToString(),
                                descripcion = dr["Descripcion"].ToString(),
                                oCategoria = new Categoria() { idCategoria = Convert.ToInt32(dr["IdCategoria"]), descripcion = dr["DescripcionCategoria"].ToString() },
                                stock = Convert.ToInt32(dr["Stock"].ToString()),
                                precioCompra = Convert.ToDecimal(dr["PrecioCompra"].ToString()),
                                precioVenta = Convert.ToDecimal(dr["PrecioVenta"].ToString()),
                                estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    //En caso de error
                    lista = new List<Producto>();
                }
            }

            return lista;
        }


        public int Registrar(Producto obj, out string Mensaje)
        {
            int idProductoGenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_RegistrarProducto", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("Codigo", obj.codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.idCategoria);
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
                    idProductoGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                idProductoGenerado = 0;
                Mensaje = ex.Message;
            }

            return idProductoGenerado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_ModificarProducto", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdProducto", obj.idProducto);
                    cmd.Parameters.AddWithValue("Codigo", obj.codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.idCategoria);
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

        public bool Eliminar(Producto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    //le pasamos el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_EliminarProducto", conexion);

                    //Le pasamos los parametros de entrada
                    cmd.Parameters.AddWithValue("IdProducto", obj.idProducto);

                    //Le pasamos los parametros de salida
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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
