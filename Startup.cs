using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditLiveFeed.Hubs;
using RedditLiveFeed.Services;
using RedditLiveFeed.Services.Interfaces;
using RedditLiveFeed.Utils;
using RedditLiveFeed.Utils.Interfaces;

namespace RedditLiveFeed
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddMemoryCache();
            services.AddMvc();
            services.AddSignalR();

            services.Configure<RedditAuthConfiguration>(Configuration.GetSection("RedditAuthConfiguration"));
            services.AddHttpClient<IRedditClient, RedditClient>();

            services.AddSingleton<IRedditClientFactory, RedditClientFactory>();

            services.AddTransient<IRedditApiService, RedditApiService>();
            services.AddTransient<INotifyService, NotifyService>();

            services.AddSingleton<IConnectionStateService, ConnectionStateService>();
            services.AddSingleton<IRedditFeedService, RedditFeedService>();
            services.AddSingleton<IHostedService, RedditHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/hub"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
                });
            });

            app.UseSignalR(options =>
            {
                options.MapHub<RedditFeedHub>("/hub");
            });
        }
    }
}
