using LibraryServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Library.Services;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Identity;

namespace Library
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddSingleton(Configuration);
      services.AddScoped<ILibraryAsset, LibraryAssetService>();
      services.AddScoped<ICheckout, CheckoutService>();
      services.AddScoped<IPatron, PatronService>();

 

      services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<LibraryContext>()
        .AddDefaultTokenProviders();
      // Add application services.
      services.AddTransient<IEmailSender, EmailSender>();

      //services.AddDbContext<UserManagementDbContext>(options =>
      //  options.UseSqlServer(Configuration.GetConnectionString("LibraryConnection")));
      services.AddDbContext<LibraryContext>(options
        => options.UseSqlServer(Configuration.GetConnectionString("LibraryConnection")));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
