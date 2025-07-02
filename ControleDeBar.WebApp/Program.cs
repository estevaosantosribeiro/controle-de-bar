using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloConta;
using ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;
using ControleDeBar.Infraestrutura.Arquivos.ModuloMesa;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;

namespace ControleDeBar.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ContextoDados>((_) => new ContextoDados(true));
            builder.Services.AddScoped<IRepositorioMesa, RepositorioMesaEmArquivo>();
            builder.Services.AddScoped<IRepositorioProduto, RepositorioProdutoEmArquivo>();
            builder.Services.AddScoped<IRepositorioGarcom, RepositorioGarcomEmArquivo>();
            builder.Services.AddScoped<IRepositorioConta, RepositorioContaEmArquivo>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAntiforgery();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();
            app.UseAuthorization();

            app.Run();
        }
    }
}
