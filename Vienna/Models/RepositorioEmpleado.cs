using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class RepositorioEmpleado : RepositorioBase, IRepositorioEmpleado
    {
        public RepositorioEmpleado(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Empleado e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Empleado (Nombre, Apellido, Correo, Clave, EstadoEmpleado) " +
                    $"VALUES ('{e.Nombre}', '{e.Apellido}', '{e.Correo}', '{e.Clave}', 1) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    e.IdEmpleado = Convert.ToInt32(id);
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
                string sql = $"UPDATE Empleado SET EstadoEmpleado = 0 WHERE IdEmpleado = {id} ;";
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

        public int Modificacion(Empleado e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Empleado SET Nombre='{e.Nombre}', Apellido='{e.Apellido}', Correo='{e.Correo}', " +
                    $"Clave='{e.Clave}', EstadoEmpleado = 1 WHERE IdEmpleado = {e.IdEmpleado} ;";
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

        public Empleado ObtenerPorCorreo(string correo)
        {
            Empleado e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdEmpleado, Nombre, Apellido, Correo, Clave, EstadoEmpleado FROM Empleado" +
                    $" WHERE Correo=@correo AND EstadoEmpleado = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Empleado
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoEmpleado = reader.GetByte(5),
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public Empleado ObtenerPorId(int id)
        {
            Empleado e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdEmpleado, Nombre, Apellido, Correo, Clave, EstadoEmpleado FROM Empleado" +
                    $" WHERE IdEmpleado=@id AND EstadoEmpleado = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Empleado
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoEmpleado = reader.GetByte(5),
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public IList<Empleado> ObtenerPorNombreApellido(string nombre, string apellido)
        {
            List<Empleado> res = new List<Empleado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdEmpleado, Nombre, Apellido, Correo, Clave, EstadoEmpleado " +
                    $" FROM Empleado WHERE EstadoEmpleado = 1 AND Nombre=@nombre AND Apellido=@apellido ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = apellido;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Empleado e = new Empleado
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoEmpleado = reader.GetByte(5),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Empleado> ObtenerTodos()
        {
            List<Empleado> res = new List<Empleado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdEmpleado, Nombre, Apellido, Correo, Clave, EstadoEmpleado " +
                    $" FROM Empleado WHERE EstadoEmpleado = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Empleado e = new Empleado
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Clave = reader.GetString(4),
                            EstadoEmpleado = reader.GetByte(5),
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
