using System.Reflection;

namespace Outbox_101.Infrastructure.Kafka.Consumers.Serialization;

public static class TypeGetter
{
    public static Type? GetTypeFromCurrentDomainAssembly(string typeName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && t.Name == typeName)
            .FirstOrDefault();
    }
}
