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
            /*builder.Entity<Zamowienie>()
                .HasKey(z => z.ZamowienieId);
            builder.Entity<Klient>().HasKey(k => k.KlientId);
            builder.Entity<Klient>().HasMany(z => z.Zamowienie).WithOne(kl => kl.Klient).HasForeignKey(kli => kli.KlientId);

            builder.Entity<Adres>().HasKey(k => k.AdresId);
            builder.Entity<Adres>().HasMany(z => z.Zamowienie).WithOne(kl => kl.Adres).HasForeignKey(kli => kli.AdresId);

            builder.Entity<Zespol>().HasKey(ze => ze.ZespolId);
            builder.Entity<Zespol>().HasMany(z => z.Zamowienie).WithOne(zes => zes.Zespol).HasForeignKey(zee => zee.ZespolId);

            

            builder.Entity<Status>().HasKey(k => k.StatusId);
            builder.Entity<Status>().HasMany(z => z.Zamowienie).WithOne(kl => kl.Status).HasForeignKey(kli => kli.StatusId);

            */

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
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
