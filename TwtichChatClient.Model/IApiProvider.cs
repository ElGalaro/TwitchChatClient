using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public interface IApiProvider
    {
        T GetApi<T>(params string[] arguments) where T: class;
        Task<T> GetApiAsync<T>(params string[] arguments) where T : class;
    }
}
