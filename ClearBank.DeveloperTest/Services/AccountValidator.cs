using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountValidator
    {
        public static MakePaymentResult ValidateAccountCanSatisfyPaymentRequest(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult();

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    ValidateBacs(account, result);
                    break;

                case PaymentScheme.FasterPayments:
                    ValidateFasterPayments(request, account, result);
                    break;

                case PaymentScheme.Chaps:
                    ValidateChaps(account, result);
                    break;
            }

            return result;
        }

        private static void ValidateChaps(Account account, MakePaymentResult result)
        {
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
        }

        private static void ValidateFasterPayments(MakePaymentRequest request, Account account, MakePaymentResult result)
        {
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
        }

        private static void ValidateBacs(Account account, MakePaymentResult result)
        {
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
        }
    }
}