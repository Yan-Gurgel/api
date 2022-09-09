using Microsoft.AspNetCore.Mvc;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.WithOrigins("*"
                                              )
                                              .WithMethods("PUT", "DELETE", "GET");
                      });
});


var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/", () => "API de Cadastro de Clientes e Endereços.");

app.MapGet("/Clients", (ApplicationDbContext context) =>
{
    var clients = context.Client.ToArray();
    if (clients != null)
    {
        return Results.Ok(clients);
    }
    return Results.NotFound("Client not found");
});

/*==============================Endereço=========================================*/

//salvar endereço
app.MapPost("/Address", (AddressResquest addressResquest, ApplicationDbContext context) =>
{
    var address = new Address
    {
        city = addressResquest.city,
        state = addressResquest.state,
    };
    context.Address.Add(address);
    context.SaveChanges();
    return Results.Created($"/Address/{address.id}", address.id + " City: " + address.city + " and State: " + address.state + " saved sucessfully!");
});

//visualizar Cidade
app.MapGet("/Address/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var address = context.Address.Where(a => a.id == id).First();
    if (address != null)
    {
        return Results.Ok(address);
    }
    return Results.NotFound();
});

/*==============================CLIENTE=========================================*/
// salvar cliente
app.MapPost("/Client", (ClientResquest clientResquest, ApplicationDbContext context) =>
{
    var client = new Client
    {
        name = clientResquest.name,
        gender = clientResquest.gender,
        birthDate = clientResquest.birthDate,
        age = clientResquest.age,
        city = clientResquest.city,
    };

    context.Client.Add(client);
    context.SaveChanges();
    return Results.Created($"/Client/{client.id}", client.id + " " + client.name + " client" + " saved successfully!");
});

//visualizar Cliente
app.MapGet("/Client/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var client = context.Client.Where(c => c.id == id).First();
    if (client != null)
    {
        return Results.Ok(client);
    }
    return Results.NotFound("Client not found");
});

//editar campos do Cliente passando o seu id
app.MapPut("/Client/{id}", ([FromRoute] int id, ClientResquest clientResquest, ApplicationDbContext context) =>
{
    var client = context.Client.Where(c => c.id == id).First();

    client.name = clientResquest.name;
    client.gender = clientResquest.gender;
    client.birthDate = clientResquest.birthDate;
    client.age = clientResquest.age;
    client.city = clientResquest.city;

    /* if (clientResquest.name != null)
    {
        client.name = clientResquest.name;
    }

    if (clientResquest.gender != null)
    {
        client.gender = clientResquest.gender;
    }

    if (clientResquest.birthDate != null)
    {
        client.birthDate = clientResquest.birthDate;
    }

    if (clientResquest.age != 0)
    {
        client.age = clientResquest.age;
    }

    if (clientResquest.city != null)
    {
        client.city = clientResquest.city;
    } */
    context.SaveChanges();
    return Results.Ok(client);
});


//Remover Cliente passando o seu id
app.MapDelete("/Client/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var client = context.Client.Where(c => c.id == id).First();
    context.Client.Remove(client);
    context.SaveChanges();
    Console.WriteLine("Client " + client.name + " removed successfully!");
    return Results.Ok();
});

app.Run();
