using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class DefaultConstructorTest
    {
        [Fact]
        public void GivenEmptyPaymentRequest_WhenMakingPayment_ThenNoExceptionsThrownAndSuccessEqualsFalse()
        {
            var paymentService = new PaymentService();
            var result = paymentService.MakePayment(new MakePaymentRequest());
            Assert.False(result.Success);
        }
    }
}