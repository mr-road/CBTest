using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class BACSAccountValidationTests
    {
        [Fact]
        public void GivenBACSRequest_AndNullAccount_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
            Account account = null;
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenBACSRequest_AndAccountWithChaps_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }
        [Fact]
        public void GivenBACSRequest_AndAccountWithFaster_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenBACSRequest_AndAccountWithBACSPermissions_WhenValidatingAccount_ThenResultSuccessIsTrue()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.True(result.Success);
        }
    }

    public class FasterAccountValidationTests
    {
        [Fact]
        public void GivenFasterPaymentsRequest_AndNullAccount_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments };
            Account account = null;
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenFasterPaymentsRequest_AndAccountWithChaps_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }
        [Fact]
        public void GivenFasterPaymentsRequest_AndAccountWithBacs_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenFasterPaymentsRequest_AndAccountWithFasterPermissions_AndBalanceLessThatnAmount_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments, Amount = 10};
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 9};
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenFasterPaymentsRequest_AndAccountWithFasterPermissions_WhenValidatingAccount_ThenResultSuccessIsTrue()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.True(result.Success);
        }
    }

    public class ChapsAccountValidationTests
    {
        [Fact]
        public void GivenChapsRequest_AndNullAccount_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            Account account = null;
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenChapsRequest_AndAccountWithBacs_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }
        [Fact]
        public void GivenChapsRequest_AndAccountWithFaster_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenChapsRequest_AndAccountWithChapsPermissions_AndDISABLEDAccount_WhenValidatingAccount_ThenResultSuccessIsFalse()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            Account account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Disabled
            };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.False(result.Success);
        }

        [Fact]
        public void GivenChapsRequest_AndAccountWithChapsPermissions_AndDefaultStatus_WhenValidatingAccount_ThenResultSuccessIsTrue()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            Account account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
            };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.True(result.Success);
        }

        [Fact]
        public void GivenChapsRequest_AndAccountWithChapsPermissions_AndLiveStatus_WhenValidatingAccount_ThenResultSuccessIsTrue()
        {
            MakePaymentRequest request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            Account account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live
            };
            var result = PaymentService.ValidateAccountCanSatisfyPaymentRequest(request, account);
            Assert.True(result.Success);
        }
    }
}
