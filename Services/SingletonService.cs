using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonPatternDI.Services
{
    public class SingletonService : ISingletonService
    {
        private readonly Guid Id;
        private int _counter;

        // private constructor prevents direct instantiation
        private SingletonService()
        {
            Id = Guid.NewGuid();
            _counter = 0;
        }

        // factory method for async initialization
        public static async Task<SingletonService> CreateAsync()
        {
            var instance = new SingletonService();
            await instance.InitializeAsync();
            return instance;
        }

        // async initialization method
        private async Task InitializeAsync()
        {
            Console.WriteLine("Initializing SingletonService...");
            await Task.Delay(1000); // Simulate async initialization
            Console.WriteLine("SingletonService initialized.");
        }

        // async method to simulate work
        public async Task DoWorkAsync()
        {
            _counter++;
            Console.WriteLine($"Singleton Instance ID: {Id}, Counter: {_counter}");
            Console.WriteLine("Starting work...");
            await Task.Delay(1000); // Simulate async work
            Console.WriteLine("Work completed.");
        }
    }

    public interface ISingletonService
    {
        Task DoWorkAsync();
    }
}
