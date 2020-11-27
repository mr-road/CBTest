using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _backupAccountDataStore;
        private readonly IDataStoreTypeProvider _dataStoreTypeProvider;
        private readonly IAccountDataStore _accountDataStore;

        public PaymentService()
        {
            _backupAccountDataStore = new BackupAccountDataStore();
            _accountDataStore = new AccountDataStore();
            _dataStoreTypeProvider = new DataStoreTypeProvider();
        }

        public PaymentService(
            IAccountDataStore mainAccountDataStore,
            IAccountDataStore backupAccountDataStore,
            IDataStoreTypeProvider dataStoreTypeProvider)
        {
            _backupAccountDataStore = backupAccountDataStore;
            _dataStoreTypeProvider = dataStoreTypeProvider;
            _accountDataStore = mainAccountDataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var accountDataStore = GetAccountDataStore();

            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var validationResult = AccountValidator.ValidateAccountCanSatisfyPaymentRequest(request, account);

            if (validationResult.Success)
            {
                account.Balance -= request.Amount;
                accountDataStore.UpdateAccount(account);
            }

            return validationResult;
        }

        private IAccountDataStore GetAccountDataStore()
        {
            var dataStoreType = _dataStoreTypeProvider.GetDataStoreType();

            IAccountDataStore accountDataStore;
            if (dataStoreType == "Backup")
            {
                accountDataStore = _backupAccountDataStore;
            }
            else
            {
                accountDataStore = _accountDataStore;
            }

            return accountDataStore;
        }
    }
}
