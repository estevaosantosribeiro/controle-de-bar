using ControleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloProduto;

public class RepositorioProdutoEmSql : IRepositorioProduto
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

    public void CadastrarRegistro(Produto registro)
    {
        var sqlInserir =
            @"INSERT INTO [TBPRODUTO]
                    (
                        [ID],
                        [NOME],
                        [VALOR]
                    )
                    VALUES
                    (
                        @ID,
                        @NOME,
                        @VALOR
                    );";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosProduto(registro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Produto registroEditado)
    {
        var sqlEditar =
            @"UPDATE [TBPRODUTO]
                    SET
                        [NOME] = @NOME,
                        [VALOR] = @VALOR
                    WHERE
                        [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosProduto(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        int registrosAfetados = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return registrosAfetados > 0;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var sqlExcluir =
            @"DELETE FROM [TBPRODUTO]
                    WHERE [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        int registrosAfetados = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return registrosAfetados > 0;
    }

    public Produto? SelecionarRegistroPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId = @"SELECT * FROM [TBPRODUTO] WHERE [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Produto? produto = null;

        if (leitor.Read())
            produto = ConverterParaProduto(leitor);

        conexaoComBanco.Close();

        return produto;
    }

    public List<Produto> SelecionarRegistros()
    {
        var sqlSelecionarTodos = @"SELECT * FROM [TBPRODUTO]";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Produto> produtos = new List<Produto>();

        while (leitor.Read())
        {
            Produto produto = ConverterParaProduto(leitor);

            produtos.Add(produto);
        }

        return produtos;
    }

    private Produto ConverterParaProduto(SqlDataReader leitor)
    {
        var produto = new Produto
        {
            Nome = leitor["NOME"].ToString()!,
            Valor = decimal.Parse(leitor["VALOR"].ToString()!)
        };

        produto.Id = Guid.Parse(leitor["ID"].ToString()!);

        return produto;
    }

    private void ConfigurarParametrosProduto(Produto produto, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", produto.Id);
        comando.Parameters.AddWithValue("NOME", produto.Nome);
        comando.Parameters.AddWithValue("VALOR", produto.Valor);
    }
}
