using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGet(
        [FromServices] IWebHostEnvironment env,
        [FromServices] IConfiguration configuration)
    {
        if (!env.IsProduction()) return Page();

        return Redirect(configuration["HomePageRedirectDestination"] ??
            throw new ArgumentException("The HomePageRedirectDestination setting is missing"));
    }
}
