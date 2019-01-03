using BleemSync.Extensions.PlayStationClassic.Core.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BleemSync.Extensions.PlayStationClassic.Core.Models
{
    public class Preference
    {
        public Preference() { }
        public Preference(string preferencesString)
        {
            var preferenceProperties = new Dictionary<string, string>();
            var preference = this;

            using (StringReader reader = new StringReader(preferencesString))
            {
                var line = "";

                do
                {
                    line = reader.ReadLine();

                    if (line != null)
                    {
                        var pieces = line.Split('=', 2);

                        preferenceProperties[pieces[0]] = pieces[1];
                    }
                } while (line != null);
            }

            var derivedType = GetType();

            foreach (var property in derivedType.GetProperties())
            {
                var attributes = property.GetCustomAttributes(true);

                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(PreferencePropertyAttribute))
                    {
                        var preferencePropertyAttribute = (PreferencePropertyAttribute)attribute;

                        if (preferenceProperties.Keys.Contains(preferencePropertyAttribute.Name))
                        {
                            var valueFromInput = preferenceProperties[preferencePropertyAttribute.Name];

                            switch (preferencePropertyAttribute.Name[0])
                            {
                                case 'i':
                                    property.SetValue(preference, Convert.ToInt32(valueFromInput), null);
                                    break;

                                case 'b':
                                    property.SetValue(preference, Convert.ToBoolean(valueFromInput), null);
                                    break;

                                case 'd':
                                    property.SetValue(preference, Convert.ToDouble(valueFromInput), null);
                                    break;

                                case 's':
                                    property.SetValue(preference, valueFromInput, null);
                                    break;

                                default:
                                    throw new InvalidDataException($"Preference {preferencePropertyAttribute.Name} is not formatted correctly. It must either start with 'i', 'b', 'd', or 's'.");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var preferenceProperties = new Dictionary<string, string>();

            var derivedType = GetType();
            var properties = derivedType.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);

                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(PreferencePropertyAttribute))
                    {
                        var preferencePropertyAttribute = (PreferencePropertyAttribute)attribute;

                        switch (Type.GetTypeCode(property.PropertyType))
                        {
                            case TypeCode.Int32:
                                if (!preferencePropertyAttribute.Name.StartsWith('i'))
                                {
                                    throw new InvalidCastException($"Preference property {preferencePropertyAttribute.Name} is defined as an integer, but the property name does not start with \"i\".");
                                }

                                preferenceProperties[preferencePropertyAttribute.Name] = Convert.ToString(property.GetValue(this, null));
                                break;

                            case TypeCode.Boolean:
                                if (!preferencePropertyAttribute.Name.StartsWith('b'))
                                {
                                    throw new InvalidCastException($"Preference property {preferencePropertyAttribute.Name} is defined as an boolean, but the property name does not start with \"b\".");
                                }

                                preferenceProperties[preferencePropertyAttribute.Name] = Convert.ToString(property.GetValue(this, null));
                                break;

                            case TypeCode.Double:
                                if (!preferencePropertyAttribute.Name.StartsWith('d'))
                                {
                                    throw new InvalidCastException($"Preference property {preferencePropertyAttribute.Name} is defined as an double, but the property name does not start with \"d\".");
                                }

                                preferenceProperties[preferencePropertyAttribute.Name] = Convert.ToString(property.GetValue(this, null));
                                break;

                            case TypeCode.String:
                                if (!preferencePropertyAttribute.Name.StartsWith('s'))
                                {
                                    throw new InvalidCastException($"Preference property {preferencePropertyAttribute.Name} is defined as an string, but the property name does not start with \"s\".");
                                }

                                preferenceProperties[preferencePropertyAttribute.Name] = Convert.ToString(property.GetValue(this, null));
                                break;

                            default:
                                throw new NotImplementedException("A preference property must be a string, int, double, or boolean.");
                                break;
                        }
                    }
                }
            }


            // Properties are converted to string properly, now build the mega string
            var sb = new StringBuilder();

            foreach (var key in preferenceProperties.Keys)
            {
                sb.AppendLine($"{key}={preferenceProperties[key]}");
            }

            return sb.ToString();
        }
    }
}
