using BusinessObjects;
using Microsoft.Extensions.Configuration; // Để đọc cấu hình từ appsettings.json

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO instance = null;
        private readonly FunewsManagementFall2024Context context;

        private SystemAccountDAO()
        {
            context = new FunewsManagementFall2024Context();
        }

        public static SystemAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemAccountDAO();
                }
                return instance;
            }
        }

        public List<SystemAccount> GetSystemAccounts()
        {
            var accountList = new List<SystemAccount>();

            var adminAccount = GetAdminAccount();
            accountList.Add(adminAccount);

            var dbAccounts = context.SystemAccounts
                                    .Where(account => account.AccountEmail != adminAccount.AccountEmail)
                                    .ToList();
            accountList.AddRange(dbAccounts);

            return accountList;
        }

        public SystemAccount Login(string email, string password)
        {
            List<SystemAccount> accounts = GetSystemAccounts();
            SystemAccount account = accounts
                                    .Where(account => account.AccountEmail == email && account.AccountPassword == password)
                                    .FirstOrDefault();
            return account;
        }

        private SystemAccount GetAdminAccount()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();

            var adminAccount = new SystemAccount
            {
                AccountId = 0,
                AccountName = "Admin",
                AccountEmail = configuration["AdminAccount:Email"],
                AccountPassword = configuration["AdminAccount:Password"],
                AccountRole = 0
            };

            return adminAccount;
        }
    }
}
