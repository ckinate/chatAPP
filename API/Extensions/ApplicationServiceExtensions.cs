using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder,IConfiguration config )
        {
            builder.Services.AddDbContext<DataContext>(options=>{
          options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
           });
         builder.Services.AddScoped<ITokenService, TokenService>();
            
         return builder;
        }

        
    }
}