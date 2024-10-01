using BusinessObjects;

namespace Services
{
    public interface ISystemAccountService
    {
        List<SystemAccount> GetSystemAccounts();
        SystemAccount GetSystemAccountById(short id);
        void CreateSystemAccount(SystemAccount systemAccount);
        void UpdateSystemAccount(SystemAccount systemAccount);
        void RemoveSystemAccount(short id);
        SystemAccount Login(string email, string password);
    }
}
