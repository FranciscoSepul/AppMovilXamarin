using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using PDRProvBackEnd.Builders.Anotations;
using PDRProvBackEnd.DTOModels;

namespace PDRProvBackEnd.Builders
{
    public class BuilderFilter<T> where T : class
    {
        public Expression<Func<T, Boolean>> GetFilterExpression(List<FilterModel> FilterList)
        {
            if (FilterList != null && FilterList.Count() > 0)
            {
                ParameterExpression entity = Expression.Parameter(typeof(T));
                Expression query = null;
                foreach (var filter in FilterList)
                {
                    query = GetQuery(entity, query, filter);
                }
                if (query != null)
                {
                    return Expression.Lambda<Func<T, Boolean>>(query, new ParameterExpression[] { entity });
                }
            }

            return null;
        }

        private Expression GetQuery(ParameterExpression entity, 
            Expression query, 
            FilterModel filter,
            LogicType logicType = LogicType.AND)
        {
            PropertyInfo columFilter = typeof(T).GetProperties().Where(p =>
                p.Name.ToLower().Trim() == filter.Prop.ToLower().Trim()).FirstOrDefault();

            if (columFilter != null)
            {
                var expressContain = prepareQueryMethodInfo(entity, columFilter, filter);
                if (expressContain != null)
                {
                    if (query == null) 
                    { 
                        query = expressContain; 
                    }
                    else
                    {
                        if (logicType == LogicType.OR)
                            query = Expression.OrElse(query, expressContain);
                        else
                            query = Expression.AndAlso(query, expressContain);

                        if (filter.LogicType != null && filter.NextCond != null)
                        {
                            query = GetQuery(entity, query, filter.NextCond, filter.LogicType.Value);
                        }
                    }

                }
            }
            else
            {
                throw new Exception($"La columana enviada {filter.Prop}, no coincide " +
                    $"con el modelo: {typeof(T).Name}");
            }

            return query;
        }

