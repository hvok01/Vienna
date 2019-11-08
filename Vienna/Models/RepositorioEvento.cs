using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class RepositorioEvento : RepositorioBase, IRepositorioEvento
    {
        public RepositorioEvento(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Evento e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Evento (Nombre, InicioEvento, CantidadAsistentes, PrecioEntrada, " +
                    $" EstadoEvento, IdSala, IdDuenioEvento, FinEvento) " +
                    $"VALUES ('{e.Nombre}', '{e.InicioEvento}', {e.CantidadAsistentes},{e.PrecioEntrada}, 1, " +
                    $" {e.IdSala}, {e.IdDuenioEvento}, {e.FinEvento}) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    e.IdSala = Convert.ToInt32(id);
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
                string sql = $"UPDATE Evento SET EstadoEvento = 0 WHERE IdSala = {id} ;";
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

        public int Modificacion(Evento e)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Evento SET Nombre='{e.Nombre}', InicioEvento='{e.InicioEvento}', CantidadAsistentes={e.CantidadAsistentes}, " +
                    $" PrecioEntrada={e.PrecioEntrada}, EstadoEvento = 1, IdSala = {e.IdSala}, IdDuenioEvento = {e.IdDuenioEvento}, FinEvento = {e.FinEvento} " +
                    $" WHERE IdEvento = {e.IdEvento} ;";
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

        public Evento ObtenerPorId(int id)
        {
            Evento e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT e.IdEvento, e.Nombre, e.InicioEvento, e.CantidadAsistentes, " +
                    $" e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento, e.FinEvento, " +
                    $" s.CapacidadMaxima, s.Nombre, s.Disponibilidad, s.EstadoSala " +
                    $" d.Nombre, d.Apellido, d.Correo, d.EstadoDuenio FROM Evento e, DuenioEvento d, Sala s" +
                    $" WHERE e.IdEvento=@id AND e.EstadoEvento = 1 AND e.IdDuenioEvento = d.IdDuenioEvento " +
                    $" AND e.IdSala = s.IdSala;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Evento
                        {
                            IdEvento = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            InicioEvento = reader.GetDateTime(2),
                            CantidadAsistentes = reader.GetInt16(3),
                            PrecioEntrada = reader.GetDecimal(4),
                            EstadoEvento = reader.GetByte(5),
                            IdSala = reader.GetInt16(6),
                            IdDuenioEvento = reader.GetInt16(7),
                            FinEvento = reader.GetDateTime(8),
                            Sala = new Sala
                            {
                                CapacidadMaxima = reader.GetInt16(9),
                                Nombre = reader.GetString(10),
                                Disponibilidad = reader.GetByte(11),
                                EstadoSala = reader.GetByte(12),
                            },
                            DuenioEvento = new DuenioEvento 
                            {
                                Nombre = reader.GetString(13),
                                Apellido = reader.GetString(14),
                                Correo = reader.GetString(15),
                                EstadoDuenio = reader.GetByte(16),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public Evento ObtenerPorNombre(string nombre)
        {
            Evento e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT e.IdEvento, e.Nombre, e.InicioEvento, e.CantidadAsistentes, " +
                    $" e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento, e.FinEvento, " +
                    $" s.CapacidadMaxima, s.Nombre, s.Disponibilidad, s.EstadoSala " +
                    $" d.Nombre, d.Apellido, d.Correo, d.EstadoDuenio FROM Evento e, DuenioEvento d, Sala s" +
                    $" WHERE e.Nombre=@nombre AND e.EstadoEvento = 1 AND e.IdDuenioEvento = d.IdDuenioEvento " +
                    $" AND e.IdSala = s.IdSala;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Evento
                        {
                            IdEvento = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            InicioEvento = reader.GetDateTime(2),
                            CantidadAsistentes = reader.GetInt16(3),
                            PrecioEntrada = reader.GetDecimal(4),
                            EstadoEvento = reader.GetByte(5),
                            IdSala = reader.GetInt16(6),
                            IdDuenioEvento = reader.GetInt16(7),
                            FinEvento = reader.GetDateTime(8),
                            Sala = new Sala
                            {
                                CapacidadMaxima = reader.GetInt16(9),
                                Nombre = reader.GetString(10),
                                Disponibilidad = reader.GetByte(11),
                                EstadoSala = reader.GetByte(12),
                            },
                            DuenioEvento = new DuenioEvento
                            {
                                Nombre = reader.GetString(13),
                                Apellido = reader.GetString(14),
                                Correo = reader.GetString(15),
                                EstadoDuenio = reader.GetByte(16),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public IList<Evento> ObtenerTodos()
        {
            List<Evento> res = new List<Evento>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT e.IdEvento, e.Nombre, e.InicioEvento, e.CantidadAsistentes, " +
                    $" e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento, e.FinEvento, " +
                    $" s.CapacidadMaxima, s.Nombre, s.Disponibilidad, s.EstadoSala " +
                    $" d.Nombre, d.Apellido, d.Correo, d.EstadoDuenio FROM Evento e, DuenioEvento d, Sala s" +
                    $" WHERE e.EstadoEvento = 1 AND e.IdDuenioEvento = d.IdDuenioEvento " +
                    $" AND e.IdSala = s.IdSala;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Evento e = new Evento
                        {
                            IdEvento = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            InicioEvento = reader.GetDateTime(2),
                            CantidadAsistentes = reader.GetInt16(3),
                            PrecioEntrada = reader.GetDecimal(4),
                            EstadoEvento = reader.GetByte(5),
                            IdSala = reader.GetInt16(6),
                            IdDuenioEvento = reader.GetInt16(7),
                            FinEvento = reader.GetDateTime(8),
                            Sala = new Sala
                            {
                                CapacidadMaxima = reader.GetInt16(9),
                                Nombre = reader.GetString(10),
                                Disponibilidad = reader.GetByte(11),
                                EstadoSala = reader.GetByte(12),
                            },
                            DuenioEvento = new DuenioEvento
                            {
                                Nombre = reader.GetString(13),
                                Apellido = reader.GetString(14),
                                Correo = reader.GetString(15),
                                EstadoDuenio = reader.GetByte(16),
                            }
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
