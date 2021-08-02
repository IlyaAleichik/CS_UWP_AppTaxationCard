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

        public DbSet<Poroda> Porodas { get; set; }

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


            builder.Entity<MaketThen>().HasOne(p => p.Poroda).WithMany(t => t.MaketThens).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Account>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<Account>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<Account>().HasIndex(u => u.Phone).IsUnique();

            builder.Entity<TypeErrosion>().HasIndex(u => u.NameTypeErrosion).IsUnique();
            builder.Entity<DegreeErrosion>().HasIndex(u => u.NameDegreeErrosion).IsUnique();
            builder.Entity<ExpositionSlope>().HasIndex(u => u.NameExpositionSlope).IsUnique();
            builder.Entity<TypeOrl>().HasIndex(u => u.NameTypeOrl).IsUnique();
            builder.Entity<TypeEarth>().HasIndex(u => u.NameTypeEarth).IsUnique();
            builder.Entity<Forestry>().HasIndex(u => u.NameForestry).IsUnique();

            builder.Entity<Forestry>().HasData(new Forestry[] {
            new Forestry { Id=1, NameForestry = "Заславльское" },
            new Forestry { Id=2, NameForestry = "Новосельское" },
            new Forestry { Id=3,NameForestry = "Путчинское"},
            new Forestry { Id=4,NameForestry = "Кайковское" },
            new Forestry { Id=5, NameForestry = "Держинское"},
            new Forestry { Id=6, NameForestry = "Станьковское"},
            new Forestry { Id=7, NameForestry = "Волмянское" }
            });


            builder.Entity<TypeErrosion>().HasData(new TypeErrosion[] {
                 new TypeErrosion {  Id=1,NameTypeErrosion = "Водная"},
                 new TypeErrosion {  Id=2,NameTypeErrosion = "Ветровая"}
                });



            builder.Entity<DegreeErrosion>().HasData(new DegreeErrosion[] {
               new DegreeErrosion { Id=1, NameDegreeErrosion = "Совсем смытые" },
               new DegreeErrosion { Id=2, NameDegreeErrosion = "Слабая"},
               new DegreeErrosion {  Id=3,NameDegreeErrosion = "Средняя"},
               new DegreeErrosion {  Id=4,NameDegreeErrosion = "Сильная" }
                });

            builder.Entity<Poroda>().HasData(new Poroda[] {
               new Poroda { Id=1, NamePoroda = "С" },
               new Poroda { Id=2, NamePoroda = "Е"},
               new Poroda {  Id=3,NamePoroda = "Д"},
               new Poroda {  Id=4,NamePoroda = "Б" },
               new Poroda {  Id=5,NamePoroda = "О" },
               new Poroda {  Id=6,NamePoroda = "Л" },
               new Poroda {  Id=7,NamePoroda = "С" }
                });



            builder.Entity<TypeOrl>().HasData(new TypeOrl[] {

              new TypeOrl { Id=1, NameTypeOrl = "Прибережные полосы леса", },
              new TypeOrl { Id=2, NameTypeOrl = "Участки леса в поймах рек", },
              new TypeOrl { Id=3, NameTypeOrl = "4-я зона радиактивного загрязнения", },
              new TypeOrl { Id=4, NameTypeOrl = "Леса генетических резерватов", }
                });



            builder.Entity<ExpositionSlope>().HasData(new ExpositionSlope[] {

                new ExpositionSlope { Id=1, NameExpositionSlope = "С", },
              new ExpositionSlope { Id=2, NameExpositionSlope = "Ю", },
              new ExpositionSlope { Id=3, NameExpositionSlope = "З", },
              new ExpositionSlope { Id=4, NameExpositionSlope = "В", },
              new ExpositionSlope { Id=5, NameExpositionSlope = "СЗ", },
              new ExpositionSlope { Id=6, NameExpositionSlope = "ЮЗ", },
              new ExpositionSlope { Id=7, NameExpositionSlope = "СВ", },
              new ExpositionSlope { Id=8, NameExpositionSlope = "ЮВ", },
                });




            builder.Entity<TypeEarth>().HasData(new TypeEarth[] {
            new TypeEarth { Id=1, NameTypeEarth = "Насаждения естественного происхождения", },
               new TypeEarth { Id=2, NameTypeEarth = "Лесные культуры", },
               new TypeEarth { Id=3, NameTypeEarth = "Квартальные просеки", }
                });

            base.OnModelCreating(builder);











        }


    }
}
