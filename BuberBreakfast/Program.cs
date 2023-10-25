using BuberBreakfas.Services.Breakfasts;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    // scoped: 每个Http请求内创建同一个instance
    // 当使用IBreakfastService时，using BreakfastService
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
}


var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}


