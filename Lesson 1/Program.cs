using Lesson_1.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lesson_1
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
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
                    x.AddSagaStateMachine<RestautantBookingSaga, RestaurantBooking>()
                    .Endpoint(e => e.Temporary = true)
                    .InMemoryRepository();
                    x.AddDelayedMessageScheduler();
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.UseDelayedMessageScheduler();
                        cfg.UseInMemoryOutbox();
                        cfg.ConfigureEndpoints(context);
                    });
                });
                services.AddTransient<RestaurantBooking>();
                services.AddTransient<RestautantBookingSaga>();
                services.AddTransient<Restaurant>();
                services.AddHostedService<Worker>();
            });
    }
}