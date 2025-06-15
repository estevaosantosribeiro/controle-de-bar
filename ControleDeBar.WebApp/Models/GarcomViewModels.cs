using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.WebApp.Extensions;

namespace ControleDeBar.WebApp.Models;

public class FormularioGarcomViewModel
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
}

public class CadastrarGarcomViewModel : FormularioGarcomViewModel
{
    public CadastrarGarcomViewModel() { }

    public CadastrarGarcomViewModel(string nome, string cpf) : this()
    {
        Nome = nome;
        Cpf = nome;
    }
}

public class EditarGarcomViewModel : FormularioGarcomViewModel
{
    public Guid Id { get; set; }

    public EditarGarcomViewModel() { }

    public EditarGarcomViewModel(Guid id, string nome, string cpf) : this()
    {
        Id = id;
        Nome = nome;
        Cpf = cpf;
    }
}

public class ExcluirGarcomViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public ExcluirGarcomViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarGarconsViewModel
{
    public List<DetalhesGarcomViewModel> Registros { get; set; }

    public VisualizarGarconsViewModel(List<Garcom> garcons)
    {
        Registros = new List<DetalhesGarcomViewModel>();

        foreach (var g in garcons)
            Registros.Add(g.ParaDetalhesVM());
    }
}

public class DetalhesGarcomViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }

    public DetalhesGarcomViewModel(Guid id, string nome, string cpf)
    {
        Id = id;
        Nome = nome;
        Cpf = cpf;
    }
}

public class SelecionarGarcomViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public SelecionarGarcomViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}