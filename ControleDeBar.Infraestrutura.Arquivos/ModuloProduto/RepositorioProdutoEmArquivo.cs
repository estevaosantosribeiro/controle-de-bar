using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;

public class RepositorioProdutoEmArquivo : RepositorioBaseEmArquivo<Produto>, IRepositorioProduto
{
    public RepositorioProdutoEmArquivo(ContextoDados contexto) : base(contexto)
    {
    }

    protected override List<Produto> ObterRegistros()
    {
        return contexto.Produtos;
    }
}
