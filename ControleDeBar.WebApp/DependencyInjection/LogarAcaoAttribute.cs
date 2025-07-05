using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ControleDeBar.WebApp.DependencyInjection;

public class LogarAcaoAttribute : ActionFilterAttribute
{
    private readonly ILogger<LogarAcaoAttribute> logger;

    public LogarAcaoAttribute(ILogger<LogarAcaoAttribute> logger)
    {
        this.logger = logger;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var result = context.Result;
        
        if (result is ViewResult viewResult && viewResult.Model is not null)
        {
            logger.LogInformation("Ação de endpoint executada com sucesso! {@Modelo}", viewResult.Model);
        }

        base.OnActionExecuted(context);
    }
}
