using AulaRedis.Dal;
using AulaRedis.Helper;
using AulaRedis.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ServiceStack.Redis;

namespace AulaRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        IProdutos produtos;
        IRedisHelper redis;

        public ProdutosController(IProdutos _produtos, IRedisHelper _redis)
        {
            produtos = _produtos;
            redis = _redis;
        }

        [HttpPost]
        public IActionResult Gravar()
        {
            produtos.Gravar();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            //Verifica se existe no redis
            //preenche a variavel para retornar para o cliente
            IEnumerable<PRODUTOS> listaProdutos = redis.Existe<IEnumerable<PRODUTOS>>("lista-produtos");

            //está no redis???
            if (listaProdutos == null)
            {
                //buscar na base de dados
                listaProdutos = await produtos.ListarProdutos();

                Console.WriteLine($"Carregamos da base {DateTime.Now}");

                //guardar no redis
                redis.Gravar<IEnumerable<PRODUTOS>>("lista-produtos", listaProdutos, DateTime.Now.AddMinutes(1));
            }

            //retornar para o cliente
            return Ok(listaProdutos);
        }

        public class Produto
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public decimal Preco { get; set; }
        }

    }
}
