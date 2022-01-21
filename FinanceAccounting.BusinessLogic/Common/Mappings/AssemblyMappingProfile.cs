using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace FinanceAccounting.BusinessLogic.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile()
        {
            ApplyMappingFromAssembly();
        }

        private void ApplyMappingFromAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var mappableTypes = assembly.GetExportedTypes()
                .Where(type => typeof(IMappable).IsAssignableFrom(type) && !type.IsInterface)
                .ToList();

            foreach (Type type in mappableTypes)
            {
                object instance = Activator.CreateInstance(type);
                MethodInfo methodInfo = type.GetMethod(nameof(IMappable.Mapping));
                methodInfo?.Invoke(instance, new object[] {this});
            }
        }
    }
}
