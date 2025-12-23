using TodoList.Models;
using TodoList.Dtos;
using TodoList.Services;
using TodoList.Endpoints;
// using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Para que swagger descubra los endpoints
builder.Services.AddSwaggerGen(); // Para que swagger genere la documentacion

var todos = new List<Todo>();
// var nextTodoId = 1;

var users = new List<User>();
var nextUserId = 1;

builder.Services.AddSingleton(users);
builder.Services.AddSingleton(todos);
builder.Services.AddSingleton<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige automáticamente a HTTPS si alguien hace un request HTTP
app.UseHttpsRedirection();

// // ======================
// // DATOS
// // ======================
// var frutas = new List<Fruta>();
// var nextId = 1;

// // ======================
// // GET - Listar frutas
// // ======================
// app.MapGet("/frutas", () =>
// {
//     return frutas;
// });

// // ======================
// // POST - Crear fruta
// // ======================
// app.MapPost("/frutas", (string nombre) =>
// {
//     var fruta = new Fruta(nextId++, nombre);
//     frutas.Add(fruta);
//     return fruta;
// });

// // ======================
// // GET - Obtener fruta por id
// // ======================
// app.MapGet("/frutas/{id}", (int id) =>
// {
//     var fruta = frutas.Find(f => f.Id == id);
//     // Console.WriteLine(fruta);
//     if (fruta == null)
//     {
//         return Results.NotFound("No existe esa fruta");
//     }
//     return Results.Ok(fruta);
// });

// // ======================
// // PUT - Actualizar fruta
// // ======================
// app.MapPut("/frutas/{id}", (int id, string nombre) =>
// {
//     var fruta = frutas.Find(f => f.Id == id);

//     if (fruta == null)
//     {
//         return Results.NotFound("No se encontro la fruta");
//     }
    
//     var nuevaFruta = fruta with { Nombre = nombre };    

//     frutas.Remove(fruta);
//     frutas.Add(nuevaFruta);

//     return Results.Ok(nuevaFruta);    
// });

// // ======================
// // DELETE - Eliminar fruta
// // ======================
// app.MapDelete("/frutas/{id}", (int id) =>
// {
//     var fruta = frutas.Find(f => f.Id == id);
    
//     if (fruta == null)
//     {
//         return Results.NotFound("No se encontro la fruta");
//     }

//     frutas.Remove(fruta);
//     return Results.Ok();
// });

// ======================
// PARA LOS TODOS
// ======================

// var todos = new List<Todo>();
// var nextTodoId = 1;

// var users = new List<User>();
// var nextUserId = 1;

// ======================
// CREAR UN NUEVO USUARIO
// ======================
app.MapPost("/users", (string name) =>
{
    if (string.IsNullOrWhiteSpace(name))    
    {
        return Results.BadRequest("El campo 'name' es obligatorio");
    }

    var user = new User
    {
        Id = nextUserId++,
        Name = name
    };

    users.Add(user);
    return Results.Ok(user);
});

// ======================
// LISTAR TODOS LOS USUARIOS
// ======================
app.MapGet("/users", () =>
{
    var response = users.Select(user => new
    {
        user.Id,
        user.Name
    }).ToList();
    return Results.Ok(response);
});

// // ======================
// // CREAR UN NUEVO TODO
// // ======================
// app.MapPost("/todos", (CreateTodoDto dto) =>
// {
//     //  Nuestras validaciones
//     if (string.IsNullOrWhiteSpace(dto.Title)) // Mejor que dto.Title == null || dto.Title == ""
//     {
//         return Results.BadRequest("El campo 'title' es obligatorio");
//     }

//     var user = users.Find(user => user.Id == dto.UserId);
//     // Console.WriteLine(user);

//     if (user == null)
//     {
//         return Results.NotFound("No existe ese usuario");
//     }

//     var todo = new Todo
//     {
//         Id = nextTodoId++,
//         Title = dto.Title,
//         IsCompleted = false,
//         User = user
//     };

//     todos.Add(todo);
//     user.Todos.Add(todo);

//     var response = new
//     {
//         todo.Id,
//         todo.Title,
//         todo.IsCompleted,
//         User = new
//         {
//             user.Id,
//             user.Name
//         }
//     };

//     return Results.Ok(response);  
// });

// // ======================
// // LISTART LOS TODOS
// // ======================
// app.MapGet("/todos", () =>
// {
//     return Results.Ok(todos.Select(t => new
//     {
//         t.Id,
//         t.Title,
//         t.IsCompleted,
//         User = new
//         {
//             t.User.Id,
//             t.User.Name
//         }
//     }));
// });

app.MapTodoEndpoints();

app.Run();

// ======================
// MODELO
// ======================
record Fruta(int Id, string Nombre);
