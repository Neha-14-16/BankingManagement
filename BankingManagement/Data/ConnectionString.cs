namespace BankingManagement.Data
{
    public static class ConnectionString
    {
        public static string GetConnectionString()
        {
            return @"Data Source=Neha-Laptop;
                     Initial Catalog=BankingManagementDB;
                     Integrated Security=True";
        }
    }
}