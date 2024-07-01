using AulaRedis.Models;

using Dapper;

using System.Data.SqlClient;

namespace AulaRedis.Dal
{
    public interface IProdutos
    {
        public void Gravar();
        public Task<IEnumerable<Models.PRODUTOS>> ListarProdutos();
    }

    public class Produtos : IProdutos
    {
        string StringConexao = "server=localhost;password=Senha@123;user=sa;initial catalog=produtos";
        public void Gravar()
        {
            string query = @"insert into tbl_produtos (id, nome, ativo) values (@id, @nome, @ativo)";
            var comm = new SqlConnection(StringConexao);
            DynamicParameters parametros;
            for (int i = 1; i <= 999999; i++)
            {
                parametros = new DynamicParameters();
                parametros.Add("id", Guid.NewGuid());
                parametros.Add("nome", $"Produto {i}");
                parametros.Add("ativo", i%2==0?1:0);

                comm.Execute(query, param: parametros, commandType: System.Data.CommandType.Text);
            }
        }

        public async Task<IEnumerable<PRODUTOS>> ListarProdutos()
        {
            string query = @"SELECT id, nome from tbl_produtos WHERE ativo = 1";
            IEnumerable<PRODUTOS> produtos;
            using (var comm = new SqlConnection(StringConexao))
            {
                produtos = await comm.QueryAsync<PRODUTOS>(query, commandType: System.Data.CommandType.Text);
            }
            return produtos;
        }
    }
}
