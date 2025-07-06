using ControleDeBar.Dominio.ModuloGarcom;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloGarcom;

public class RepositorioGarcomEmSql : IRepositorioGarcom
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

    public void CadastrarRegistro(Garcom registro)
    {
        var sqlInserir =
            @"INSERT INTO [TBGARCOM]
                    (
                        [ID],
                        [NOME],
                        [CPF]
                    )
                    VALUES
                    (
                        @ID,
                        @NOME,
                        @CPF
                    );";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosGarcom(registro, comandoInsercao);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public bool EditarRegistro(Guid idRegistro, Garcom registroEditado)
    {
        var sqlEditar =
            @"UPDATE [TBGARCOM]
                    SET
                        [NOME] = @NOME,
                        [CPF] = @CPF
                    WHERE
                        [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosGarcom(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        int registrosAfetados = comandoEdicao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return registrosAfetados > 0;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var sqlExcluir =
            @"DELETE FROM [TBGARCOM]
                    WHERE
                        [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        int registrosAfetados = comandoExclusao.ExecuteNonQuery();

        conexaoComBanco.Close();

        return registrosAfetados > 0;
    }

    public Garcom? SelecionarRegistroPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId = @"SELECT * FROM [TBGARCOM] WHERE [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Garcom? garcom = null;

        if (leitor.Read())
            garcom = ConverterParaGarcom(leitor);

        conexaoComBanco.Close();

        return garcom;
    }

    public List<Garcom> SelecionarRegistros()
    {
        var sqlSelecionarTodos = @"SELECT * FROM [TBGARCOM]";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Garcom> garcons = new List<Garcom>();

        while (leitor.Read())
        {
            Garcom garcom = ConverterParaGarcom(leitor);

            garcons.Add(garcom);
        }

        conexaoComBanco.Close();

        return garcons;
    }

    private Garcom ConverterParaGarcom(SqlDataReader leitor)
    {
        var garcom = new Garcom
        {
            Nome = Convert.ToString(leitor["Nome"])!,
            Cpf = Convert.ToString(leitor["Cpf"])!
        };

        garcom.Id = Guid.Parse(leitor["Id"].ToString()!);

        return garcom;
    }

    private void ConfigurarParametrosGarcom(Garcom registro, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", registro.Id);
        comando.Parameters.AddWithValue("NOME", registro.Nome);
        comando.Parameters.AddWithValue("CPF", registro.Cpf);
    }
}
