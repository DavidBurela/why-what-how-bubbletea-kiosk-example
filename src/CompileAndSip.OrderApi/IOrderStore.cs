namespace CompileAndSip.OrderApi;

public interface IOrderStore
{
    void Add(Order order);
    Order? GetById(Guid id);
    IReadOnlyList<Order> GetActiveOrders();
    int NextOrderNumber();
}
