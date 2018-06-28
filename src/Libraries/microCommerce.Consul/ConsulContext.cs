using Consul;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace microCommerce.Consul
{
    public class ConsulContext : IConsulContext
    {
        private readonly Lazy<string> _connectionString;
        private IConsulClient _client;
        private readonly object _lock = new object();

        public ConsulContext(string connectionString)
        {
            _connectionString = new Lazy<string>(connectionString);
        }
        
        protected virtual IKVEndpoint GetDatabase()
        {
            if (_client != null) return _client.KV;

            lock (_lock)
            {
                if (_client != null) return _client.KV;

                _client = new ConsulClient(config =>
                {
                    config.Address = new Uri(_connectionString.Value);
                });
            }

            return _client.KV;
        }

        public virtual T Get<T>(string key)
        {
            var response = GetDatabase().Get(key).GetAwaiter().GetResult().Response;
            if (response == null)
                return default(T);

            if (response.Value.Length == 0)
                return default(T);

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(response.Value));
        }

        public virtual void Put(string key, object obj)
        {
            var item = new KVPair(key);
            item.Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item));
            GetDatabase().Put(item).GetAwaiter();
        }

        public virtual async Task<T> GetAsync<T>(string key)
        {
            var queryResult = await GetDatabase().Get(key);
            if (queryResult == null || queryResult.Response == null)
                return default(T);

            var response = queryResult.Response;
            if (response.Value.Length == 0)
                return default(T);

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(response.Value));
        }

        public virtual async Task PutAsync(string key, object obj)
        {
            var item = new KVPair(key);
            item.Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item));
            await GetDatabase().Put(item);
        }
    }
}