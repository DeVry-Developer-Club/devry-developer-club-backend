using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DevryDeveloperClub.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Move values from one object into another. This relies on properties or fields
        /// with the same naming convention being in BOTH types
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="customValues"></param>
        /// <typeparam name="TDestination">Type of object that will consume obj values</typeparam>
        /// <returns>Hydrated instance of <typeparamref name="TDestination"/></returns>
        public static TDestination CloneTo<TDestination>(this object obj, params (string name, object value)[] customValues)
            where TDestination : new()
        {
            if (obj == null)
                return default;
            
            PropertyInfo[] destProps = typeof(TDestination)
                    .GetTypeInfo()
                    .GetProperties()
                    .ToArray();

            PropertyInfo[] props = obj.GetType()
                .GetTypeInfo()
                .GetProperties()
                .Where(x => destProps.Any(y => y.Name == x.Name))
                .ToArray();

            FieldInfo[] destFields = typeof(TDestination)
                .GetTypeInfo()
                .GetFields()
                .ToArray();
            
            FieldInfo[] fields = obj.GetType()
                    .GetTypeInfo()
                    .GetFields()
                    .Where(x => destFields.Any(y => y.Name == x.Name))
                    .ToArray();

            TDestination instance = new();

            foreach (var prop in props)
            {
                object value = props.First(x => x.Name == prop.Name)
                    .GetValue(obj);

                var destProp = destProps.First(x => x.Name == prop.Name);
                
                if (destProp.SetMethod == null)
                    continue;

                destProp.SetValue(instance, value);
            }

            foreach (var field in fields)
            {
                object value = fields.First(x => x.Name == field.Name);
                
                destFields.First(x=>x.Name == field.Name)
                    .SetValue(instance, value);
            }
            
            foreach(var custom in customValues)
                if (destProps.Any(x => x.Name == custom.name))
                    destProps.First(x => x.Name == custom.name)
                        .SetValue(instance, custom.value);
            
            return instance;
        }
    }
}