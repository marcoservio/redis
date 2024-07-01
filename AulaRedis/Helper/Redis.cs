using Microsoft.Extensions.Options;

using ServiceStack.Redis;

namespace AulaRedis.Helper
{
    public interface IRedisHelper
    {
        T Existe<T>(string key);
        void Gravar<T>(string key, dynamic valor, DateTime dtExpirar);
        void Gravar<T>(string key, dynamic valor);
        public bool Remover(string key);
    }

    public class RedisHelper : IRedisHelper
    {
        RedisEndpoint redisEndpoint;

        public RedisHelper()
        {
            int porta = 6378;
            string host = "localhost";
            string senha = "Senha@123";

            redisEndpoint = new RedisEndpoint(host, porta, password: senha);
        }

        public T Existe<T>(string key)
        {
            using (var client = new RedisClient(redisEndpoint))
            {
                return client.Get<T>(key);
            }
        }

        public void Gravar<T>(string key, dynamic valor)
        {
            using (var client = new RedisClient(redisEndpoint))
            {
                client.Set<T>(key, valor);
            }
        }

        public void Gravar<T>(string key, dynamic valor, DateTime dtExpirar)
        {
            using (var client = new RedisClient(redisEndpoint))
            {
                client.Set<T>(key, valor, dtExpirar);
            }
        }

        public bool Remover(string key)
        {
            using (var client = new RedisClient(redisEndpoint))
            {
                return client.Remove(key);
            }
        }
    }
}
