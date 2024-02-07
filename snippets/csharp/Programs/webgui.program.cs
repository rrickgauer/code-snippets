bool isProduction = true;

#if DEBUG
isProduction = false;
#endif

//isProduction = false;


#region - Setup web application builder -

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();



builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    //options.KeepAliveInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
    options.Filters.Add<ValidationErrorFilter>();
    options.SuppressAsyncSuffixInActionNames = false;
})

// https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0#disable-automatic-400-response
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
})

.AddJsonOptions(options =>
{
    if (!isProduction)
    {
        options.JsonSerializerOptions.WriteIndented = true;
    }

    //options.JsonSerializerOptions.Converters.Add(new UssConverter());
    
});


IConfigs config = isProduction ? new ConfigurationProduction() : new ConfigurationDev();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(IPAddress.Parse(config.VpsIpAddress));
});




// session management
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = GuiSessionKeys.SessionName;
    options.Cookie.IsEssential = true;
});

#endregion


#region - Dependency Injection -

InjectionProject projectTypes = InjectionProject.Always | InjectionProject.WebGui;

DependencyInjectionUtilities.InjectAll(builder.Services, isProduction, projectTypes, Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#endregion



#region - Build application -

var app = builder.Build();

app.UseWebSockets();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();


//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(config.StaticFilesPath),
    ServeUnknownFileTypes = true,
});

app.MapHub<LoadProjectEditorHub>("/load-project-editor-hub", options =>
{

});


app.MapHub<JobRunHub>("/hubs/job-run", options =>
{

});


app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.UseSession();
app.UseForwardedHeaders();
app.Run();

#endregion