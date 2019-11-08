using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Contrato (EstadoContrato, PrecioFinal, Pagado, IdEvento) " +
                    $"VALUES (1, {c.PrecioFinal}, {c.PrecioFinal}, {c.Pagado}, {c.IdEvento}) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    c.IdContrato = Convert.ToInt32(id);
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
                string sql = $"UPDATE Contrato SET EstadoContrato = 0 WHERE IdContrato = {id} ;";
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

        public int Modificacion(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Contrato SET EstadoContrato=1, PrecioFinal={c.PrecioFinal}, " +
                    $"IdEvento={c.IdEvento}, Pagado = {c.Pagado} WHERE IdContrato = {c.IdContrato} ;";
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

        public Contrato ObtenerPorEvento(string nombre)
        {
            Contrato c = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.IdContrato, c.PrecioFinal, c.IdEvento, c.Pagado, c.EstadoContrato " +
                    $" e.Nombre, e.InicioEvento, e.FinEvento , e.CantidadAsistentes, e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento " +
                    $" FROM Contrato c, Evento e " +
                    $" WHERE e.Nombre =@nombre AND e.IdEvento = c.IdEvento " +
                    $" AND c.EstadoContrato = 1 AND e.EstadoEvento = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            PrecioFinal = reader.GetDecimal(1),
                            IdEvento = reader.GetInt16(2),
                            Pagado = reader.GetByte(3),
                            EstadoContrato = reader.GetByte(4),
                            Evento = new Evento
                            {
                                Nombre = reader.GetString(5),
                                InicioEvento = reader.GetDateTime(6),
                                FinEvento = reader.GetDateTime(7),
                                CantidadAsistentes = reader.GetInt16(8),
                                PrecioEntrada = reader.GetByte(9),
                                EstadoEvento = reader.GetByte(10),
                                IdSala = reader.GetInt16(11),
                                IdDuenioEvento = reader.GetInt16(12),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato c = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.IdContrato, c.PrecioFinal, c.IdEvento, c.Pagado, c.EstadoContrato " +
                    $" e.Nombre, e.InicioEvento, e.FinEvento, e.CantidadAsistentes, e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento " +
                    $" FROM Contrato c, Evento e " +
                    $" WHERE e.IdContrato =@id AND e.IdEvento = c.IdEvento " +
                    $" AND c.EstadoContrato = 1 AND e.EstadoEvento = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            PrecioFinal = reader.GetDecimal(1),
                            IdEvento = reader.GetInt16(2),
                            Pagado = reader.GetByte(3),
                            EstadoContrato = reader.GetByte(4),
                            Evento = new Evento
                            {
                                Nombre = reader.GetString(5),
                                InicioEvento = reader.GetDateTime(6),
                                FinEvento = reader.GetDateTime(7),
                                CantidadAsistentes = reader.GetInt16(8),
                                PrecioEntrada = reader.GetByte(9),
                                EstadoEvento = reader.GetByte(10),
                                IdSala = reader.GetInt16(11),
                                IdDuenioEvento = reader.GetInt16(12),
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }

        public IList<Contrato> ObtenerTodos()
        {
            List<Contrato> res = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.IdContrato, c.PrecioFinal, c.IdEvento, c.Pagado, c.EstadoContrato " +
                    $" e.Nombre, e.InicioEvento, e.FinEvento , e.CantidadAsistentes, e.PrecioEntrada, e.EstadoEvento, e.IdSala, e.IdDuenioEvento " +
                    $" FROM Contrato c, Evento e " +
                    $" WHERE e.IdEvento = c.IdEvento " +
                    $" AND c.EstadoContrato = 1 AND e.EstadoEvento = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato c = new Contrato
                        {
                            IdContrato = reader.GetInt32(0),
                            PrecioFinal = reader.GetDecimal(1),
                            IdEvento = reader.GetInt16(2),
                            Pagado = reader.GetByte(3),
                            EstadoContrato = reader.GetByte(4),
                            Evento = new Evento
                            {
                                Nombre = reader.GetString(5),
                                InicioEvento = reader.GetDateTime(6),
                                FinEvento = reader.GetDateTime(7),
                                CantidadAsistentes = reader.GetInt16(8),
                                PrecioEntrada = reader.GetByte(9),
                                EstadoEvento = reader.GetByte(10),
                                IdSala = reader.GetInt16(11),
                                IdDuenioEvento = reader.GetInt16(12),
                            }
                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
