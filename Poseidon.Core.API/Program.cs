using Microsoft.OData.Edm;
using Poseidon.Common.OData;
using Poseidon.Core.Entities;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Call Core API to get Entity Types desse servico.
// Criar domain entities dinamicamente
// Controllers dinamicos

// Add services to the container.
IEdmModel edmModel = EdmModelBuilder.GetEdmModel(typeof(EntityType).Assembly);
builder.Services.AddControllers().AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5)
                    .AddRouteComponents(edmModel));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseODataRouteDebug();
app.UseHttpsRedirection();


app.UseRouting();
// app.UseRouter(router => {
//     router.MapGet("/api/{entityType:string}", async (request, response, data) => {

//     });
// });

app.UseAuthorization();
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

app.Run();
