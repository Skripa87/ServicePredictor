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
        public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}