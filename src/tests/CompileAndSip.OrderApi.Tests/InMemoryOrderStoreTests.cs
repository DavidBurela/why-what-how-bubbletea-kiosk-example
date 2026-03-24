using CompileAndSip.OrderApi;
using FluentAssertions;

namespace CompileAndSip.OrderApi.Tests;

public sealed class InMemoryOrderStoreTests
{
    private static Order CreateTestOrder(int orderNumber = 1, OrderStatus targetStatus = OrderStatus.Created)
    {
        var drink = new Drink("classic-milk-tea", "Classic Milk Tea", "Test", 5.50m);
        var customisation = new DrinkCustomisation();
        var item = new OrderItem(drink, customisation, 5.50m);
        var order = new Order(item, orderNumber);
        if (targetStatus == OrderStatus.Paid || targetStatus == OrderStatus.Complete)
            order.MarkPaid();
        if (targetStatus == OrderStatus.Complete)
            order.MarkComplete();
        return order;
    }

    [Fact]
    public void Add_ThenGetById_ReturnsOrder()
    {
        var store = new InMemoryOrderStore();
        var order = CreateTestOrder();
        store.Add(order);
        var retrieved = store.GetById(order.Id);
        retrieved.Should().BeSameAs(order);
    }

    [Fact]
    public void GetActiveOrders_ReturnsPaidOnly()
    {
        var store = new InMemoryOrderStore();
        var created = CreateTestOrder(1, OrderStatus.Created);
        var paid = CreateTestOrder(2, OrderStatus.Paid);
        var complete = CreateTestOrder(3, OrderStatus.Complete);
        store.Add(created);
        store.Add(paid);
        store.Add(complete);

        var active = store.GetActiveOrders();
        active.Should().HaveCount(1);
        active[0].OrderNumber.Should().Be(2);
    }

    [Fact]
    public void NextOrderNumber_IsSequential()
    {
        var store = new InMemoryOrderStore();
        store.NextOrderNumber().Should().Be(1);
        store.NextOrderNumber().Should().Be(2);
        store.NextOrderNumber().Should().Be(3);
    }
}
