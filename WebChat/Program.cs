using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Azure;
using WebChat.Hubs;
using WebChat.Identity;
using WebChat.Models;
using WebChat.Repositories;
using WebMessage.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
var table = builder.Configuration.GetSection("AppSettings")["TableName"];
var conect = builder.Configuration.GetConnectionString("AzureConnection");
builder.Services.AddSingleton(provider =>
{
	TableServiceClient tableServiceClient = new TableServiceClient(conect);
	var tabClient = tableServiceClient.GetTableClient(table);
	tabClient.CreateIfNotExists();
	return tabClient;
});
builder.Services.AddSingleton<TChatRepository>();
builder.Services.AddSingleton<TMessagesRepository>();
builder.Services.AddIdentity<TUser, TRole>(builder =>
{
	builder.SignIn.RequireConfirmedPhoneNumber = false;
	builder.SignIn.RequireConfirmedEmail = false;
	builder.SignIn.RequireConfirmedAccount = false;

	builder.Password.RequireNonAlphanumeric=false;
	builder.Password.RequireUppercase=false;
	builder.Password.RequireLowercase=false;
})
	.AddUserStore<UserStore>()
	.AddRoleStore<RoleStore>();

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Chats}/{action=MyChats}/{id?}");

app.Run();
