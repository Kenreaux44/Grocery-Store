using GroceryStoreData.Contracts.Interfaces;
using GroceryStoreData.Data;
using GroceryStoreData.Repositories;
using Microsoft.EntityFrameworkCore;
using MyfirstLib.Contracts.Interfaces;
using MyfirstLib.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(ProductService));
builder.Services.AddAutoMapper(typeof(ShoppingListItemService));
builder.Services.AddAutoMapper(typeof(ShoppingListService));
builder.Services.AddAutoMapper(typeof(StateService));
builder.Services.AddAutoMapper(typeof(StoreProductService));
builder.Services.AddAutoMapper(typeof(StoreService));
builder.Services.AddAutoMapper(typeof(UserService));

AddDataContexts(builder.Services);
AddRepositories(builder.Services);
AddRepositoryServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

void AddDataContexts(IServiceCollection service)
{
    service.AddScoped<GroceryStore_DataContext>();

    var connectionString = builder.Configuration.GetConnectionString("GroceryStoreDefaultConnection");

    builder.Services.AddDbContext<GroceryStore_DataContext>(options =>
        options.UseSqlServer(connectionString));
}

void AddRepositories(IServiceCollection service)
{
    service.AddScoped<IProductRepository, ProductRepository>();
    service.AddScoped<IShoppingListItemRepository, ShoppingListItemRepository>();
    service.AddScoped<IShoppingListRepository, ShoppingListRepository>();
    service.AddScoped<IStateRepository, StateRepository>();
    service.AddScoped<IStoreProductRepository, StoreProductRepository>();
    service.AddScoped<IStoreRepository, StoreRepository>();
    service.AddScoped<IUserRepository, UserRepository>();
}

void AddRepositoryServices(IServiceCollection service)
{
    service.AddScoped<IProductService, ProductService>();
    service.AddScoped<IShoppingListItemService, ShoppingListItemService>();
    service.AddScoped<IShoppingListService, ShoppingListService>();
    service.AddScoped<IStateService, StateService>();
    service.AddScoped<IStoreProductService, StoreProductService>();
    service.AddScoped<IStoreService, StoreService>();
    service.AddScoped<IUserService, UserService>();
}