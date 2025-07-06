using ControleDeBar.Dominio.Compartilhado;
using ControleDeBar.Dominio.ModuloMesa;
using Microsoft.Data.SqlClient;
using System.Xml;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloMesa;

public class RepositorioMesaEmSql : IRepositorioMesa
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

    public void CadastrarRegistro(Mesa registro)
    {
        var sqlInserir =
            @"INSERT INTO [TBMESA]
                    (
                        [ID],
                        [NUMERO],
                        [CAPACIDADE],
                        [ESTAOCUPADA]
                    )
                    VALUES
                    (
                        @ID,
                        @NUMERO,
                        @CAPACIDADE,
                        @ESTAOCUPADA
                    );";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosMesa(registro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Mesa registroEditado)
    {
        var sqlEditar =
            @"UPDATE [TBMESA]
                    SET
                        [NUMERO] = @NUMERO,
                        [CAPACIDADE] = @CAPACIDADE,
                        [ESTAOCUPADA] = @ESTAOCUPADA
                    WHERE
                        [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosMesa(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var sqlExcluir = @"DELETE FROM [TBMESA] WHERE [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return linhasAfetadas > 0;
    }

    public Mesa? SelecionarRegistroPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId = @"SELECT * FROM [TBMESA] WHERE [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Mesa? mesa = null;

        if (leitor.Read())
            mesa = ConverterParaMesa(leitor);

        conexaoComBanco.Close();

        return mesa;
    }

    public List<Mesa> SelecionarRegistros()
    {
        var sqlSelecionarTodos = "SELECT * FROM [TBMESA]";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        var mesas = new List<Mesa>();

        while (leitor.Read())
        {
            var mesa = ConverterParaMesa(leitor);
            
            mesas.Add(mesa);
        }

        conexaoComBanco.Close();

        return mesas;
    }

    private Mesa ConverterParaMesa(SqlDataReader leitor)
    {
        var mesa = new Mesa(
            Convert.ToInt32(leitor["Numero"]),
            Convert.ToInt32(leitor["Capacidade"])
        );

        mesa.Id = Guid.Parse(leitor["Id"].ToString()!);

        return mesa;
    }

    private void ConfigurarParametrosMesa(Mesa mesa, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("Id", mesa.Id);
        comando.Parameters.AddWithValue("Numero", mesa.Numero);
        comando.Parameters.AddWithValue("Capacidade", mesa.Capacidade);
        comando.Parameters.AddWithValue("EstaOcupada", mesa.EstaOcupada);
    }
}
