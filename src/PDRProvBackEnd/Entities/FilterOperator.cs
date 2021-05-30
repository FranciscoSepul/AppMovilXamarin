using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Entities
{
    public static class FilterOperator
    {
        public const string Equal = "=";
        public const string NotEqual = "!=";
        public const string GreaterThan = ">";
        public const string SmallerThan = "<";
        public const string GreaterOrEqualThan = ">=";
        public const string SmallerOrEqualThan = "<=";
    }
}
