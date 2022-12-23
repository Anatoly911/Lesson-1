using System;
using MassTransit;
using MassTransit.Audit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Lesson_1.Consumers;
using Restaurant.Messages;
using Restaurant.Messages.InMemoryDb;


namespace Lesson_1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMassTransit(x =>
            {
                services.AddSingleton<IMessageAuditStore, AuditStore>();
                var serviceProvider = services.BuildServiceProvider();
                var auditStore = serviceProvider.GetServices<IMessageAuditStore>();
                x.AddConsumer<RestaurantBookingRequestConsumer>(config =>
                {
                    config.UseScheduledRedelivery(r =>
                    {
                        r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30));
                    });
                    config.UseMessageRetry(r =>
                    {
                        r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                        r.Handle<ArgumentNullException>();
                    });
                })
                .Endpoint(e =>
                {
                    e.Temporary = true;
                });
                x.AddConsumer<BookingRequestFaultConsumer>(config =>
                {
                    config.UseScheduledRedelivery(r =>
                    {
                        r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30));
                    });
                    config.UseMessageRetry(r =>
                    {
                        r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                        r.Handle<ArgumentNullException>();
                    });
                })
                .Endpoint(e =>
                {
                    e.Temporary = true;
                });
                x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                .Endpoint(e => e.Temporary = true)
                .InMemoryRepository();
                x.AddDelayedMessageScheduler();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UsePrometheusMetrics(serviceName: "booking_service");
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(context);
                    cfg.ConnectSendAuditObservers((IMessageAuditStore)auditStore);
                    cfg.ConnectConsumeAuditObserver((IMessageAuditStore)auditStore);
                });
            });
            services.Configure<MassTransitHostOptions>(options =>
            {
                options.WaitUntilStarted = true;
                options.StartTimeout= TimeSpan.FromSeconds(30);
                options.StopTimeout= TimeSpan.FromSeconds(1);
            });
            services.AddTransient<RestaurantBooking>();
            services.AddTransient<RestaurantBookingSaga>();
            services.AddTransient<Restaurant>();
            services.AddSingleton<IInMemoryRepository<IBookingRequest>, InMemoryRepository<IBookingRequest>>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
        }
    }
}
