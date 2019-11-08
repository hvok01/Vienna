using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class RepositorioDuenio : RepositorioBase, IRepositorioDuenio
    {
        public RepositorioDuenio(IConfiguration configuracion) : base(configuracion)
        {

        }
        public int Alta(DuenioEvento e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO DuenioEvento (Nombre, Apellido, Correo, Clave, EstadoDuenio) " +
                    $"VALUES ('{e.Nombre}', '{e.Apellido}', '{e.Correo}', '{e.Clave}', 1) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    e.IdDuenioEvento = Convert.ToInt32(id);
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE DuenioEvento SET EstadoDuenio = 0 WHERE IdDuenioEvento = {id} ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int Modificacion(DuenioEvento e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE DuenioEvento SET Nombre='{e.Nombre}', Apellido='{e.Apellido}', Correo='{e.Correo}', " +
                    $"Clave='{e.Clave}', EstadoDuenio = 1 WHERE IdDuenioEvento = {e.IdDuenioEvento} ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public DuenioEvento ObtenerPorCorreo(string correo)
        {
            DuenioEvento e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdDuenioEvento, Nombre, Apellido, Correo, Clave, EstadoDuenio FROM DuenioEvento" +
                    $" WHERE Correo=@correo AND EstadoDuenio = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new DuenioEvento
                        {
                            IdDuenioEvento = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoDuenio = reader.GetByte(5),
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public DuenioEvento ObtenerPorId(int id)
        {
            DuenioEvento e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdDuenioEvento, Nombre, Apellido, Correo, Clave, EstadoDuenio FROM DuenioEvento" +
                    $" WHERE IdDuenioEvento=@id AND EstadoDuenio = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new DuenioEvento
                        {
                            IdDuenioEvento = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoDuenio = reader.GetByte(5),
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public IList<DuenioEvento> ObtenerTodos()
        {
            List<DuenioEvento> res = new List<DuenioEvento>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdDuenioEvento, Nombre, Apellido, Correo, Clave, EstadoDuenio " +
                    $" FROM DuenioEvento WHERE EstadoDuenio = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DuenioEvento e = new DuenioEvento
                        {
                            IdDuenioEvento = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoDuenio = reader.GetByte(5),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<DuenioEvento> ObtenerPorNombreApellido(string nombre, string apellido)
        {
            List<DuenioEvento> res = new List<DuenioEvento>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdDuenioEvento, Nombre, Apellido, Correo, Clave, EstadoDuenio " +
                    $" FROM DuenioEvento WHERE EstadoDuenio = 1 AND Nombre = @nombre AND  Apellido = @apellido;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = apellido;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DuenioEvento e = new DuenioEvento
                        {
                            IdDuenioEvento = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoDuenio = reader.GetByte(5),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }
            }
            return res;
        }

    }
}
