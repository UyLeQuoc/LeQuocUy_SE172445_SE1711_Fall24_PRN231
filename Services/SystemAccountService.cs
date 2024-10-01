using BusinessObjects;
using Repositories;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository systemAccountRepository;

        public SystemAccountService(ISystemAccountRepository repository)
        {
            this.systemAccountRepository = repository;
        }

        public List<SystemAccount> GetSystemAccounts()
        {
            return systemAccountRepository.GetSystemAccounts();
        }

        public SystemAccount GetSystemAccountById(short id)
        {
            return systemAccountRepository.GetSystemAccountById(id);
        }

        public void CreateSystemAccount(SystemAccount systemAccount)
        {
            systemAccountRepository.AddSystemAccount(systemAccount);
        }

        public void UpdateSystemAccount(SystemAccount systemAccount)
        {
            systemAccountRepository.UpdateSystemAccount(systemAccount);
        }

        public void RemoveSystemAccount(short id)
        {
            systemAccountRepository.DeleteSystemAccount(id);
        }

        public SystemAccount Login(string email, string password)
        {
            return systemAccountRepository.Login(email, password);
        }
    }
}
