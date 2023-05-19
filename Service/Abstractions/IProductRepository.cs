namespace OnlineShoppingAPI.Service.Abstractions
{
    public interface IProductRepository
    {
        decimal CalculateProductPrice(int id);

    }
}
