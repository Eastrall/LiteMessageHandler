using System;

namespace MessageHandler.Extensions;

internal static class ReflectionExtensions
{
    public static bool ImplementsGenericType(this Type sourceType, Type genericType)
    {
        if (sourceType is null)
        {
            throw new ArgumentNullException(nameof(sourceType));
        }

        if (genericType is null)
        {
            throw new ArgumentNullException(nameof(genericType));
        }

        return sourceType.BaseType?.FindGenericType(genericType) != null;
    }

    public static Type? FindGenericType(this Type sourceType, Type? genericType)
    {
        if (sourceType == null)
        {
            throw new ArgumentNullException(nameof(sourceType));
        }

        if (sourceType.BaseType == null)
        {
            return null;
        }

        if (!sourceType.IsGenericType)
        {
            return sourceType.BaseType.FindGenericType(genericType);
        }

        if (sourceType.GetGenericTypeDefinition() == genericType)
        {
            return sourceType;
        }

        if (sourceType.BaseType != null)
        {
            return sourceType.BaseType.FindGenericType(genericType);
        }

        return null;
    }
}
