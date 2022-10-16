using back_end.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaquetesSucursales>()
                .HasKey(x => new { x.PaqueteId, x.SucursalId });

            modelBuilder.Entity<PaquetesServicios>()
                .HasKey(x => new { x.PaqueteId, x.ServicioId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<PaquetesSucursales> PaquetesSucursales { get; set; }
        public DbSet<PaquetesServicios> PaquetesServicios { get; set; }
    }
}
