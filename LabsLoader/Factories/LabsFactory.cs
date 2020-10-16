using Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LabsLoader.Factories
{
    class LabFactory
    {
        public static IEnumerable<IRunnable> GetLabs()
        {
            Assembly assembly = AssemblyFactory.GetAssembly("LabsLibrary");

            var labsTypes = assembly.GetTypes().Where(
                s => s.GetInterfaces().Contains(typeof(IRunnable)) &&
                s.FullName.Split('.').Last().StartsWith("Lab"));

            List<IRunnable> labs = new List<IRunnable>();

            foreach (var type in labsTypes)
            {
                var obj = Activator.CreateInstance(type);
                labs.Add(obj as IRunnable);
            }

            return labs;
        }
    }
}