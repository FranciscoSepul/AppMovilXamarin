using System.Collections.Generic;

namespace PDRProvBackEnd.Builders.Anotations
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Filter : System.Attribute
    {
        TypeFilter typeFilter;
        public Filter(TypeFilter type = TypeFilter.EQUAL)
        {
            typeFilter = type;
        }

    }

    public enum TypeFilter
    {
       EQUAL, NOT_EQUAL, LESS, GREATER, LESSE_EQUAL, 
        GREATER_EQUAL, BETWEEN, BETWEEN_INCLUSIVE, 
        IN, NOT_IN, CONTAIN, START_CONTAIN, END_CONTAIN
    }

    public enum LogicType
    {
        AND, OR
    }
    public static class OperatorFilter
    {
        const string  EQUAL = ":";
        const string  NOT_EQUAL = "!:";
        const string  LESS = "-";
        const string  GREATER = "*";
        const string  LESSE_EQUAL = "-:";
        const string  GREATER_EQUAL = "*:";
        const string  BETWEEN = "*-";
        const string  BETWEEN_INCLUSIVE = "*-:";
        const string  IN = "::";
        const string  NOT_IN = "!::";
        const string  CONTAIN = ":*:";
        const string  START_CONTAIN = ":*!";
        const string  END_CONTAIN = "!*:";

        public static Dictionary<string,TypeFilter> MapOperators = new Dictionary<string, TypeFilter>(){
            {EQUAL,TypeFilter.EQUAL},
            {NOT_EQUAL,TypeFilter.NOT_EQUAL},
            {LESS,TypeFilter.LESS},
            {GREATER,TypeFilter.GREATER},
            {LESSE_EQUAL,TypeFilter.LESSE_EQUAL},
            {GREATER_EQUAL,TypeFilter.GREATER_EQUAL},
            {BETWEEN,TypeFilter.BETWEEN},
            {BETWEEN_INCLUSIVE, TypeFilter.BETWEEN_INCLUSIVE},
            {IN,TypeFilter.IN},
            {NOT_IN, TypeFilter.NOT_IN},
            {CONTAIN, TypeFilter.CONTAIN},
            {START_CONTAIN, TypeFilter.START_CONTAIN},
            {END_CONTAIN, TypeFilter.END_CONTAIN}
        };

    }
}