using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

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
            var dataStoreType = _dataStoreTypeProvider.GetDataStoreType();

            Account account = null;

            if (dataStoreType == "Backup")
            {
                var accountDataStore = _backupAccountDataStore;
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }
            else
            {
                var accountDataStore = _accountDataStore;
                account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            }

            var result = ValidateAccountCanSatisfyPaymentRequest(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;

                if (dataStoreType == "Backup")
                {
                    var accountDataStore = _backupAccountDataStore;
                    accountDataStore.UpdateAccount(account);
                }
                else
                {
                    var accountDataStore = _accountDataStore;
                    accountDataStore.UpdateAccount(account);
                }
            }

            return result;
        }

        public static MakePaymentResult ValidateAccountCanSatisfyPaymentRequest(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult();

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }

                    break;

                case PaymentScheme.FasterPayments:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }

                    break;

                case PaymentScheme.Chaps:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }

                    break;
            }

            return result;
        }
    }

    public interface IDataStoreTypeProvider
    {
        string GetDataStoreType();
    }

    public class DataStoreTypeProvider : IDataStoreTypeProvider
    {
        public string GetDataStoreType()
        {
            return ConfigurationManager.AppSettings["DataStoreType"];
        }
    }
}
