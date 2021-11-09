using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using P2Hospital.Models;

namespace P2Hospital.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<P2Hospital.Models.Enfermeiro> Enfermeiro { get; set; }
        public DbSet<P2Hospital.Models.Hospital> Hospital { get; set; }
        public DbSet<P2Hospital.Models.MaterialConsumo> MaterialConsumo { get; set; }
        public DbSet<P2Hospital.Models.Medicamento> Medicamento { get; set; }
        public DbSet<P2Hospital.Models.OPME> OPME { get; set; }
    }
}
