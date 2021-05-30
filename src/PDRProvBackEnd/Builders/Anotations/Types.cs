using System;
using System.Globalization;

namespace PDRProvBackEnd.Builders.Anotations
{
    public class Types
    {
        public static Type GetType(String value)
        {
            if (IsDate(value)) { return typeof(DateTime); }
            if (IsInterger(value)) { return typeof(Int32); }
            if (IsBool(value)) { return typeof(Boolean); }
            return typeof(String);
        }

        public static Type GetType(Object value)
        {
            if (value == null)
                return null;

            if(DateTime.TryParse(Convert.ToString(value), out _))
                return typeof(DateTime);

            if(int.TryParse(Convert.ToString(value), out _))
                return typeof(Int32);

            if (bool.TryParse(Convert.ToString(value), out _))
                return typeof(Boolean);

            return typeof(String);
        }

        public static Object ChangeType(Object value)
        {
            if (value == null)
                return null;

            var vtype = Types.GetType(value);
            if (vtype == typeof(DateTime))
            {
                return Convert.ChangeType(Convert.ToString(value, CultureInfo.CurrentCulture.DateTimeFormat),
                    vtype,
                    CultureInfo.CurrentCulture.DateTimeFormat);
            }
            if (vtype == typeof(Boolean))
            {
                return Convert.ChangeType(Convert.ToString(value),
                    vtype);
            }
            if (vtype == typeof(Int32))
            {
                return Convert.ChangeType(Convert.ToString(value, CultureInfo.CurrentCulture.NumberFormat),
                    vtype, 
                    CultureInfo.CurrentCulture.NumberFormat);
            }

            return Convert.ChangeType(Convert.ToString(value),
                                         Types.GetType(value));
        }

        public static bool IsDate(string value)
        {
            try
            {
                DateTime.Parse(value);
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public static bool IsInterger(string value)
        {
            try
            {
                Int64.Parse(value);
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public static bool IsBool(string value)
        {
            try
            {
                bool.Parse(value);
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public static bool IsNullable(Type type) {
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}