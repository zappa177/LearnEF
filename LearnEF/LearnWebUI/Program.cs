var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//add context 
//builder.Services.AddDbContext<MyDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();




app.Run();
