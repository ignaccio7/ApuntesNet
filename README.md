# Apuntes .net

Para crear un proyecto de .net usar la siguiente combicacion de teclas: 
ctrl + shift + p
elegir ".NET New Project"
Aqui luego seleccionar el tipo de proyecto quiza web api

Para correr el proyecto 

```bash
dotnet new console -> para crear un proyecto de consola.

dotnet add package NombrePaquete -> para instalar paquetes NuGet.

dotnet restore -> para restaurar paquetes.

dotnet run -> para ejecutar el proyecto.

dotnet clean -> borra bin/ y obj/ equivalente a rm -rf node_modules/.cache

dotnet restore -> equivalente a npm install

dotnet watch run -> ejecuta el proyecto en modo watch
```

Si nos saliera error por culpa del puerto https para instalar el certificado
```bash
dotnet dec-cert https --trust
```

## Variables

```csharp
int edad = 25;
string nombre = "Néstor";
bool activo = true;
double precio = 12.5;
```

## Metodos y funciones

```csharp
int Sumar(int a, int b)
{
    return a + b;
}
```

En C# todo método debe estar dentro de una clase.

## Clases y Objetos

```csharp
class Persona
{
    public string Nombre { get; set; }
    public int Edad { get; set; }

    public void Saludar()
    {
        Console.WriteLine($"Hola, soy {Nombre}");
    }
}
```

Usar la clase:

```csharp
var p = new Persona { Nombre = "Néstor", Edad = 20 };
p.Saludar();
```

```csharp
public class Todo
{
    // 1. Propiedades (lo básico)
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    
    // 2. Campo privado (para lógica interna)
    private DateTime _createdAt = DateTime.Now;

    // Constructor 1: Vacío
    public Todo()
    {
        CreatedAt = DateTime.Now;
        IsCompleted = false;
    }
    
    // Constructor 2: Solo título
    public Todo(string title) : this()  // Llama al constructor vacío primero
    {
        Title = title;
    }
    
    // Constructor 3: Con todos los datos
    public Todo(int id, string title, bool isCompleted)
    {
        Id = id;
        Title = title;
        IsCompleted = isCompleted;
        CreatedAt = DateTime.Now;
    }

    
    // 3. Propiedad con lógica
    public string Status 
    { 
        get { return IsCompleted ? "Completado" : "Pendiente"; }
    }
    
    // 4. Propiedad de solo lectura
    public DateTime CreatedAt => _createdAt;
    
    // 5. Métodos
    public void Complete()
    {
        IsCompleted = true;
    }
    
    // 6. Constructor
    public Todo(string title)
    {
        Title = title;
        IsCompleted = false;
    }
}
```

Uso 
```csharp
var todo1 = new Todo();                       // Constructor 1
var todo2 = new Todo("Hacer compras");        // Constructor 2  
var todo3 = new Todo(1, "Estudiar", false);   // Constructor 3
```


## Herencia / POO

```csharp
class Animal 
{ 
  public void Comer() => Console.WriteLine("Comiendo..."); 
}

class Perro : Animal
{
    public void Ladrar() => Console.WriteLine("Guau!");
}
```

## Colecciones
```csharp
var lista = new List<string>();
lista.Add("React");
lista.Add("C#");s
```
## Listas
```csharp
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
// enves de var podemos usar algo como
string[] summaries = new string[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
```

## Para crear una lista de objetos
```csharp
record Fruta(int Id, string Nombre);
var frutas = new List<Fruta>();
// agregamos datos
frutas.Add(new Fruta(1, "Mandarina"));
frutas.Add(new Fruta(2, "Naranja"));
// buscamos una mediante
var fruta = frutas.Find(f => f.Id == 1);
// eliminamos de la siguiente manera
frutas.Remove(fruta);
```

## Usando el TodoList

