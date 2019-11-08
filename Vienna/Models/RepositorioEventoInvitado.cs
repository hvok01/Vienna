using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class RepositorioEventoInvitado : RepositorioBase, IRepositorioEventoInvitado
    {
        public RepositorioEventoInvitado(IConfiguration configuracion) : base(configuracion)
        {

        }

        public int Alta(EventoInvitado t)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO EventoInvitado (IdEvento, IdInvitado) " +
                    $"VALUES ({t.IdEvento}, {t.IdInvitado}) ;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    t.IdInvitado = Convert.ToInt32(id);
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
                string sql = $"UPDATE EventoInvitado SET EstadoRelacion = 0 WHERE IdEvento = {id};";
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

        public int Baja(int id, int id2)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE EventoInvitado SET EstadoRelacion = 0 WHERE IdEvento = {id} AND IdInvitado = {id2};";
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

        public int Modificacion(EventoInvitado t)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE EventoInvitado SET EstadoRelacion=1, IdEvento={t.IdEvento}, " +
                    $"IdInvitado={t.IdInvitado} WHERE IdInvitado = {t.IdInvitado} AND IdEvento = {t.IdEvento} ;";
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

        public List<EventoInvitado> ObtenerActivos()
        {
            List<EventoInvitado> res = new List<EventoInvitado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT EstadoRelacion, IdInvitado, IdEvento FROM EventoInvitado WHERE EstadoRelacion = 1; ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EventoInvitado e = new EventoInvitado
                        {
                            EstadoRelacion = reader.GetByte(1),
                            IdInvitado = reader.GetInt16(2),
                            IdEvento = reader.GetInt16(3),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public EventoInvitado ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<EventoInvitado> ObtenerTodos()
        {
            List<EventoInvitado> res = new List<EventoInvitado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT EstadoRelacion, IdInvitado, IdEvento FROM EventoInvitado; ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EventoInvitado e = new EventoInvitado
                        {
                            EstadoRelacion = reader.GetByte(1),
                            IdInvitado = reader.GetInt16(2),
                            IdEvento = reader.GetInt16(3),
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
