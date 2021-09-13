using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace AwsLambdaExample.Application
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ScanAndRegisterAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

        /// <summary>
        /// Implementing interfaces
        /// </summary>
        public Type[]? Interfaces { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Scan assembly for types which has <see cref="ScanAndRegisterAttribute"/> decorated
        /// NOTE: Do not need to call this on the module because it is called once in Program.cs
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection ScanAndRegister(this IServiceCollection services, Assembly assembly)
        {
            // Optional components
            assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.GetCustomAttribute<ScanAndRegisterAttribute>() != null)
                .ToList()
                .ForEach(type =>
                {
                    var att = type.GetCustomAttribute<ScanAndRegisterAttribute>()!;
                    var its = type.GetInterfaces();

                    if (type.IsGenericType)
                    {
                        services.Add(new ServiceDescriptor(type.GetInterfaces()[0].GetGenericTypeDefinition(), type, att.Lifetime));
                    }
                    else
                    {
                        var interfaces = att.Interfaces == null ? its.ToList() : att.Interfaces.ToList();
                        interfaces.ForEach(interfaceType =>
                        {
                            services.Add(new ServiceDescriptor(interfaceType, type, att.Lifetime));
                        });
                        services.Add(new ServiceDescriptor(type, type, att.Lifetime));
                    }
                });
            return services;
        }
    }
}
