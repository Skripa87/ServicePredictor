namespace ServicePredictor.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ServicePredictorDbContext : DbContext
    {
        // Контекст настроен для использования строки подключения "ServicePredictorDbContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "ServicePredictor.Models.ServicePredictorDbContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "ServicePredictorDbContext" 
        // в файле конфигурации приложения.
        
        public ServicePredictorDbContext()
            : base("name=ServicePredictorDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<BusRoute>()
            //            .HasKey(b => b.Id)
            //            .HasMany(r => r.Stations)
            //            .WithMany(s => s.BusRoutes)
            //            .Map(m => m.MapLeftKey("Station_Id")
            //                       .MapRightKey("BusRoute_Id")
            //                       .ToTable("Stations_BusRoutes"));
            modelBuilder.Entity<MapPoint>()
                        .HasKey(m => m.Id)
                        .HasMany(r => r.BusRoutes)
                        .WithMany(w => w.MapPoints)
                        .Map(m => m.MapLeftKey("BusRoute_Id")
                        .MapRightKey("MapPoint_Id")
                        .ToTable("BusRoutes_MapPoints"));
        }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<BusRoute> BusRoutes { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}