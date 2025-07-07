using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloConta;

public class RepositorioContaEmSql : IRepositorioConta
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

    public void CadastrarConta(Conta conta)
    {
        var sqlInserirConta =
            @"INSERT INTO [TBConta] 
                (ID, TITULAR, MESA_ID, GARCOM_ID, ABERTURA, FECHAMENTO, ESTAABERTA) 
            VALUES 
                (@Id, @Titular, @MesaId, @GarcomId, @Abertura, @Fechamento, @EstaAberta);";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoInsercao = new SqlCommand(sqlInserirConta, conexaoComBanco);

        ConfigurarParametrosConta(comandoInsercao, conta);

        conexaoComBanco.Open();

        comandoInsercao.ExecuteNonQuery();

        conexaoComBanco.Close();
    }

    public List<Conta> SelecionarContas()
    {
        var sqlSelecionarContas =
           @"SELECT 
                CT.[ID], 
                CT.[TITULAR], 
                CT.[ABERTURA], 
                CT.[FECHAMENTO], 
                CT.[ESTAABERTA],
                CT.[MESA_ID],
                CT.[GARCOM_ID],
                MS.[ID] AS MESA_ID_INTERNO,
                MS.[NUMERO],
                MS.[CAPACIDADE],
                MS.[ESTAOCUPADA],
                GR.[ID] AS GARCOM_ID_INTERNO,
                GR.[NOME],
                GR.[CPF]
            FROM 
                [TBConta] AS CT
            LEFT JOIN 
                [TBMesa] AS MS ON CT.[MESA_ID] = MS.[ID]
            LEFT JOIN 
                [TBGarcom] AS GR ON CT.[GARCOM_ID] = GR.[ID];";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarContas, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contas = new List<Conta>();

        while (leitor.Read())
        {
            Conta conta = ConverterParaConta(leitor);

            contas.Add(conta);
        }

        conexaoComBanco.Close();

        return contas;
    }

    public List<Conta> SelecionarContasAbertas()
    {
        var sqlSelecionarContasAbertas =
            @"SELECT 
                CT.[ID], 
                CT.[TITULAR], 
                CT.[ABERTURA], 
                CT.[FECHAMENTO], 
                CT.[ESTAABERTA],
                CT.[MESA_ID],
                CT.[GARCOM_ID],
                MS.[ID] AS MESA_ID_INTERNO,
                MS.[NUMERO],
                MS.[CAPACIDADE],
                MS.[ESTAOCUPADA],
                GR.[ID] AS GARCOM_ID_INTERNO,
                GR.[NOME],
                GR.[CPF]
            FROM 
                [TBConta] AS CT
            LEFT JOIN 
                [TBMesa] AS MS ON CT.[MESA_ID] = MS.[ID]
            LEFT JOIN 
                [TBGarcom] AS GR ON CT.[GARCOM_ID] = GR.[ID]
            WHERE 
                CT.ESTAABERTA = 1;";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarContasAbertas, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contasAbertas = new List<Conta>();

        while (leitor.Read())
        {
            Conta conta = ConverterParaConta(leitor);

            contasAbertas.Add(conta);
        }

        conexaoComBanco.Close();

        return contasAbertas;
    }

    public List<Conta> SelecionarContasFechadas()
    {
        var sqlSelecionarContasFechadas =
            @"SELECT 
                CT.[ID], 
                CT.[TITULAR], 
                CT.[ABERTURA], 
                CT.[FECHAMENTO], 
                CT.[ESTAABERTA],
                CT.[MESA_ID],
                CT.[GARCOM_ID],
                MS.[ID] AS MESA_ID_INTERNO,
                MS.[NUMERO],
                MS.[CAPACIDADE],
                MS.[ESTAOCUPADA],
                GR.[ID] AS GARCOM_ID_INTERNO,
                GR.[NOME],
                GR.[CPF]
            FROM 
                [TBConta] AS CT
            LEFT JOIN 
                [TBMesa] AS MS ON CT.[MESA_ID] = MS.[ID]
            LEFT JOIN 
                [TBGarcom] AS GR ON CT.[GARCOM_ID] = GR.[ID]
            WHERE 
                CT.ESTAABERTA = 0;";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarContasFechadas, conexaoComBanco);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contasFechadas = new List<Conta>();

        while (leitor.Read())
        {
            Conta conta = ConverterParaConta(leitor);

            contasFechadas.Add(conta);
        }

        conexaoComBanco.Close();

        return contasFechadas;
    }

    public List<Conta> SelecionarContasPorPeriodo(DateTime data)
    {
        var sqlSelecionarContasPorPeriodo =
            @"SELECT 
                CT.[ID], 
                CT.[TITULAR], 
                CT.[ABERTURA], 
                CT.[FECHAMENTO], 
                CT.[ESTAABERTA],
                CT.[MESA_ID],
                CT.[GARCOM_ID],
                MS.[ID] AS MESA_ID_INTERNO,
                MS.[NUMERO],
                MS.[CAPACIDADE],
                MS.[ESTAOCUPADA],
                GR.[ID] AS GARCOM_ID_INTERNO,
                GR.[NOME],
                GR.[CPF]
            FROM 
                [TBConta] AS CT
            LEFT JOIN 
                [TBMesa] AS MS ON CT.[MESA_ID] = MS.[ID]
            LEFT JOIN 
                [TBGarcom] AS GR ON CT.[GARCOM_ID] = GR.[ID]
            WHERE 
                CAST(CT.ABERTURA AS DATE) = @Data;";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarContasPorPeriodo, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("Data", data.Date);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        List<Conta> contasPorPeriodo = new List<Conta>();

        while (leitor.Read())
        {
            Conta conta = ConverterParaConta(leitor);

            contasPorPeriodo.Add(conta);
        }

        conexaoComBanco.Close();

        return contasPorPeriodo;
    }

    public Conta? SelecionarPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId =
            @"SELECT 
                CT.[ID], 
                CT.[TITULAR], 
                CT.[ABERTURA], 
                CT.[FECHAMENTO], 
                CT.[ESTAABERTA],
                CT.[MESA_ID],
                CT.[GARCOM_ID],
                MS.[ID] AS MESA_ID_INTERNO,
                MS.[NUMERO],
                MS.[CAPACIDADE],
                MS.[ESTAOCUPADA],
                GR.[ID] AS GARCOM_ID_INTERNO,
                GR.[NOME],
                GR.[CPF]
            FROM 
                [TBConta] AS CT
            LEFT JOIN 
                [TBMesa] AS MS ON CT.[MESA_ID] = MS.[ID]
            LEFT JOIN 
                [TBGarcom] AS GR ON CT.[GARCOM_ID] = GR.[ID]
            WHERE 
                CT.ID = @ID;";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Conta? conta = null;

        if (leitor.Read())
        {
            conta = ConverterParaConta(leitor);
        }

        conexaoComBanco.Close();

        return conta;
    }

    private Conta ConverterParaConta(SqlDataReader leitor)
    {
        DateTime? fechamento = null;
        Mesa? mesa = null;
        Garcom? garcom = null;

        if (!leitor["Fechamento"].Equals(DBNull.Value))
            fechamento = Convert.ToDateTime(leitor["Fechamento"]);

        if (!leitor["Mesa_Id"].Equals(DBNull.Value))
            mesa = ConverterParaMesa(leitor);

        if (!leitor["Garcom_Id"].Equals(DBNull.Value))
            garcom = ConverterParaGarcom(leitor);

        var conta = new Conta
        {
            Id = Guid.Parse(leitor["ID"].ToString()!),
            Titular = leitor["Titular"].ToString()!,
            Mesa = mesa!,
            Garcom = garcom!,
            Abertura = Convert.ToDateTime(leitor["Abertura"]),
            Fechamento = fechamento,
            EstaAberta = Convert.ToBoolean(leitor["EstaAberta"])
        };

        return conta;
    }

    private Mesa ConverterParaMesa(SqlDataReader leitor)
    {
        var mesa = new Mesa(
            Convert.ToInt32(leitor["Numero"]),
            Convert.ToInt32(leitor["Capacidade"])
        );

        mesa.Id = Guid.Parse(leitor["Mesa_Id_Interno"].ToString()!);

        return mesa;
    }

    private Garcom ConverterParaGarcom(SqlDataReader leitor)
    {
        var garcom = new Garcom
        {
            Nome = Convert.ToString(leitor["Nome"])!,
            Cpf = Convert.ToString(leitor["Cpf"])!
        };

        garcom.Id = Guid.Parse(leitor["Garcom_Id_Interno"].ToString()!);

        return garcom;
    }

    private void ConfigurarParametrosConta(SqlCommand comando, Conta conta)
    {
        comando.Parameters.AddWithValue("Id", conta.Id);
        comando.Parameters.AddWithValue("Titular", conta.Titular);
        comando.Parameters.AddWithValue("MesaId", conta.Mesa.Id);
        comando.Parameters.AddWithValue("GarcomId", conta.Garcom.Id);
        comando.Parameters.AddWithValue("Abertura", conta.Abertura);
        comando.Parameters.AddWithValue("Fechamento", conta.Fechamento ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("EstaAberta", conta.EstaAberta);
    }
}
