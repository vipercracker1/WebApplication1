public class Startup
{
    public IConfiguration configRoot
    {
        get;
    }
    public Startup(IConfiguration configuration)
    {
        configRoot = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddRazorPages();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin() // Cho phép tất cả nguồn gốc
                       .AllowAnyMethod() // Cho phép tất cả các phương thức HTTP
                       .AllowAnyHeader(); // Cho phép tất cả các header
            });
        });
    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "api",
                pattern: "api/{controller}/{action}/{id?}",
                defaults: new { controller = "API", action = "Index" });
        });
        app.Run();
    }
}