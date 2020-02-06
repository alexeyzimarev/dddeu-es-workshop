using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Application;
using EventSourcing.Domain;
using EventSourcing.Infrastructure;
using EventSourcing.Projections;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EventSourcing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var esConnection = EventStoreConnection.Create(
                Configuration["eventStore:connectionString"],
                ConnectionSettings.Create().KeepReconnecting(),
                "test-workshop");
            var store = new EsAggregateStore(esConnection);

            var mongoDb = GetMongo(Configuration["mongoDb:connectionString"]);

            services.AddSingleton<IAggregateStore>(store);
            services.AddSingleton<ScreeningAppService>();
            services.AddSingleton<IHostedService>(
                new EventStoreHostedService(esConnection,
                    new ScheduledScreeningProjection(mongoDb)
                )
            );

            services.AddSingleton<GetMovieDuration>(s =>
                _ => Task.FromResult(120)
            );
            services.AddSingleton<GetUtcNow>(s => () => DateTimeOffset.Now);
            services.AddSingleton<GetTheaterCapacity>(s => id => Task.FromResult(100));
        }

        static IMongoDatabase GetMongo(string connectionString)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);

            var client   = new MongoClient(settings);
            var database = MongoUrl.Create(connectionString).DatabaseName;

            return client.GetDatabase(database);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}