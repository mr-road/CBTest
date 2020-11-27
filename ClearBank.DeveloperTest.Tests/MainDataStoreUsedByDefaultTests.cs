using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class MainDataStoreUsedByDefaultTests
    {
        [Fact]
        public void GivenEmptyPaymentRequest_WhenMakingPayment_ThenNoExceptionsThrownAndSuccessEqualsFalse()
        {
            var mainAccountDataStore = new Mock< IAccountDataStore>();
            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var paymentService = new PaymentService(mainAccountDataStore.Object, backupAccountDataStore.Object);
            var debtorAccountNumber = "D123";

            var result = paymentService.MakePayment(new MakePaymentRequest{DebtorAccountNumber = debtorAccountNumber});
            
            Assert.False(result.Success);
            mainAccountDataStore.Verify(main => main.GetAccount(debtorAccountNumber), Times.Once);
            backupAccountDataStore.Verify(backup => backup.GetAccount(debtorAccountNumber), Times.Never);
        }
    }
}
