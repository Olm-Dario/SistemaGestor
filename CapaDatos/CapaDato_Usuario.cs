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

    }
}
