using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class RepositorioInvitado : RepositorioBase, IRepositorioInvitado
    {
        public RepositorioInvitado(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Invitado i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Invitado (Nombre, Apellido, Dni, EstadoInvitado ) " +
                    $"VALUES ('{i.Nombre}', '{i.Apellido}', {i.Dni}, 1) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    i.IdInvitado = Convert.ToInt32(id);
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
                string sql = $"UPDATE Invitado SET EstadoInvitado = 0 WHERE IdInvitado = {id} ;";
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

        public int Modificacion(Invitado i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Invitado SET EstadoInvitado=1, Nombre='{i.Nombre}', Apellido='{i.Apellido}', " +
                    $"Dni={i.Dni} WHERE IdInvitado = {i.IdInvitado} ;";
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

        public Invitado ObtenerPorDni(decimal dni)
        {
            Invitado i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.IdInvitado, i.Nombre, i.Apellido, i.Dni, i.EstadoInvitado, " +
                    $" e.Nombre, e.InicioEvento, e.FinEvento, e.CantidadAsistentes, e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento " +
                    $" FROM Invitado i, Evento e " +
                    $" WHERE i.Dni=@dni AND i.EstadoInvitado = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", SqlDbType.Decimal).Value = dni;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Invitado
                        {
                            IdInvitado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetDecimal(3),
                            EstadoInvitado = reader.GetByte(4),
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public Invitado ObtenerPorId(int id)
        {
            Invitado i = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.IdInvitado, i.Nombre, i.Apellido, i.Dni, i.EstadoInvitado, " +
                    $" e.Nombre, e.InicioEvento, e.FinEvento, e.CantidadAsistentes, e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento " +
                    $" FROM Invitado i, Evento e " +
                    $" WHERE i.IdInvitado=@id AND i.EstadoInvitado = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Invitado
                        {
                            IdInvitado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetDecimal(3),
                            EstadoInvitado = reader.GetByte(4),
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }

        public IList<Invitado> ObtenerTodos()
        {
            List<Invitado> res = new List<Invitado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.IdInvitado, i.Nombre, i.Apellido, i.Dni, i.EstadoInvitado, " +
                    $" e.Nombre, e.InicioEvento, e.FinEvento, e.CantidadAsistentes, e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento " +
                    $" FROM Invitado i, Evento e " +
                    $" WHERE i.EstadoInvitado = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Invitado i = new Invitado
                        {
                            IdInvitado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetDecimal(3),
                            EstadoInvitado = reader.GetByte(4),
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
