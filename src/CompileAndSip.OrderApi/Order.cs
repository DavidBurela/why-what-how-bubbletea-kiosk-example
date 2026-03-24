namespace CompileAndSip.OrderApi;

public sealed class Order
{
    public Guid Id { get; } = Guid.NewGuid();
    public int OrderNumber { get; }
    public OrderItem Item { get; }
    public OrderStatus Status { get; private set; } = OrderStatus.Created;
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

    public Order(OrderItem item, int orderNumber)
    {
        Item = item;
        OrderNumber = orderNumber;
    }

    public void MarkPaid()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException($"Cannot mark order as paid from {Status} state.");
        Status = OrderStatus.Paid;
    }

    public void MarkComplete()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException($"Cannot mark order as complete from {Status} state.");
        Status = OrderStatus.Complete;
    }
}
