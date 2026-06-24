using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AisSysUtils
{


    public class aisEnum
    {
        public static Type GetTypeConverter(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                TypeConverterAttribute[] attributes =
                    (TypeConverterAttribute[])fi.GetCustomAttributes(
                    typeof(TypeConverterAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return Type.GetType(attributes[0].ConverterTypeName);
                else
                    return null;
            }
            return null;
        }

        public static string GetTypeConverterName(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                TypeConverterAttribute[] attributes =
                    (TypeConverterAttribute[])fi.GetCustomAttributes(
                    typeof(TypeConverterAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].ConverterTypeName;
                else
                    return null;
            }
            return null;
        }


        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            return value.ToString();
        }

        public static object GetEnumAttibute(Enum value, Type attributeType, String propName)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                object[] attributes = fi.GetCustomAttributes(attributeType, false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributeType.GetProperty(propName).GetValue(attributes[0], null);
                else
                    return value.ToString();
            }
            return value.ToString();
        }

        public static T GetEnumAttibute<T>(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                object[] attributes = fi.GetCustomAttributes(typeof(T), false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return (T)attributes[0];
                return default(T);
            }
            return default(T);
        }



        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
        }

        public static Type GetEnumType(string enumName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(enumName);
                if (type == null)
                    continue;
                if (type.IsEnum)
                    return type;
            }
            return null;
        }
    }

}
