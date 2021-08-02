using Microsoft.EntityFrameworkCore;


namespace AppTaxationCard.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Videl> Videls { get; set; }
        public DbSet<SessionUser> SessionUsers { get; set; }
        public DbSet<Kvartal> Kvartals { get; set; }

        public DbSet<TypeErrosion> TypeErrosions { get; set; }

        public DbSet<DegreeErrosion> DegreeErrosions { get; set; }

        public DbSet<ExpositionSlope> ExpositionSlopes { get; set; }

        public DbSet<TypeOrl> TypeOrls { get; set; }

        public DbSet<TypeEarth> TypeEarths { get; set; }

        public DbSet<Forestry> Forestries { get; set; }
        public DbSet<MaketThen> MaketThens { get; set; }

        //public DbSet<Poroda> Porodas { get; set; }

        public DbSet<MaketThenPoroda> MaketThenPorodas { get; set; }


        public ModelContext()
        {
            Database.EnsureCreated();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + "LocalDataBase.db");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<Account>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<Account>().HasIndex(u => u.Phone).IsUnique();

        }


    }
}