Para instalar swagger en nuestro proyecto en caso de que no venga por defecto.
> Para instalar paquetes podemos revisar la siguiente [documentacion](https://www.nuget.org/)

## 
```csharp
dotnet add package Swashbuckle.AspNetCore
```

Si diera error y no tuvieramos apuntando a la versión de nuget, podemos instalar la versión de nuget de swagger.

## Si saliera vacio
```bash
dotnet nuget list source
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
```

## Nuestro primer CRUD solo con un vector de frutas

Cosas importantes aqui:
1. Creamos un objeto o en este caso un record para almacenar los datos de la fruta,
`record Fruta(int Id, string Nombre);` En js simularia a tener un objeto con las propiedades Id y Nombre.
2. Al momento de actualizar la fruta
Usamos "with" para crear una COPIA modificada y luego la eliminamos y reasignamos a la lista.
3. Devolvemos las respuestas en la api con Results
Esto es importante porque si tratamos de devolver un string y mas abajo un objeto esto fallara.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Para que swagger descubra los endpoints
builder.Services.AddSwaggerGen(); // Para que swagger genere la documentacion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige automáticamente a HTTPS si alguien hace un request HTTP
app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };


// ======================
// DATOS
// ======================
var frutas = new List<Fruta>();
var nextId = 1;

// ======================
// GET - Listar frutas
// ======================
app.MapGet("/frutas", () =>
{
    return frutas;
});

// ======================
// POST - Crear fruta
// ======================
app.MapPost("/frutas", (string nombre) =>
{
    var fruta = new Fruta(nextId++, nombre);
    frutas.Add(fruta);
    return fruta;
});

// ======================
// GET - Obtener fruta por id
// ======================
app.MapGet("/frutas/{id}", (int id) =>
{
    var fruta = frutas.Find(f => f.Id == id);
    // Console.WriteLine(fruta);
    if (fruta == null)
    {
        return Results.NotFound("No existe esa fruta");
    }
    return Results.Ok(fruta);
});

// ======================
// PUT - Actualizar fruta
// ======================
app.MapPut("/frutas/{id}", (int id, string nombre) =>
{
    var fruta = frutas.Find(f => f.Id == id);

    if (fruta == null)
    {
        return Results.NotFound("No se encontro la fruta");
    }
    
    var nuevaFruta = fruta with { Nombre = nombre };    

    frutas.Remove(fruta);
    frutas.Add(nuevaFruta);

    return Results.Ok(nuevaFruta);    
});

// ======================
// DELETE - Eliminar fruta
// ======================
app.MapDelete("/frutas/{id}", (int id) =>
{
    var fruta = frutas.Find(f => f.Id == id);
    
    if (fruta == null)
    {
        return Results.NotFound("No se encontro la fruta");
    }

    frutas.Remove(fruta);
    return Results.Ok();
});

app.Run();

// ======================
// MODELO
// ======================
record Fruta(int Id, string Nombre);
```

## Metodos importantes 

Para validaciones en strings

```csharp
// Validaciones comunes
string.IsNullOrEmpty(value); // Solo verifica null o vacío
value.Contains("texto"); // Si contiene subcadena
value.StartsWith("prefijo");
value.EndsWith("sufijo");
value.Length; // Longitud
int.TryParse("123", out int numero); // Intenta convertir a int
```

Aca tenemos para las listas LINQ (Language Integrated Query) es como tener métodos funcionales de arrays en JavaScript pero mucho más potentes.

Comparando con **JS**
```javascript
// JavaScript/TypeScript
const todos = [{id: 1, title: "A", completed: true}, {id: 2, title: "B", completed: false}];

// Map
const response = todos.map(t => ({
    id: t.id,
    title: t.title
    // No incluimos completed
}));

// Filter
const completed = todos.filter(t => t.completed);

// Find
const todo = todos.find(t => t.id === 1);
```

En **csharp**

```csharp
// C# con LINQ
var todos = new List<Todo> 
{ 
    new Todo { Id = 1, Title = "A", IsCompleted = true },
    new Todo { Id = 2, Title = "B", IsCompleted = false }
};

// Select = map
var response = todos.Select(t => new TodoResponseDto
{
    Id = t.Id,
    Title = t.Title
    // No incluimos IsCompleted
}).ToList();  // <- IMPORTANTE: ToList() ejecuta la consulta

// Where = filter
var completed = todos.Where(t => t.IsCompleted).ToList();

// FirstOrDefault = find (mejor que Find)
var todo = todos.FirstOrDefault(t => t.Id == 1);

