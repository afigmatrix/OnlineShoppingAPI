using System.Threading.Tasks;

namespace OnlineShoppingAPI.Service.Abstractions
{
    public interface IProductRepository
    {
        decimal CalculateProductPrice(int id);
        Task<string> GetProductFileBase64(int id);

    }
}
