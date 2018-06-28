using System.Threading.Tasks;

namespace microCommerce.Consul
{
    public interface IConsulContext
    {
        T Get<T>(string key);
        void Put(string key, object obj);
        Task<T> GetAsync<T>(string key);
        Task PutAsync(string key, object obj);
    }
}