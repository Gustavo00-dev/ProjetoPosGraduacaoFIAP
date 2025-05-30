namespace APIFCG.Configuracao
{
    public static class AzureMySqlConnectionHelper
    {
        public static string BuildConnectionString(
            string server,
            string database,
            string user,
            string password,
            int port = 3306,
            string sslMode = "Required")
        {
            return $"Server={server};Database={database};User={user};Password={password};Port={port};SslMode={sslMode};";
        }
    }
}
