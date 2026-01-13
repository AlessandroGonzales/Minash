using Npgsql;

namespace Infrastructure.Persistence.Configuration
{
    internal class PreserveCasingNameTranslator : INpgsqlNameTranslator
    {
        public string TranslateTypeName(string clrName) => clrName;
        public string TranslateMemberName(string clrName) => clrName;
    }
}