        private Expression prepareQueryMethodInfo(ParameterExpression entity, 
            PropertyInfo colum, FilterModel filt)
        {
            Expression expressionProperty = Expression.Property(entity, colum.Name);
            switch (filt.CondType)
            {
                case Anotations.TypeFilter.EQUAL:
                    return prepareExpressionEqual(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.NOT_EQUAL:
                    return prepareExpressionNotEqual(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.LESS:
                    return prepareExpressionLess(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.LESSE_EQUAL:
                    return prepareExpressionLesseEqual(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.GREATER:
                    return prepareExpressionGreater(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.GREATER_EQUAL:
                    return prepareExpressionGreaterEqual(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.BETWEEN:
                    return prepareExpressionBetween(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.BETWEEN_INCLUSIVE:
                    return prepareExpressionInclusive(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.IN:
                    return prepareExpressionIn(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.NOT_IN:
                    return prepareExpressionNotIn(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.CONTAIN:
                    return prepareExpressionContain(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.START_CONTAIN:
                    return prepareExpressionStartContain(expressionProperty, filt.Value, colum);
                case Anotations.TypeFilter.END_CONTAIN:
                    return prepareExpressionEndContain(expressionProperty, filt.Value, colum);
                default:
                    return prepareExpressionEqual(expressionProperty, filt.Value, colum);
            }
        }

        public Expression prepareExpressionEqual(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && !Columntype.GenericTypeArguments.Contains(typeof(String))))
            {
                if(value == null)
                    return Expression.Equal(expressionProperty, Expression.Constant(null));

                Object v = value;
                if (Columntype.IsEnum) {
                    v = Enum.Parse(Columntype, Convert.ToString(value));
                }

                if ((Columntype == v.GetType() || Columntype == Types.GetType(value) 
                    || Columntype.GenericTypeArguments.Contains(Types.GetType(value))))
                {
                    Expression exprConst;
                    exprConst = Expression.Constant(Types.ChangeType(value));

                    if (Columntype.IsEnum)
                    {
                        exprConst = Expression.Constant(v);
                    }
                    if (IsNullable(Columntype))
                    {
                        return Expression.Equal(Expression.PropertyOrField(expressionProperty, "Value"), exprConst);
                    }
                    else
                    {
                        return Expression.Equal(expressionProperty, exprConst);
                    }
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no " +
                        $"corresponde al valor : {value} {typeof(T).Name}");
                }
            }
            else
            {

                if (IsNullable(Columntype))
                {
                    Expression isnull = Expression.NotEqual(expressionProperty, Expression.Constant(null));
                    Expression expressToString = 
                        Expression.Call(expressionProperty, Columntype.GetMethod("ToString", new Type[0]));
                    expressToString = 
                        Expression.Call(expressToString, typeof(String).GetMethod("ToLower", new Type[0]));
                    Expression equal = 
                        Expression.Equal(expressToString, Expression.Constant(value.ToString()));
                    return Expression.And(isnull, equal);
                }
                else
                {
                    Expression expressToLower = Expression.Call(expressionProperty, typeof(String).GetMethod("ToLower", new Type[0]));
                    return Expression.Equal(expressToLower, Expression.Constant(value.ToString().ToLower()));
                }

            }

        }

        public Expression prepareExpressionNotEqual(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && !Columntype.GenericTypeArguments.Contains(typeof(String))))
            {
                if (value == null)
                    return Expression.NotEqual(expressionProperty, Expression.Constant(null));

                Object v = value;
                if (Columntype.IsEnum)
                {
                    v = Enum.Parse(Columntype, Convert.ToString(value));
                }

                if ((Columntype == v.GetType() || 
                    Columntype == Types.GetType(value) || 
                    Columntype.GenericTypeArguments.Contains(Types.GetType(value))))
                {
                    if (IsNullable(Columntype))
                    {

                        return Expression.NotEqual(Expression.PropertyOrField(expressionProperty, "Value"), Expression.Constant(Types.ChangeType(value)));
                    }
                    else
                    {
                        return Expression.NotEqual(expressionProperty, Expression.Constant(Types.ChangeType(value)));
                    }
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde al valor : {value} - {typeof(T).Name}");
                }
            }
            else
            {

                if (IsNullable(Columntype))
                {
                    Expression isnull = Expression.NotEqual(expressionProperty, Expression.Constant(null));
                    Expression expressToString = Expression.Call(expressionProperty, Columntype.GetMethod("ToString", new Type[0]));
                    expressToString = Expression.Call(expressToString, typeof(String).GetMethod("ToLower", new Type[0]));
                    Expression equal = Expression.NotEqual(expressToString, Expression.Constant(value.ToString()));
                    return Expression.And(isnull, equal);
                }
                else
                {
                    Expression expressToLower = Expression.Call(expressionProperty, typeof(String).GetMethod("ToLower", new Type[0]));
                    return Expression.NotEqual(expressToLower, Expression.Constant(value.ToString().ToLower()));
                }

            }
            
        }

        public Expression prepareExpressionLess(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && 
                !Columntype.GenericTypeArguments.Contains(typeof(String))) && 
                (Columntype != typeof(Boolean) && !Columntype.GenericTypeArguments.Contains(typeof(Boolean))))
            {
                if ((Columntype == Types.GetType(value) || Columntype.GenericTypeArguments.Contains(Types.GetType(value))))
                {
                    if (IsNullable(Columntype))
                    {

                        return Expression.LessThan(Expression.PropertyOrField(expressionProperty, "Value"), Expression.Constant(Types.ChangeType(value)));
                    }
                    else
                    {
                        return Expression.LessThan(expressionProperty, Expression.Constant(Types.ChangeType(value)));
                    }
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde al valor : {value} - {typeof(T).Name}");
                }
            }
            else
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de operador del filtro. {typeof(T).Name}");
            }
        }

        public Expression prepareExpressionLesseEqual(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && 
                !Columntype.GenericTypeArguments.Contains(typeof(String))) && 
                (Columntype != typeof(Boolean) && !Columntype.GenericTypeArguments.Contains(typeof(Boolean))))
            {
                if ((Columntype == Types.GetType(value) || Columntype.GenericTypeArguments.Contains(Types.GetType(value))))
                {
                    if (IsNullable(Columntype))
                    {

                        return Expression.LessThanOrEqual(Expression.PropertyOrField(expressionProperty, "Value"), Expression.Constant(Types.ChangeType(value)));
                    }
                    else
                    {
                        return Expression.LessThanOrEqual(expressionProperty, Expression.Constant(Types.ChangeType(value)));
                    }
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde al valor : {value} - {typeof(T).Name}");
                }
            }
            else
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de operador del filtro. {typeof(T).Name}");
            }
        }

