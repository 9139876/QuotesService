using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.BL.Static
{
    public static class ModelPropertiesCollectionConverter
    {
        public static List<KeyValuePair<string, string>> ModelToPropertiesCollection(object obj)
        {
            return obj
                .GetType()
                .GetProperties()
                .Select(x => new KeyValuePair<string, string>(x.Name, x.GetValue(obj).ToString()))
                .ToList();
        }

        public static T PropertiesCollectionToModel<T>(List<KeyValuePair<string, string>> propertiesCollection)
        {
            var instance = (T)Activator.CreateInstance(typeof(T));

            var instanceProperties = instance.GetType().GetProperties();

            foreach (var property in propertiesCollection)
            {
                var currentProperty = instanceProperties.SingleOrDefault(x => x.Name == property.Key);

                if (currentProperty != null)
                {
                    currentProperty.SetValue(instance, property.Value);
                }
            }

            return instance;
        }
    }
}
