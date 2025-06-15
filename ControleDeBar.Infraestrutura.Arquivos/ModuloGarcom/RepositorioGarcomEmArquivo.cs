using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;

public class RepositorioGarcomEmArquivo : RepositorioBaseEmArquivo<Garcom>, IRepositorioGarcom
{
    public RepositorioGarcomEmArquivo(ContextoDados contexto) : base(contexto) { }

    protected override List<Garcom> ObterRegistros()
    {
        return contexto.Garcons;
    }
}
