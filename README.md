# UsePathBase
UsePathBase for aspnetcore 1.0 and UsePathEnvironment()

Thanks to Microsoft and community for original code.

This allows you to define sites pathbase in an environment variable and make it optional.

```Install-Package ebyte23.UsePathBase```

You must put this at the top of your apps configure like so.

```
 public void (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            //This should be your first middleware
            app.UsePathBaseEnvironment(true);
            
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
```
