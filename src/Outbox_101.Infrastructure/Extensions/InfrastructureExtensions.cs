namespace Outbox_101.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddHandlersFromType(this IServiceCollection services, Type type)
    {
        Assembly assembly = type.Assembly;
        Type[] assemblyTypes = assembly.GetTypes() ??
            throw new ArgumentNullException(nameof(type));

        foreach (var assemblyType in assemblyTypes)
        {
            if (assemblyType.IsAbstract || assemblyType.IsInterface)
                continue;

            var interfaces = assemblyType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType)
                    continue;

                Type interfaceDefinition = @interface.GetGenericTypeDefinition();

                if (interfaceDefinition == typeof(IEventHandler<>))
                {
                    var handledType = @interface.GenericTypeArguments[0];
                    var handlerType = typeof(IEventHandler<>).MakeGenericType(handledType);
                    services.AddScoped(handlerType, assemblyType);
                }                
            }
        }

        return services;
    }
}