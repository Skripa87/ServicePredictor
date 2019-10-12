namespace ServicePredictor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        Lat = c.Double(nullable: false),
                        Lng = c.Double(nullable: false),
                        Type = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        InformationTable_Id = c.String(),
                        AccessCode = c.String(),
                        UserCity = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusRoutes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        StartStationId_Id = c.String(),
                        LastStation_Id = c.String(),
                        LastStation_Id1 = c.String(maxLength: 128),
                        StartStation_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.LastStation_Id1)
                .ForeignKey("dbo.Stations", t => t.StartStation_Id)
                .Index(t => t.LastStation_Id1)
                .Index(t => t.StartStation_Id);
            
            CreateTable(
                "dbo.MapPoints",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TimePoint = c.DateTime(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Azimut = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusRoutes_MapPoints",
                c => new
                    {
                        BusRoute_Id = c.String(nullable: false, maxLength: 128),
                        MapPoint_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.BusRoute_Id, t.MapPoint_Id })
                .ForeignKey("dbo.MapPoints", t => t.BusRoute_Id, cascadeDelete: true)
                .ForeignKey("dbo.BusRoutes", t => t.MapPoint_Id, cascadeDelete: true)
                .Index(t => t.BusRoute_Id)
                .Index(t => t.MapPoint_Id);
            
            CreateTable(
                "dbo.Stations_BusRoutes",
                c => new
                    {
                        Station_Id = c.String(nullable: false, maxLength: 128),
                        BusRoute_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Station_Id, t.BusRoute_Id })
                .ForeignKey("dbo.BusRoutes", t => t.Station_Id, cascadeDelete: true)
                .ForeignKey("dbo.Stations", t => t.BusRoute_Id, cascadeDelete: true)
                .Index(t => t.Station_Id)
                .Index(t => t.BusRoute_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations_BusRoutes", "BusRoute_Id", "dbo.Stations");
            DropForeignKey("dbo.Stations_BusRoutes", "Station_Id", "dbo.BusRoutes");
            DropForeignKey("dbo.BusRoutes", "StartStation_Id", "dbo.Stations");
            DropForeignKey("dbo.BusRoutes_MapPoints", "MapPoint_Id", "dbo.BusRoutes");
            DropForeignKey("dbo.BusRoutes_MapPoints", "BusRoute_Id", "dbo.MapPoints");
            DropForeignKey("dbo.BusRoutes", "LastStation_Id1", "dbo.Stations");
            DropIndex("dbo.Stations_BusRoutes", new[] { "BusRoute_Id" });
            DropIndex("dbo.Stations_BusRoutes", new[] { "Station_Id" });
            DropIndex("dbo.BusRoutes_MapPoints", new[] { "MapPoint_Id" });
            DropIndex("dbo.BusRoutes_MapPoints", new[] { "BusRoute_Id" });
            DropIndex("dbo.BusRoutes", new[] { "StartStation_Id" });
            DropIndex("dbo.BusRoutes", new[] { "LastStation_Id1" });
            DropTable("dbo.Stations_BusRoutes");
            DropTable("dbo.BusRoutes_MapPoints");
            DropTable("dbo.MapPoints");
            DropTable("dbo.BusRoutes");
            DropTable("dbo.Stations");
        }
    }
}