// También puedes encadenar (method chaining):
var result = todos
    .Where(t => !t.IsCompleted)        // filter
    .Select(t => t.Title)              // map a solo titles
    .OrderBy(title => title)           // sort
    .ToList();


El **ToList** sirve para ejecutar

// SIN ToList() - IQueryable/IEnumerable (ejecución diferida)
var query = todos.Select(t => new { t.Id, t.Title });
// Aún no se ejecutó, solo se preparó la consulta

// CON ToList() - ejecuta inmediatamente
var results = query.ToList();  // ¡Ahora sí se ejecuta y obtienes List<>

// Otros métodos de ejecución:
.ToArray()       // A array en vez de List
.First()         // Primer elemento
.Count()         // Cantidad
.Any()           // ¿Hay alguno?
```
Metodos mas comunes de LINQ

```csharp
// Proyección
.Select(t => t.Title)           // map: [t1, t2] → ["T1", "T2"]

// Filtrado
.Where(t => t.IsCompleted)      // filter
.Where(t => t.Title.Contains("urgent"))

// Ordenamiento
.OrderBy(t => t.Title)          // sort
.OrderByDescending(t => t.Id)
.ThenBy(t => t.CreatedAt)       // sort por múltiples campos

// Agrupación
.GroupBy(t => t.IsCompleted)    // group by
.GroupBy(t => t.CreatedAt.Date)

// Agregación
.Count()                        // length
.Any(t => t.IsCompleted)        // some
.All(t => t.Id > 0)             // every
.Sum(t => t.Priority)           // reduce para suma
.Average(t => t.Priority)       // promedio
.Min(t => t.CreatedAt)          // mínimo
.Max(t => t.CreatedAt)          // máximo

// Elementos
.First()                        // [0]
.FirstOrDefault()               // find o null
.Last()                         // [length-1]
.Single()                       // único elemento
.Skip(5).Take(10)               // paginación
.ElementAt(2)                   // [2]
```

##  Devolver la respuesta con Results
```csharp
Results.Ok(data);              // 200 OK
Results.Created(url, data);    // 201 Created
Results.NoContent();           // 204 No Content
Results.BadRequest(message);   // 400 Bad Request
Results.Unauthorized();        // 401 Unauthorized
Results.NotFound(message);     // 404 Not Found
Results.Conflict(message);     // 409 Conflict
Results.StatusCode(418);       // Cualquier código

```

## Utilizando clases en CSHARP para crear un controller

Empezamos creando tanto el modelo como el dto para la creacion de nuestra lista de todos

[TodoList/Models/Todo.cs](TodoList/Models/Todo.cs)
```csharp
namespace TodoList.Models;

public class Todo
{
  public int Id { get; set; }
  public string Title { get; set; }
  public bool IsCompleted { get; set; }

}
```

[TodoList/Dtos/CreateTodoDto.cs](TodoList/Dtos/CreateTodoDto.cs)
```csharp
namespace TodoList.Dtos;

public class CreateTodoDto
{
  public string Title { get; set; }
}
```

Y nuestro **program.cs** se volvera de la siguiente manera:

```csharp
using TodoList.Models;
using TodoList.Dtos;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Para que swagger descubra los endpoints
builder.Services.AddSwaggerGen(); // Para que swagger genere la documentacion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige automáticamente a HTTPS si alguien hace un request HTTP
app.UseHttpsRedirection();

// ======================
// DATOS
// ======================
var frutas = new List<Fruta>();
var nextId = 1;

// ....... todo el codigo anterior que teniamos de frutas

// ======================
// PARA LOS TODOS
// ======================

var todos = new List<Todo>();
var nextTodoId = 1;

// ======================
// CREAR UN NUEVO TODO
// ======================
app.MapPost("/todos", (CreateTodoDto dto) =>
{
    //  Nuestras validaciones
    if (string.IsNullOrWhiteSpace(dto.Title)) // Mejor que dto.Title == null || dto.Title == ""
    {
        return Results.BadRequest("El campo 'title' es obligatorio");
    }

    var todo = new Todo
    {
        Id = nextTodoId++,
        Title = dto.Title,
        IsCompleted = false        
    };

    todos.Add(todo);

    return Results.Ok(todo);  
});

