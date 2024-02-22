using System.Threading.Tasks;

namespace SaloonReservation.Services
{
    public interface IRazorPartialToStringRenderer
    {
        Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
    }
}
