using BusinessObjects;
using Microsoft.Extensions.Configuration;

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

        public SystemAccount GetSystemAccountById(short id)
        {
            return context.SystemAccounts.FirstOrDefault(account => account.AccountId == id);
        }

        public void AddSystemAccount(SystemAccount systemAccount)
        {

            try
            {
                var accounts = GetSystemAccounts();
                short maxId = accounts.Max(a => a.AccountId);

                if (accounts.Any(a => a.AccountEmail == systemAccount.AccountEmail))
                {
                    throw new InvalidOperationException("Email already exists.");
                }

                systemAccount.AccountId = (short)(maxId + 1);
                context.SystemAccounts.Add(systemAccount);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateSystemAccount(SystemAccount systemAccount)
        {
            if (systemAccount.AccountId == 0)
            {
                throw new Exception("Not Update Admin Account");
            }
            var existingAccount = context.SystemAccounts.FirstOrDefault(a => a.AccountId == systemAccount.AccountId);
            if (existingAccount != null)
            {
                existingAccount.AccountName = systemAccount.AccountName;
                existingAccount.AccountEmail = systemAccount.AccountEmail;
                existingAccount.AccountRole = systemAccount.AccountRole;
                existingAccount.AccountPassword = systemAccount.AccountPassword;

                context.SaveChanges();
            }
        }

        public void DeleteSystemAccount(short id)
        {
            if (id == 0)
            {
                throw new InvalidOperationException("Cannot delete admin account.");
            }
            //Check account is associated with news articles
            NewsArticleDAO newsArticle = NewsArticleDAO.Instance;
            if (newsArticle.GetNewsArticles().Any(na => na.CreatedById == id))
            {
                throw new InvalidOperationException("Cannot delete account associated with news articles.");
            }

            var systemAccount = context.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
            if (systemAccount != null && !context.NewsArticles.Any(na => na.CreatedById == id))
            {
                context.SystemAccounts.Remove(systemAccount);
                context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Cannot delete account associated with news articles.");
            }
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
