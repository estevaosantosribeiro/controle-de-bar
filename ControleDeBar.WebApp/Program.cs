using ControleDeBar.Dominio.ModuloConta;
using ControleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrutura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloConta;
using ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;
using ControleDeBar.Infraestrutura.Arquivos.ModuloMesa;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;
using ControleDeBar.Infraestrutura.SqlServer.ModuloGarcom;
using ControleDeBar.Infraestrutura.SqlServer.ModuloMesa;
using ControleDeBar.Infraestrutura.SqlServer.ModuloProduto;
using ControleDeBar.WebApp.ActionFilters;
using ControleDeBar.WebApp.DependencyInjection;

namespace ControleDeBar.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ValidarModeloAttribute>();
                options.Filters.Add<LogarAcaoAttribute>();
            });

            builder.Services.AddScoped<ContextoDados>((_) => new ContextoDados(true));
            builder.Services.AddScoped<IRepositorioMesa, RepositorioMesaEmSql>();
            builder.Services.AddScoped<IRepositorioProduto, RepositorioProdutoEmSql>();
            builder.Services.AddScoped<IRepositorioGarcom, RepositorioGarcomEmSql>();
            builder.Services.AddScoped<IRepositorioConta, RepositorioContaEmArquivo>();

            builder.Services.AddSerilogConfig(builder.Logging);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
                app.UseExceptionHandler("/erro");
            else
                app.UseDeveloperExceptionPage();

            app.UseAntiforgery();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
