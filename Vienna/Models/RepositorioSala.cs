using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class RepositorioSala : RepositorioBase, IRepositorioSala
    {
        public RepositorioSala(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Sala s)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Sala (CapacidadMaxima, Nombre, Disponibilidad, EstadoSala) " +
                    $"VALUES ({s.CapacidadMaxima}, '{s.Nombre}', {s.Disponibilidad}, {s.EstadoSala}) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    s.IdSala = Convert.ToInt32(id);
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
                string sql = $"UPDATE Sala SET EstadoSala = 0 WHERE IdSala = {id} ;";
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

        public int Modificacion(Sala s)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Sala SET CapacidadMaxima = {s.CapacidadMaxima}, Nombre ='{s.Nombre}', Disponibilidad= {s.Disponibilidad}, " +
                    $"EstadoSala= 1 WHERE IdSala = {s.IdSala} ;";
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

        public Sala ObtenerPorId(int id)
        {
            Sala s = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdSala, CapacidadMaxima, Nombre, Disponibilidad, EstadoSala FROM Sala " +
                    $" WHERE IdSala=@id AND EstadoSala = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        s = new Sala
                        {
                            IdSala = reader.GetInt32(0),
                            CapacidadMaxima = reader.GetInt16(1),
                            Nombre = reader.GetString(2),
                            Disponibilidad = reader.GetByte(3),
                            EstadoSala = reader.GetByte(4)
                        };
                    }
                    connection.Close();
                }
            }
            return s;
        }

        public Sala ObtenerPorNombre(string nombre)
        {
            Sala s = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdSala, CapacidadMaxima, Nombre, Disponibilidad, EstadoSala FROM Sala " +
                    $" WHERE Nombre=@nombre AND EstadoSala = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        s = new Sala
                        {
                            IdSala = reader.GetInt32(0),
                            CapacidadMaxima = reader.GetInt16(1),
                            Nombre = reader.GetString(2),
                            Disponibilidad = reader.GetByte(3),
                            EstadoSala = reader.GetByte(4)
                        };
                    }
                    connection.Close();
                }
            }
            return s;
        }

        public IList<Sala> ObtenerTodos()
        {
            List<Sala> res = new List<Sala>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IdSala, CapacidadMaxima, Nombre, Disponibilidad, EstadoSala FROM Sala " +
                    $" WHERE EstadoSala = 1 ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Sala s = new Sala
                        {
                            IdSala = reader.GetInt32(0),
                            CapacidadMaxima = reader.GetInt16(1),
                            Nombre = reader.GetString(2),
                            Disponibilidad = reader.GetByte(3),
                            EstadoSala = reader.GetByte(4)
                        };
                        res.Add(s);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
