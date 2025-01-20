using Bunit;
using Jobify.BL.DALModels;
using Jobify.BL.Database;
using Jobify.BL.Interfaces;
using Jobify.BL.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace JobifyTests
{
    //public class JobifyTestContext : TestContext
    //{
    //    public JobifyTestContext()
    //    {
    //        var options = new DbContextOptionsBuilder<JobifyContext>()
    //            .UseNpgsql("Host=truly-universal-spitz.data-1.use1.tembo.io;Port=5432;Username=postgres;Password=y2csSQsBqEyAi4c6")
    //            .Options;

    //        var dbContext = new JobifyContext(options);
    //        var queries = new Queries(dbContext);

    //        Services.AddSingleton<JobifyContext>(dbContext);
    //        //Services.AddDbContext<JobifyContext>(options => options.UseNpgsql(), ServiceLifetime.Transient);
    //        //Services.AddTransient<JobifyContext>(sp => dbContext);
    //        //Services.AddSingleton<IQueries>(queries);
    //        Services.AddSingleton<IQueries>(q => Queries.GetInstance(q.GetRequiredService<JobifyContext>()));

    //        DbQueryProvider.Init(queries);
    //    }

    //}
    public class JobifyTestContext : TestContext
    {
        public JobifyTestContext()
        {
            // Register DbContext with Scoped Lifetime
            Services.AddDbContext<JobifyContext>(options =>
                options.UseNpgsql("Host=truly-universal-spitz.data-1.use1.tembo.io;Port=5432;Username=postgres;Password=y2csSQsBqEyAi4c6"),
                ServiceLifetime.Scoped
            );

            // Register Queries with Scoped Lifetime
            Services.AddScoped<IQueries, Queries>();

            // Ensure DbQueryProvider is initialized correctly within the scope
            var serviceProvider = Services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var queries = scope.ServiceProvider.GetRequiredService<IQueries>();
            DbQueryProvider.Init(queries);

            // Ensure the database is ready
            var dbContext = scope.ServiceProvider.GetRequiredService<JobifyContext>();
            dbContext.Database.EnsureCreated();
        }
    }

}