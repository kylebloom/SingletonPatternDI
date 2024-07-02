using Microsoft.Extensions.DependencyInjection;
using SingletonPatternDI.Services;

namespace SingletonPatternDI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Set up dependency injection

            // if singletonservice doesn't require async set up to retrieve data

            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton<ISingletonService, SingletonService>()
            //    .BuildServiceProvider();

            // if singletonservice does async set up to retrieve data from a database or other source
            // you can use the factory method to create the instance
            var services = new ServiceCollection();

            var singletonService = await SingletonService.CreateAsync();

            // register manually created service 
            services.AddSingleton<ISingletonService>(singletonService);

            var serviceProvider = services.BuildServiceProvider();



            // Resolve the SingletonService
            var resolvedSingletonService = serviceProvider.GetService<ISingletonService>();
            if (resolvedSingletonService == null)
            {
                Console.WriteLine("Failed to set up singletonService DI. Exiting..");
                Console.ReadKey();
                return;
            }

            // Call an async method on the singleton service multiple times
            await resolvedSingletonService.DoWorkAsync();
            await resolvedSingletonService.DoWorkAsync();

            // Resolve the SingletonService again to prove it's the same instance
            var anotherResolvedSingletonService = serviceProvider.GetService<ISingletonService>();
            await anotherResolvedSingletonService.DoWorkAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