// ======================
// LISTART LOS TODOS
// ======================
app.MapGet("/todos", () =>
{
    return Results.Ok(todos);
});

app.Run();

record Fruta(int Id, string Nombre);

```

## Mejorando nuestra API V1

Para este punto haremos lo siguiente tener 2 clases una de **Usuarios** y otra de **Todos** en la cual haremos uso de composicion para que cada **Todo** pertenezca a un respectivo **Usuario** y crear el **CRUD** completo y pasar aqui los archivos la estructura de carpetas que realizamos.

La estructura de carpetas que realizaremos es la siguiente;
```bash
TodoList/
│
├── Models/
│   ├── User.cs
│   └── Todo.cs
│
├── Dtos/
│   ├── CreateTodoDto.cs
│
├── Program.cs

```

`User.cs`
```csharp
namespace TodoList.Models;

public class User
{
  public int Id { get; set; }
  public string Name { get; set; }

  // Relacion -> un usuario tiene muchos todos
  // que sera opcional porque puede crearse un user sin todos
  public List< Todo>? Todos { get; set; } = new();
}
```

`Todo.cs`
```csharp
namespace TodoList.Models;

public class Todo
{
  public int Id { get; set; }
  public string Title { get; set; }
  public bool IsCompleted { get; set; }

  // Relacion -> un todo tiene un usuario
  public User User { get; set; } = null!;
}
```

`CreateTodoDto.cs`
```bash
namespace TodoList.Dtos;

public class CreateTodoDto
{
  public string Title { get; set; } = string.Empty;
  public int UserId { get;set; }
}
```

Nuestro `Program.cs` se volvera de la siguiente manera:
```bash
// ======================
// PARA LOS TODOS
// ======================

var todos = new List<Todo>();
var nextTodoId = 1;

var users = new List<User>();
var nextUserId = 1;

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
    return Results.Ok(users);
});

// ======================
// CREAR UN NUEVO TODO
// ======================
app.MapPost("/todos", (CreateTodoDto dto) =>
{
    //  Nuestras validaciones
    if (string.IsNullOrWhiteSpace(dto.Title)) // Mejor que dto.Title == null || dto.Title == ""
    {
        return Results.BadRequest("El campo 'title' es obligatorio");
    }

    var user = users.Find(user => user.Id == dto.UserId);
    // Console.WriteLine(user);

    if (user == null)
    {
        return Results.NotFound("No existe ese usuario");
    }

    var todo = new Todo
    {
        Id = nextTodoId++,
        Title = dto.Title,
        IsCompleted = false,
        User = user
    };

    todos.Add(todo);
    user.Todos.Add(todo);

    var response = new
    {
        todo.Id,
        todo.Title,
        todo.IsCompleted,
        User = new
        {
            user.Id,
            user.Name
        }
    };

    return Results.Ok(response);  
});

// ======================
// LISTART LOS TODOS
// ======================
app.MapGet("/todos", () =>
{
    return Results.Ok(todos.Select(t => new
    {
        t.Id,
        t.Title,
        t.IsCompleted,
        User = new
        {
            t.User.Id,
            t.User.Name
        }
    }));
});

app.Run();
```

### Agregando modificacion y eliminacion ademas de separar em Servicios y Rutas de API V2

Para este punto separaremos la logica en diferentes archivos crearemos tambien asi una interfaz para que el servicio lo implemente y luego proceder a separar los endpoints respectivos de la creacion de *todos* y sacarlos a un archivo aparte y usarlo en nuestro punto de entrada **program.cs**.

La estructura de carpetas que realizaremos es la siguiente;
```bash
TodoList/
│
├── Models/
│   ├── User.cs
│   └── Todo.cs
│
├── Dtos/
│   ├── CreateTodoDto.cs
│   └── UpdateTodoDto.cs
│   └── TodoResponseDto.cs
│
├── Services/
│   ├── ITodoService.cs
│   └── TodoService.cs
│
├── Endpoints/
│   └── TodoEndpoints.cs
│
├── Program.cs
```

Nuestros DTO`s
1. CreateTodoDto.cs
2. UpdateTodoDto.cs
3. TodoResponseDto.cs
```csharp
namespace TodoList.Dtos;

public class CreateTodoDto
{
  public string Title { get; set; } = string.Empty;
  public int UserId { get;set; }
}

namespace TodoList.Dtos;

public class UpdateTodoDto
{
  public string Title { get; set; } = string.Empty;
  public bool IsCompleted { get; set; }
}

namespace TodoList.Dtos;

public class TodoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}

```

