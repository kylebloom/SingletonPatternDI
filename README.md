# Sample Console App with Singleton Pattern, Dependency Injection, and Async/Await

This repository contains a sample C# console application that demonstrates the use of the Singleton Pattern combined with Dependency Injection and asynchronous programming using `async` and `await`.

# Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Setup and Installation](#setup-and-installation)
- [Usage](#usage)
- [Code Overview](#code-overview)
  - [Singleton Pattern](#singleton-pattern)
  - [Dependency Injection](#dependency-injection)
  - [Async/Await](#async-await)

## Introduction

This sample application showcases how to implement the Singleton Pattern and use Dependency Injection in a C# console application, along with asynchronous methods using `async` and `await`.

## Features

- Singleton Pattern for creating a singular instance to be shared across the app.
- Dependency Injection for managing dependencies.
- Asynchronous methods to simulate I/O-bound operations.


## Usage

The application demonstrates initializing a single worker class to be shared across the app that performs async methods. The worker creation is also async which could simulate fetching necessary resources via api or other. Dependencies injected using the Microsoft.Extensions.DependencyInjection library.

## Code Overview

### Singleton Pattern

- **SingletonService.cs**: The singleton worker. File includes the interface with single method `DoWorkAsync`.

### Dependency Injection

- **Program.cs**: Sets up the DI container and manually creates the instance to be registered in the DI container.

### Async/Await

- **Program.cs**: Contains the `AsyncAwait`code that simulates fetching resources during the creation of the worker instance. Could be useful to retrieve API keys or other config-related resources that are hosted outside of the app.
- **SingletonService.cs**: Contains the `AsyncAwait`code that simulates performing the work.


### Example Code

**SingletonService.cs**
```csharp

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
```



**Program.cs**
```csharp
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

```

