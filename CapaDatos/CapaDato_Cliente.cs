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
    public class CapaDato_Cliente
    {

        public List<Cliente> Listar()
        {

        List<Cliente> lista = new List<Cliente>();
        using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT IdCliente, Documento, Nombre, Apellido, Correo, Telefono, Estado, from Cliente");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            lista.Add(new Cliente()
                            {
                                idCliente = Convert.ToInt32(dr["IdCliente"]),
                                documento = dr["Documento"].ToString(),
                                nombre = dr["Nombre"].ToString(),
                                apellido = dr["Apellido"].ToString(),
                                correo = dr["Correo"].ToString(),
                                telefono = dr["Telefono"].ToString(),
                                estado = Convert.ToBoolean(dr["Estado"])
                            });
                                
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Cliente>();
                }
            }

            return lista;
        }
        
    }
}
