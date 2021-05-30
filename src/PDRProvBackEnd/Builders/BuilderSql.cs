using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PDRProvBackEnd.Builders
{
    public class BuilderSql
    {
        DbContext context;
        public BuilderSql(DbContext context) {
            this.context = context;
        }
        public Expression<Func<T, bool>> BuildQueryPrimaryKey<T>(List<String> list) where T : class
        {
            
            var p = Expression.Parameter(typeof(T));
            List<String> PkList = GetKeyNames<T>(context);
            int i = 0;
            Expression query = null;
            foreach(var pk in PkList) {
                var x = Expression.Property(p, pk);
                var c = Expression.Constant(Convert.ChangeType(list[i], x.Type));
                if (query == null) {
                    query = Expression.Equal(x, c);
                    
                } else {
                    query = Expression.And(query, Expression.Equal(x, c));
                }
                i++;


            }
            return Expression.Lambda<Func<T, bool>>(query, new ParameterExpression[] { p });
        }

        public List<String> GetKeyNames<T>(DbContext context) where T : class
        {
            var keysName = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).ToList();
            
            return keysName;
        }
    }
}