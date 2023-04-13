using Microsoft.Extensions.Configuration;
using System;

namespace ProjectManager.Shared.Helper
{
    public static class ConnectionHelper
    {
        public enum DatabaseName
        {
            MainConnectionString
        }

        private static string GetConnectionString(DatabaseName dbName, IConfiguration config)
        {
            var connectionString = config.GetConnectionString(dbName.ToString());
            if (connectionString == null)
                throw new Exception($"Connection string {dbName} not in config");

            return connectionString;
        }
    }
}
