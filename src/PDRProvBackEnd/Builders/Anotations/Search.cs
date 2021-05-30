namespace PDRProvBackEnd.Builders.Anotations
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Search : System.Attribute
    {
        TypeSearch typeSearch;
        public Search(TypeSearch type = TypeSearch.CONTAIN)
        {
            typeSearch = type;
        }

    }

    public enum TypeSearch
    {
        CONTAIN, START_CONTAIN, END_CONTAIN, EQUAL
    }
}