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
    public class CapaDato_Permisos
    {
        //Metodo que lista todos los permisos en la base de datos
        public List<Permiso> listar(int idUsuario)
        {
            //devuelve la lista de permisos
            List<Permiso> lista = new List<Permiso>();

            //Se conecta a la base de datos
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    //Trae los menus que tiene permitido ver el usuario segun su rol y permiso
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select p.IdRol, p.NombreMenu from PERMISO p");
                    query.AppendLine("inner join ROL r on r.IdRol = p.IdRol");
                    query.AppendLine("inner join USUARIO u on u.IdRol = r.IdRol");
                    query.AppendLine("where u.IdUsuario = @idUsuario");


                    //Ejecutamos la sentancia sql a la base
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    //Le decimos que interprete "@idUsuario" como el contenido de "idUsuario"
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

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
                            lista.Add(new Permiso()
                            {
                                oRol = new Rol() { idRol= Convert.ToInt32(dr["IdRol"]) },
                                nombreMenu = dr["nombreMenu"].ToString()
                            }) ;
                        }
                    }

                }
                catch (Exception ex)
                {
                    //En caso de error
                    lista = new List<Permiso>();
                }
            }

            return lista;

        }
    }
}