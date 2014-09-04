namespace ShoppingCart.Tests
{
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class PriceCalculatorTests
    {
        private PriceCalculator calculator;
        private ICart cart;

        [SetUp]
        public void SetUp()
        {
            cart = A.Fake<ICart>();
            calculator = new PriceCalculator(cart);
        }

        [Test]
        public void Empty_cart_should_cost_nothing()
        {
            // Given
            A.CallTo(() => cart.Items).Returns(Enumerable.Empty<string>());

            // When
            var price = calculator.CalculatePrice();

            // Then
            price.Should().Be(0);
        }

        [Test]
        public void An_apple_should_cost_fifty()
        {
            A.CallTo(() => cart.Items).Returns(new List<string> { "A99" });

            var price = calculator.CalculatePrice();

            price.Should().Be(50);
        }

        [Test]
        public void Three_apples_should_cost_130()
        {
            A.CallTo(() => cart.Items).Returns(new List<string> { "A99", "A99", "A99" });

            var price = calculator.CalculatePrice();

            price.Should().Be(130);
        }

        [Test]
        public void Five_Biscuits_Cost_120()
        {
            A.CallTo(() => cart.Items).Returns(new List<string> { "B15", "B15", "B15", "B15", "B15" });

            var price = calculator.CalculatePrice();

            price.Should().Be(120);
        }

        [Test]
        public void Order_of_items_does_not_matter()
        {
            A.CallTo(() => cart.Items).Returns(new List<string> { "A99", "B15", "A99", "B15", "A99" });

            var price = calculator.CalculatePrice();

            price.Should().Be(175);
        }
    }
}