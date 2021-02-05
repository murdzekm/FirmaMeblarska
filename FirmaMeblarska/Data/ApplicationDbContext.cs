using FirmaMeblarska.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirmaMeblarska.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ZamowieniePlyta>().HasKey(zp => new { zp.ZamowienieId, zp.PlytaId });
            builder.Entity<ZespolPracownik>().HasKey(zp => new { zp.ZespolId, zp.PracownikId });
            builder.Entity<PracownikMaszyna>().HasKey(zp => new { zp.MaszynaId, zp.PracownikId });
           
        }
        public DbSet<Magazyn> Magazyn { get; set; }
        public DbSet<Adres> Adres { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Plyta> Plyta { get; set; }
        public DbSet<Pracownik> Pracownik { get; set; }
        public DbSet<Klient> Klient { get; set; }
        public DbSet<Zamowienie> Zamowienie { get; set; }
        public DbSet<Maszyna> Maszyna { get; set; }
        public DbSet<Narzedzie> Narzedzie { get; set; }
        public DbSet<Zespol> Zespol { get; set; }
        public DbSet<ZamowieniePlyta> ZamowieniePlyta { get; set; }
        public DbSet<ZespolPracownik> ZespolPracownik { get; set; }
    }
}
