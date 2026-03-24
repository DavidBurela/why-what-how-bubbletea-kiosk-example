using CompileAndSip.OrderApi;
using FluentAssertions;

namespace CompileAndSip.OrderApi.Tests;

public sealed class OrderTests
{
    private static OrderItem CreateTestOrderItem()
    {
        var drink = new Drink("classic-milk-tea", "Classic Milk Tea", "Traditional black tea with milk and tapioca pearls. The original.", 5.50m);
        var customisation = new DrinkCustomisation();
        return new OrderItem(drink, customisation, 5.50m);
    }

    [Fact]
    public void NewOrder_HasCreatedStatus()
    {
        var order = new Order(CreateTestOrderItem(), 1);
        order.Status.Should().Be(OrderStatus.Created);
    }

    [Fact]
    public void MarkPaid_FromCreated_Succeeds()
    {
        var order = new Order(CreateTestOrderItem(), 1);
        order.MarkPaid();
        order.Status.Should().Be(OrderStatus.Paid);
    }

    [Fact]
    public void MarkComplete_FromPaid_Succeeds()
    {
        var order = new Order(CreateTestOrderItem(), 1);
        order.MarkPaid();
        order.MarkComplete();
        order.Status.Should().Be(OrderStatus.Complete);
    }

    [Fact]
    public void MarkComplete_FromCreated_Throws()
    {
        var order = new Order(CreateTestOrderItem(), 1);
        var act = () => order.MarkComplete();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkPaid_FromPaid_Throws()
    {
        var order = new Order(CreateTestOrderItem(), 1);
        order.MarkPaid();
        var act = () => order.MarkPaid();
        act.Should().Throw<InvalidOperationException>();
    }
}
