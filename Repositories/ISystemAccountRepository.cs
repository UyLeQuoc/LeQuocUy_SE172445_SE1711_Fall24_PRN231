using BusinessObjects;

namespace Repositories
{
    public interface ISystemAccountRepository
    {
        List<SystemAccount> GetSystemAccounts();
        SystemAccount GetSystemAccountById(short id);
        void AddSystemAccount(SystemAccount systemAccount);
        void UpdateSystemAccount(SystemAccount systemAccount);
        void DeleteSystemAccount(short id);
        SystemAccount Login(string email, string password);
    }
}
