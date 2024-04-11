using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesGraph.Core.Configuration;
using SalesGraph.Core.DataAccess.Models.Entities;
using SalesGraph.Core.Infrastructure.Helpers;

namespace SalesGraph.Core.DataAccess.Context
{
    public class SalesGraphContext : DbContext
    {

        private readonly DbSettings _dbSettings;
        //Ensure that new database will be created
        public SalesGraphContext(DbContextOptions<SalesGraphContext> options, IOptions<DbSettings> dbSettings) : base(options)
        {
            _dbSettings = dbSettings?.Value ?? throw new ApplicationException("Cannot initialize database settings");

            if (!_dbSettings.UseSeedData)
                Database.EnsureDeleted();

            Database.EnsureCreated();
            var returnVal = Database.ExecuteSqlRaw(GetGroupingKeyFunctionSql());
        }

        //Seed database with random data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(typeof(SalesGraphContext).GetMethod(nameof(GetGroupingKey), new[] { typeof(DateTime), typeof(int) }))
                .HasName("fn_GetGroupingKey");

            if (_dbSettings.UseSeedData)
                PopulateData(modelBuilder);
        }

        //stored function for calculation grouping index
        public string GetGroupingKey(DateTime date, int groupingParam) => throw new NotSupportedException();


        //The products collection
        public DbSet<Product> Products { get; set; }

        //The sales information collection
        public DbSet<SaleItem> Sales { get; set; }

        #region Helper method to populate database
        private void PopulateData(ModelBuilder modelBuilder)
        {
            //warning, large object, do not use big span between two dates
            var products = SeedHelper.GetProducts();
            var sales = SeedHelper.GetSales(products, Infrastructure.Constants.Generator.MinDateTime, DateTime.Now);

            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<SaleItem>().HasData(sales);
        }

        private string GetGroupingKeyFunctionSql()
        {
            return @"CREATE OR ALTER FUNCTION dbo.fn_GetGroupingKey
                    (
                        @date DATETIME,
                        @groupingParam INT
                    )
                    RETURNS NVARCHAR(50)
                    AS
                    BEGIN
                        DECLARE @result NVARCHAR(50)

                        IF @groupingParam = 1
                            SET @result = CONVERT(NVARCHAR(10), @date, 112)
                        ELSE IF @groupingParam = 2
                            SET @result = CONVERT(NVARCHAR(10), DATEADD(WEEK, DATEDIFF(WEEK, 0, @date), 0), 112)
                        ELSE IF @groupingParam = 3
                            SET @result = CONVERT(NVARCHAR(7), @date, 120)
                        ELSE IF @groupingParam = 4
                            SET @result = 'Q' + CAST(DATEPART(QUARTER, @date) AS NVARCHAR(1)) + '-' + CAST(YEAR(@date) AS NVARCHAR(4))

                        RETURN @result
                    END
                    ";
        }
        #endregion
    }
}
