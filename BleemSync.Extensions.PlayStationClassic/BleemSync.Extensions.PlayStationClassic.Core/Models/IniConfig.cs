using BleemSync.Extensions.PlayStationClassic.Core.Attributes;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Models
{
    public class IniConfig
    {
        public IniConfig() : this(new Configuration()) { }
        public IniConfig(Configuration configuration)
        {
            var derivedType = GetType();
            var config = this;

            foreach (var property in derivedType.GetProperties())
            {
                var useDefault = true;
                var attributes = property.GetCustomAttributes(true);
                var propertyName = property.Name;
                var sectionName = "";

                var propertyAttribute = (IniPropertyAttribute)attributes.Where(a => a.GetType() == typeof(IniPropertyAttribute)).FirstOrDefault();
                var sectionAttribute = (IniSectionAttribute)attributes.Where(a => a.GetType() == typeof(IniSectionAttribute)).FirstOrDefault();

                if (propertyAttribute != null)
                {
                    propertyName = propertyAttribute.Name;
                }

                if (sectionAttribute != null)
                {
                    sectionName = sectionAttribute.Name;
                }

                var valFromConfig = configuration[sectionName][propertyName];

                if (!valFromConfig.IsEmpty)
                {
                    switch (property.PropertyType.FullName)
                    {
                        case "System.String":
                            property.SetValue(config, configuration[sectionName][propertyName].StringValue);
                            break;

                        case "System.Boolean":
                            property.SetValue(config, configuration[sectionName][propertyName].BoolValue);
                            break;

                        default:
                            throw new NotImplementedException($"Type \"{property.PropertyType.FullName}\" not supported by the INI parser.");
                            break;
                    }
                }
                else
                {
                    var defaultAttribute = (DefaultValueAttribute)attributes.Where(a => a.GetType() == typeof(DefaultValueAttribute)).FirstOrDefault();

                    if (defaultAttribute != null)
                    {
                        property.SetValue(config, defaultAttribute.Value, null);
                    }
                }
            }
        }

        public Configuration ToConfiguration()
        {
            var configuration = new Configuration();

            var derivedType = GetType();
            var config = this;

            foreach (var property in derivedType.GetProperties())
            {
                var useDefault = true;
                var attributes = property.GetCustomAttributes(true);
                var propertyName = property.Name;
                var sectionName = "";

                var propertyAttribute = (IniPropertyAttribute)attributes.Where(a => a.GetType() == typeof(IniPropertyAttribute)).FirstOrDefault();
                var sectionAttribute = (IniSectionAttribute)attributes.Where(a => a.GetType() == typeof(IniSectionAttribute)).FirstOrDefault();

                if (propertyAttribute != null)
                {
                    propertyName = propertyAttribute.Name;
                }

                if (sectionAttribute != null)
                {
                    sectionName = sectionAttribute.Name;
                }

                switch (property.PropertyType.FullName)
                {
                    case "System.String":
                        configuration[sectionName][propertyName].StringValue = Convert.ToString(property.GetValue(this, null));
                        break;

                    case "System.Boolean":
                        configuration[sectionName][propertyName].BoolValue = Convert.ToBoolean(property.GetValue(this, null));
                        break;

                    default:
                        throw new NotImplementedException($"Type \"{property.PropertyType.FullName}\" not supported by the INI parser.");
                        break;
                }
            }

            return configuration;
        }
    }
}
