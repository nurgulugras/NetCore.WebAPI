using System;
using System.Text;
using ALMS.Core;
using ALMS.Model;
using Elsa.NNF.Data.ORM.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ALMS.Data.EFCore
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions options) : base(options) { }

        public ApiContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionStringFromAppSettings = GetConnectionString();
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseNpgsql(connectionStringFromAppSettings);

                optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning));
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
        private static string GetConnectionString()
        {
            var connectionString = new StringBuilder();

            var databaseSettings = GlobalFields.ApiConfig.DatabaseSettings;

            var dataSource = databaseSettings.Server;
            var port = databaseSettings.Port;
            var initialCatalog = databaseSettings.Database;
            var userName = databaseSettings.User;
            var password = databaseSettings.Password;

            connectionString.Append(string.Concat("Host=", dataSource, ";"));

            if (port != default)
                connectionString.Append(string.Concat("Port=", port, ";"));

            connectionString.Append(string.Concat("Database=", initialCatalog, ";"));
            connectionString.Append(string.Concat("Username=", userName, ";"));
            connectionString.Append(string.Concat("Password=", password, ";"));

            return connectionString.ToString();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LoadSeedDatas(modelBuilder);
            LoadUniqueColumns(modelBuilder);
        }

        #region [ LoadUniqueColumns ]
        private void LoadUniqueColumns(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<App>()
              .HasIndex(p => p.Name)
              .IsUnique();

            modelBuilder.Entity<App>()
              .HasIndex(p => p.ApiKey)
              .IsUnique();

            modelBuilder.Entity<AppLimit>()
              .HasIndex(p => new { p.AppId, p.LimitName })
              .IsUnique();

            modelBuilder.Entity<AppProduct>()
              .HasIndex(p => new { p.AppId, p.Product })
              .IsUnique();

            modelBuilder.Entity<License>()
              .HasIndex(p => p.LicenseNo)
              .IsUnique();

            modelBuilder.Entity<LicenseLimit>()
              .HasIndex(p => new { p.LicenseId, p.AppLimitId })
              .IsUnique();

            modelBuilder.Entity<LicenseProduct>()
              .HasIndex(p => new { p.LicenseId, p.AppProductId })
              .IsUnique();

            modelBuilder.Entity<User>()
              .HasIndex(p => new { p.Email })
              .IsUnique();

            modelBuilder.Entity<Organization>()
              .HasIndex(p => new { p.Name })
              .IsUnique();

            modelBuilder.Entity<Company>()
              .HasIndex(p => new { p.OrganizationId, p.Name })
              .IsUnique();
        }

        #endregion

        #region [ LoadSeedDatas ]
        private void LoadSeedDatas(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Name = "system", Surname = "admin", Email = "taner.selek@elsabilisim.com", Role = RoleType.Admin, CreateDate = DateTime.Now, Password = "RH0mFbWRdT+sGr8cUfc7JZ30I/qNuWP//K4SGcgEAX8=", PasswordHashCode = "afewg32ggh_235", IsActive = true });
        }

        #endregion

        #region [ Entities ]
        DbSet<EntityActivityLog> EntityActivityLog { get; set; }
        DbSet<User> User { get; set; }
        DbSet<App> App { get; set; }
        DbSet<AppLimit> AppLimit { get; set; }
        DbSet<AppProduct> AppProduct { get; set; }
        DbSet<License> License { get; set; }
        DbSet<LicenseLimit> LicenseLimit { get; set; }
        DbSet<LicenseProduct> LicenseProduct { get; set; }
        DbSet<Session> Session { get; set; }
        DbSet<Organization> Organization { get; set; }
        DbSet<Company> Company { get; set; }

        #endregion
    }
}