        public Expression prepareExpressionGreater(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && 
                !Columntype.GenericTypeArguments.Contains(typeof(String))) && 
                (Columntype != typeof(Boolean) && !Columntype.GenericTypeArguments.Contains(typeof(Boolean))))
            {
                if ((Columntype == Types.GetType(value) || Columntype.GenericTypeArguments.Contains(Types.GetType(value))))
                {
                    if (IsNullable(Columntype))
                    {

                        return Expression.GreaterThan(Expression.PropertyOrField(expressionProperty, "Value"), Expression.Constant(Types.ChangeType(value)));
                    }
                    else
                    {
                        return Expression.GreaterThan(expressionProperty, Expression.Constant(Types.ChangeType(value)));
                    }
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde al valor : {value} - {typeof(T).Name}");
                }
            }
            else
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de operador del filtro. {typeof(T).Name}");
            }
        }

        public Expression prepareExpressionGreaterEqual(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && 
                !Columntype.GenericTypeArguments.Contains(typeof(String))) && 
                (Columntype != typeof(Boolean) && !Columntype.GenericTypeArguments.Contains(typeof(Boolean))))
            {
                if ((Columntype == Types.GetType(value) || Columntype.GenericTypeArguments.Contains(Types.GetType(value))))
                {
                    if (IsNullable(Columntype))
                    {

                        return Expression.GreaterThanOrEqual(Expression.PropertyOrField(expressionProperty, "Value"), Expression.Constant(Types.ChangeType(value)));
                    }
                    else
                    {
                        return Expression.GreaterThanOrEqual(expressionProperty, Expression.Constant(Types.ChangeType(value)));
                    }
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde al valor : {value} - {typeof(T).Name}");
                }
            }
            else
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de operador del filtro. {typeof(T).Name}");
            }
        }

        public Expression prepareExpressionBetween(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            String[] values = Convert.ToString(value).Split(";");
            if ((Convert.ToString(value).Split(";").Length == 2) && 
                (Columntype != typeof(String) && 
                !Columntype.GenericTypeArguments.Contains(typeof(String))) && 
                (Columntype != typeof(Boolean) && 
                !Columntype.GenericTypeArguments.Contains(typeof(Boolean))))
            {
                if ((Columntype == Types.GetType(values[0]) && Columntype == Types.GetType(values[1])) || (Columntype.GenericTypeArguments.Contains(Types.GetType(values[0])) && Columntype.GenericTypeArguments.Contains(Types.GetType(values[1]))))
                {
                    return Expression.And(prepareExpressionGreater(expressionProperty, values[0], colum), prepareExpressionLess(expressionProperty, values[1], colum));
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de los valores. {typeof(T).Name}");
                }
            }
            else
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de operador del filtro. {typeof(T).Name}");
            }
        }

        public Expression prepareExpressionInclusive(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            String[] values = Convert.ToString(value).Split(";");
            if ((Convert.ToString(value).Split(";").Length == 2) && 
                (Columntype != typeof(String) && 
                !Columntype.GenericTypeArguments.Contains(typeof(String))) && 
                (Columntype != typeof(Boolean) && !Columntype.GenericTypeArguments.Contains(typeof(Boolean))))
            {
                if ((Columntype == Types.GetType(values[0]) && Columntype == Types.GetType(values[1])) || (Columntype.GenericTypeArguments.Contains(Types.GetType(values[0])) && Columntype.GenericTypeArguments.Contains(Types.GetType(values[1]))))
                {
                    return Expression.And(prepareExpressionGreaterEqual(expressionProperty, values[0], colum), prepareExpressionLesseEqual(expressionProperty, values[1], colum));
                }
                else
                {
                    throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de los valores. {typeof(T).Name}");
                }
            }
            else
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no corresponde con el tipo de operador del filtro. {typeof(T).Name}");
            }
        }

        public Expression prepareExpressionIn(Expression expressionProperty, Object value, PropertyInfo Column)
        {
            Expression constant = null;
            var isdate = Column.PropertyType == typeof(DateTime) || 
                Column.PropertyType.GenericTypeArguments.Contains(typeof(DateTime));
            if (isdate)
            {
                try
                {
                    var tmp = Convert.ToString(value).Split(";").Select(d => DateTime.Parse(d)).ToList();
                    if (Types.IsNullable(Column.PropertyType))
                    {
                        constant = Expression.Constant(tmp.Select(t => t as DateTime?).ToList());
                    }
                    else
                    {
                        constant = Expression.Constant(tmp);
                    }

                }
                catch
                {
                    throw new Exception($"El valor enviado no es valido: {value}, debe tener el siguente fotmato:YYYY-MM-ddTHH:mm:ss");
                }
            }
            Type typeList = isdate ? Types.IsNullable(Column.PropertyType) ? typeof(List<DateTime?>) : typeof(List<DateTime>) : typeof(List<String>);
            var Columntype = Column.PropertyType;
            expressionProperty = isdate ? expressionProperty : Expression.Call(expressionProperty, Column.PropertyType.GetMethod("ToString", new Type[0]));
            constant = constant ?? Expression.Constant(Convert.ToString(value).Split(";").ToList());
            return Expression.Call(constant, typeList.GetMethod("Contains", new[] { isdate ? Column.PropertyType : typeof(String) }), expressionProperty);

        }

        public Expression prepareExpressionNotIn(Expression expressionProperty, Object value, PropertyInfo Column)
        {

            return Expression.Not(prepareExpressionIn(expressionProperty, value, Column));

        }

        public Expression prepareExpressionContain(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && !Columntype.GenericTypeArguments.Contains(typeof(String))))
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no " +
                    $"corresponde al condicional Contain valor : {value} {typeof(T).Name}");
            }
            else
            {
                if (IsNullable(Columntype))
                {
                    Expression isnull = Expression.NotEqual(expressionProperty, Expression.Constant(null));

                    Expression expressToString =
                        Expression.Call(expressionProperty, Columntype.GetMethod("ToString", new Type[0]));
                    expressToString =
                        Expression.Call(expressToString, typeof(String).GetMethod("ToLower", new Type[0]));
                    Expression contain = Expression.Call(expressToString, 
                        typeof(String).GetMethod("Contains", new[] { typeof(String) }), 
                        Expression.Constant(Convert.ToString(value).ToLower()));

                    return Expression.And(isnull, contain);
                }
                else
                {
                    Expression expressToLower = Expression.Call(expressionProperty, typeof(String).GetMethod("ToLower", new Type[0]));
                    return Expression.Call(expressToLower,
                        typeof(String).GetMethod("Contains", new[] { typeof(String) }),
                        Expression.Constant(Convert.ToString(value).ToLower()));
                }
                
            }

        }

        public Expression prepareExpressionStartContain(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && !Columntype.GenericTypeArguments.Contains(typeof(String))))
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no " +
                    $"corresponde al condicional Contain valor : {value} {typeof(T).Name}");
            }
            else
            {
                if (IsNullable(Columntype))
                {
                    Expression isnull = Expression.NotEqual(expressionProperty, Expression.Constant(null));

                    Expression expressToString =
                        Expression.Call(expressionProperty, Columntype.GetMethod("ToString", new Type[0]));
                    expressToString =
                        Expression.Call(expressToString, typeof(String).GetMethod("ToLower", new Type[0]));
                    Expression startsContain = Expression.Call(expressToString,
                        typeof(String).GetMethod("StartsWith", new[] { typeof(String) }),
                        Expression.Constant(Convert.ToString(value).ToLower()));
                    
                    return Expression.And(isnull, startsContain);
                }
                else
                {
                    Expression expressToLower = Expression.Call(expressionProperty, typeof(String).GetMethod("ToLower", new Type[0]));
                    return Expression.Call(expressToLower,
                        typeof(String).GetMethod("StartsWith", new[] { typeof(String) }),
                        Expression.Constant(Convert.ToString(value).ToLower()));
                }

            }

        }

        public Expression prepareExpressionEndContain(Expression expressionProperty, Object value, PropertyInfo colum)
        {
            Type Columntype = colum.PropertyType;
            Boolean IsNullable(Type typeChecking) => Nullable.GetUnderlyingType(typeChecking) != null;
            if ((Columntype != typeof(String) && !Columntype.GenericTypeArguments.Contains(typeof(String))))
            {
                throw new Exception($"El tipo de dato de la columna {colum.Name} no " +
                    $"corresponde al condicional Contain valor : {value} {typeof(T).Name}");
            }
            else
            {
                if (IsNullable(Columntype))
                {
                    Expression isnull = Expression.NotEqual(expressionProperty, Expression.Constant(null));

                    Expression expressToString =
                        Expression.Call(expressionProperty, Columntype.GetMethod("ToString", new Type[0]));
                    expressToString =
                        Expression.Call(expressToString, typeof(String).GetMethod("ToLower", new Type[0]));
                    Expression endsContain = Expression.Call(expressToString,
                        typeof(String).GetMethod("EndsWith", new[] { typeof(String) }),
                        Expression.Constant(Convert.ToString(value).ToLower()));
                    
                    return Expression.And(isnull, endsContain);
                }
                else
                {
                    Expression expressToLower = Expression.Call(expressionProperty, typeof(String).GetMethod("ToLower", new Type[0]));
                    return Expression.Call(expressToLower,
                        typeof(String).GetMethod("EndsWith", new[] { typeof(String) }),
                        Expression.Constant(Convert.ToString(value).ToLower()));
                }
            }

        }

        public static FilterModel GetFilterFromString(KeyValuePair<String, StringValues> fill)
        {
            FilterModel objfill = new FilterModel();
            List<String> value = fill.Value.ToString().Trim().Split(";").ToList<String>();
            if (OperatorFilter.MapOperators.ContainsKey(value[0]))
            {
                objfill.CondType = OperatorFilter.MapOperators.GetValueOrDefault(value[0]);
                objfill.Prop = fill.Key;
                objfill.Value = fill.Value.ToString().Trim().Remove(0, value[0].Length + 1);
            }
            else
            {
                if (value.Count() == 1)
                {
                    objfill.CondType = OperatorFilter.MapOperators.GetValueOrDefault(":");
                    objfill.Prop = fill.Key;
                    objfill.Value = value[0];
                }
                else
                {
                    throw new Exception($"Favor indicar un operador valido para el filtro {fill.Key}");
                }
            }
            return objfill;
        }

    }

    public class FilterObject
    {
        public String Column { get; set; }
        public Anotations.TypeFilter typeFilter { get; set; }
        public String value { get; set; }

        public static FilterObject ValidFilters(KeyValuePair<String, StringValues> fill)
        {
            FilterObject objfill = new FilterObject();
            List<String> value = fill.Value.ToString().Trim().Split(";").ToList<String>();
            if (OperatorFilter.MapOperators.ContainsKey(value[0]))
            {
                objfill.typeFilter = OperatorFilter.MapOperators.GetValueOrDefault(value[0]);
                objfill.Column = fill.Key;
                objfill.value = fill.Value.ToString().Trim().Remove(0, value[0].Length + 1);
            }
            else
            {
                if (value.Count() == 1)
                {
                    objfill.typeFilter = OperatorFilter.MapOperators.GetValueOrDefault(":");
                    objfill.Column = fill.Key;
                    objfill.value = value[0];
                }
                else
                {
                    throw new Exception($"Favor indicar un operador valido para el filtro {fill.Key}");
                }
            }
            return objfill;

        }

    }
}
