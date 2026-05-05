
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Behaviors;
using PMS.Application.Interfaces.Services;
using PMS.Infrastructre.Data;
using PMS.Infrastructre.Services.UnitOfWork;


namespace PMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            // builder.Services.AddTransient(typeof(IunitOfWork<,>), typeof(UnitOfWork<,>));
          //  builder.Services.AddTransient<IStudentServices, StudentServices>();
            //builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(Application.IAssemplyMarker).Assembly));
            //builder.Services.AddValidatorsFromAssembly(typeof(Application.IAssemplyMarker).Assembly);
           // builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
