using System.Reflection;

namespace LabsLoader.Factories
{
    public class AssemblyFactory
    {
        public static Assembly GetAssembly(string assemblyName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            return assembly;
        }
    }
}