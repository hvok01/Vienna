using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vienna.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<DuenioEvento> DuenioEvento { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<Sala> Sala { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<Invitado> Invitado { get; set; }
        public DbSet<EventoInvitado> EventoInvitado { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventoInvitado>()
                .HasKey(c => new { c.IdEvento, c.IdInvitado});
        }

    }
}
