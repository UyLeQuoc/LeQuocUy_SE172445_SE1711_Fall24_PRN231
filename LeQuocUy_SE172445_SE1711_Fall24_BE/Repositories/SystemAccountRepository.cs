using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public List<SystemAccount> GetSystemAccounts()
        {
            return SystemAccountDAO.Instance.GetSystemAccounts();
        }

        public SystemAccount GetSystemAccountById(short id)
        {
            return SystemAccountDAO.Instance.GetSystemAccounts()
                .FirstOrDefault(account => account.AccountId == id);
        }

        public void AddSystemAccount(SystemAccount systemAccount)
        {
            SystemAccountDAO.Instance.AddSystemAccount(systemAccount);
        }

        public void UpdateSystemAccount(SystemAccount systemAccount)
        {
            SystemAccountDAO.Instance.UpdateSystemAccount(systemAccount);
        }

        public void DeleteSystemAccount(short id)
        {
            SystemAccountDAO.Instance.DeleteSystemAccount(id);
        }

        public SystemAccount Login(string email, string password)
        {
            return SystemAccountDAO.Instance.Login(email, password);
        }
    }
}
