using System.Security.Claims;
using System.Text;
using ITI.Poll.AspNetCore;
using ITI.Poll.GraphQL.Services;
using ITI.Poll.GraphQL.Types;
using ITI.Poll.Infrastructure;
using ITI.Poll.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ITI.Poll.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddQueryType<PollQueryType>()
                .AddMutationType<PollMutationType>()
                .AddAuthorization();

            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.IsAuthenticated, policy =>
                {
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });

            services.AddDbContext<PollContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:Poll"]);
            });
            services.AddScoped<IPollContextAccessor, PollContextAccessor>();

            services.AddSingleton<IUserDeletedEventHandler, UserDeletedEventHandler>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IPollService, PollService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPollRepository, PollRepository>();

            string secretKey = Configuration["Jwt:SigningKey"];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.AddAuthentication("Jwt")
                .AddJwtBearer("Jwt", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        AuthenticationType = "Jwt",
                        ValidateLifetime = false,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            services.Configure<TokenServiceOptions>(options =>
            {
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(cors =>
            {
                cors.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }

    public static class Policy
    {
        public static readonly string IsAuthenticated = "IsAuthenticated";
    }
}