`TodoEndpoints.cs`
```csharp
using TodoList.Dtos;
using TodoList.Services;
using TodoList.Models;

namespace TodoList.Endpoints;

public static class TodoEndpoints
{
  public static void MapTodoEndpoints(this WebApplication app)
  {
    app.MapGet("/todos", (ITodoService service) =>
    {
      var todos = service.GetAll();
      return Results.Ok(todos);
    });

    app.MapPost("/todos", (CreateTodoDto dto, ITodoService service) =>
    {
      var todo = service.Create(dto.Title, dto.UserId);

      return Results.Ok(todo);
    });

    app.MapPut("/todos/{id}", (int id, UpdateTodoDto dto, ITodoService service) =>
    {
      var ok = service.Update(id, dto.Title, dto.IsCompleted);
      return ok ? Results.Ok() : Results.NotFound();
    });

    app.MapDelete("/todos/{id}", (int id, ITodoService service) =>
    {
      var ok = service.Delete(id);
      return ok ? Results.Ok() : Results.NotFound();
    });

  }
}
```

`ITodoService.cs`
```csharp
using TodoList.Models;
using TodoList.Dtos;
namespace TodoList.Services;

public interface ITodoService
{
  List<TodoResponseDto> GetAll();
  TodoResponseDto Create(string title, int userId);
  bool Update(int id, string title, bool isCompleted);
  bool Delete(int id);
}
```

`TodoService.cs`
```csharp
using TodoList.Models;
using TodoList.Dtos;

namespace TodoList.Services;

public class TodoService : ITodoService
{
  private readonly List<User> _users;
  private readonly List<Todo> _todos;
  private int _nextId = 1;

  public TodoService(List<User> users, List<Todo> todos)
  {
    _users = users;
    _todos = todos;
  }

  public List<TodoResponseDto> GetAll()
  {
    return _todos.Select(t => new TodoResponseDto
    {
      Id = t.Id,
      Title = t.Title,
      IsCompleted = t.IsCompleted,
      UserId = t.User.Id,
      UserName = t.User.Name
    }).ToList();
  }

  public TodoResponseDto Create(string title, int userId)
  {
    var user = _users.Find(u => u.Id == userId);
    Console.WriteLine(user);
    if (user == null)
    {
      throw new Exception("Usuario no existe");
    }
    var todo = new Todo
    {
      Id = _nextId++,
      Title = title,
      User = user
    };

    _todos.Add(todo);
    user.Todos.Add(todo);

    return new TodoResponseDto
    {
      Id = todo.Id,
      Title = todo.Title,
      IsCompleted = todo.IsCompleted,
      UserId = todo.User.Id,
      UserName = todo.User.Name
    };
  }

  public bool Update(int id, string title, bool isCompleted)
  {
    var todo = _todos.Find(t => t.Id == id);
    if (todo == null)
    {
      throw new Exception("Todo no existe");
    }

    todo.Title = title;
    todo.IsCompleted = isCompleted;

    Console.WriteLine("El todo es:::::::::::::::::::::::::");
    Console.WriteLine($"Id: {todo.Id}");
    Console.WriteLine($"Title: {todo.Title}");
    Console.WriteLine($"IsCompleted: {todo.IsCompleted}");
    Console.WriteLine($"UserId: {todo.User.Id}");


    return true;
  }

  public bool Delete(int id)
  {
    var todo = _todos.Find(t => t.Id == id);
    if (todo == null)
    {
      throw new Exception("Todo no existe");
    }

    _todos.Remove(todo);
    todo.User.Todos.Remove(todo);
    return true;
  }

}
```

`Program.cs`
```csharp
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

app.MapTodoEndpoints();

app.Run();

```


```bash

```

```bash

```

```bash

```



## 
```csharp

```

```bash

```
```bash

```
```bash

```
```bash

```
```bash

```
```bash

```
```bash

```
```bash

```
