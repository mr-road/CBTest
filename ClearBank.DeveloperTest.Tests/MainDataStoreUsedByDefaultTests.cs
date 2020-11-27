using System;
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
        public void GivenBasicPaymentRequest_WhenMakingPayment_ThenSuccessEqualsFalseAndDefaultToMainDataStore()
        {
            var mainAccountDataStore = new Mock<IAccountDataStore>();
            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var paymentService = new PaymentService(mainAccountDataStore.Object, backupAccountDataStore.Object, new DataStoreTypeProvider());
            var debtorAccountNumber = "D123";

            var result = paymentService.MakePayment(new MakePaymentRequest { DebtorAccountNumber = debtorAccountNumber });

            Assert.False(result.Success);
            mainAccountDataStore.Verify(main => main.GetAccount(debtorAccountNumber), Times.Once);
            backupAccountDataStore.Verify(backup => backup.GetAccount(debtorAccountNumber), Times.Never);
        }

        [Fact]
        public void GivenValidBACSPaymentRequest_WhenMakingPayment_ThenSuccessEqualsTrueAndDefaultToMainDataStore()
        {
            var debtorAccountNumber = "D123";
            var mainAccountDataStore = new Mock<IAccountDataStore>();
            var validBacsAccount = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            mainAccountDataStore.Setup(main => main.GetAccount(debtorAccountNumber)).Returns(validBacsAccount);
            var backupAccountDataStore = new Mock<IAccountDataStore>();
            var paymentService = new PaymentService(
                mainAccountDataStore.Object,
                backupAccountDataStore.Object,
                new DataStoreTypeProvider());


            var validBacsPaymentRequest = new MakePaymentRequest
            {
                DebtorAccountNumber = debtorAccountNumber,
                Amount = 1,
                CreditorAccountNumber = "C123",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Bacs
            };

            var result = paymentService.MakePayment(validBacsPaymentRequest);

            Assert.True(result.Success);

            mainAccountDataStore.Verify(main => main.GetAccount(debtorAccountNumber), Times.Once);
            mainAccountDataStore.Verify(main => main.UpdateAccount(validBacsAccount), Times.Once);

            backupAccountDataStore.Verify(backup => backup.GetAccount(debtorAccountNumber), Times.Never);
            backupAccountDataStore.Verify(main => main.UpdateAccount(validBacsAccount), Times.Never);
        }

        [Fact]
        public void GivenValidBACSPaymentRequest_BackupSetInConfig_WhenMakingPayment_ThenSuccessEqualsTrueAndUsesBackupDataStore()
        {
            var debtorAccountNumber = "D123";
            var mainAccountDataStore = new Mock<IAccountDataStore>();
            var validBacsAccount = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };
            var backupAccountDataStore = new Mock<IAccountDataStore>();
            backupAccountDataStore.Setup(main => main.GetAccount(debtorAccountNumber)).Returns(validBacsAccount);
            var dataStoreTypeProvider = new Mock<IDataStoreTypeProvider>();
            dataStoreTypeProvider.Setup(dstp => dstp.GetDataStoreType()).Returns("Backup");
            var paymentService = new PaymentService(
                mainAccountDataStore.Object,
                backupAccountDataStore.Object,
                dataStoreTypeProvider.Object
                );

            var validBacsPaymentRequest = new MakePaymentRequest
            {
                DebtorAccountNumber = debtorAccountNumber,
                Amount = 1,
                CreditorAccountNumber = "C123",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Bacs
            };

            var result = paymentService.MakePayment(validBacsPaymentRequest);

            Assert.True(result.Success);

            mainAccountDataStore.Verify(main => main.GetAccount(debtorAccountNumber), Times.Never);
            mainAccountDataStore.Verify(main => main.UpdateAccount(validBacsAccount), Times.Never);

            backupAccountDataStore.Verify(backup => backup.GetAccount(debtorAccountNumber), Times.Once);
            backupAccountDataStore.Verify(main => main.UpdateAccount(validBacsAccount), Times.Once);
        }
    }
}
