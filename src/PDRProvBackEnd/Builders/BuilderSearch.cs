using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PDRProvBackEnd.Builders.Anotations;

namespace PDRProvBackEnd.Builders
{
    public class BuilderSearch<T> where T : class
    {
        public BuilderSearch()
        {

        }

        public Expression<Func<T, bool>> Search(string search)
        {
            if (search != null)
            {
                search = search.ToLower();
                var columsSearch = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(Search), false).Any());
                if (!columsSearch.Any()) {
                    return null;
                }
                ParameterExpression entity = Expression.Parameter(typeof(T));
                Expression query = null;
                foreach (var columSearch in columsSearch)
                {
                    var expressContain = prepareQueryMethodInfo(entity, columSearch, search);
                    if (query == null) { query = expressContain; } else { query = Expression.Or(query, expressContain); }
                }

                return Expression.Lambda<Func<T, bool>>(query, new ParameterExpression[] { entity });
            }else
            {
                return null;
            }
        }

        private Expression prepareQueryMethodInfo(ParameterExpression entity, PropertyInfo colum, string value)
        {
            var expressionProperty = Expression.Property(entity, colum.Name);
            Type type = colum.PropertyType;
            bool IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((type == typeof(DateTime) || type.GenericTypeArguments.Contains(typeof(DateTime))) && Types.GetType(value) == typeof(DateTime))
            {
                if (IsNullable(type))
                {
                    return Expression.Equal(Expression.PropertyOrField(expressionProperty, "Value"), Expression.Constant(DateTime.Parse(value)));
                }
                else
                {
                    return Expression.Equal(expressionProperty, Expression.Constant(DateTime.Parse(value)));
                }
            }
            if ((type == typeof(int) || type.GenericTypeArguments.Contains(typeof(int))) && Types.GetType(value) == typeof(int))
            {
                if (IsNullable(type))
                {
                    return Expression.Equal(Expression.PropertyOrField(expressionProperty, "Value"), Expression.Constant(int.Parse(value)));
                }
                else
                {
                    return Expression.Equal(expressionProperty, Expression.Constant(int.Parse(value)));
                }
            }

            if (IsNullable(colum.PropertyType))
            {
                Expression isnull = Expression.NotEqual(expressionProperty, Expression.Constant(null));
                Expression expressToString = Expression.Call(expressionProperty, colum.PropertyType.GetMethod("ToString", new Type[0]));
                expressToString = Expression.Call(expressToString, typeof(string).GetMethod("ToLower", new Type[0]));
                Expression contain = Expression.Call(expressToString, typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(value));
                return Expression.And(isnull, contain);
            }
            else
            {
                Expression expressToString = Expression.Call(expressionProperty, colum.PropertyType.GetMethod("ToString", new Type[0]));

                if (typeof(string) == colum.PropertyType)
                {
                    expressToString = Expression.Call(expressionProperty, typeof(string).GetMethod("ToLower", new Type[0]));
                    Expression isnull = Expression.NotEqual(expressionProperty, Expression.Constant(null));
                    return Expression.And(isnull, Expression.Call(expressToString, colum.PropertyType.GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(value)));
                }
                else
                {
                    expressToString = Expression.Call(expressToString, typeof(string).GetMethod("ToLower", new Type[0]));
                    return Expression.Call(expressToString, typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(value));
                }
            }

        }

    }
}