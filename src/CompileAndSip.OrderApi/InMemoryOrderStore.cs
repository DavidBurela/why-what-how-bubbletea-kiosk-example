using System.Collections.Concurrent;

namespace CompileAndSip.OrderApi;

public sealed class InMemoryOrderStore : IOrderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();
    private int _orderNumber;

    public void Add(Order order) => _orders[order.Id] = order;

    public Order? GetById(Guid id) => _orders.GetValueOrDefault(id);

    public IReadOnlyList<Order> GetActiveOrders() =>
        _orders.Values.Where(o => o.Status == OrderStatus.Paid).ToList();

    public int NextOrderNumber() => Interlocked.Increment(ref _orderNumber);
}
