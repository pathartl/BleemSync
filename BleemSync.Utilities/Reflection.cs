using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BleemSync.Utilities
{
    public static class Reflection
    {
        public static IEnumerable<Type> TypesImplementingInterface(Type desiredType)
        {
            return AppDomain
                   .CurrentDomain
                   .GetAssemblies()
                   .SelectMany(assembly => assembly.GetTypes())
                   .Where(type => desiredType.IsAssignableFrom(type));
        }
    }
}
