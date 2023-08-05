using ChatAppBackEnd.Data;
using ChatAppBackEnd.Helper.AutoMapper;
using ChatAppBackEnd.Hubs.Service;
using ChatAppBackEnd.Service.ChatRoomService;
using ChatAppBackEnd.Service.HubService;
using ChatAppBackEnd.Service.MessageService;
using ChatAppBackEnd.Service.UserChatRoomService;
using ChatAppBackEnd.Service.UserRelationshipService;
using ChatAppBackEnd.Service.UserService;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEnd.Startup
{
    public static class DependencyInjectionSetup
    {
        public static string AllowCorsOrigin = "_myAllowSpecificOrigins";
        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");


            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            var connectionStringFromEnvironmentVariable = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnections");
            //.UseLazyLoadingProxies()
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserChatRoomService, UserChatRoomService>();
            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddSingleton<IUserConnectionIdService, UserConnectionIdService>();
            services.AddScoped<IHubService, HubService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IUserRelationshipService, UserRelationshipService>();
            services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowCorsOrigin,
                                  builder =>
                                  {
                                      builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                                  });
            });

            return services;
        }
    }
}
