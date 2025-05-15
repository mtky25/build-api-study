using ApiProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
public class Startup
{
    public IConfiguration Configuration { get; }
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApiProjectDbContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        //app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
