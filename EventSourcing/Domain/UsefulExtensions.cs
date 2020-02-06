using System;

namespace EventSourcing.Domain
{
    public static class UsefulExtensions
    {
        public static T With<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}