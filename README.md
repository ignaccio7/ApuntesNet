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

## Agregando modificacion y eliminacion ademas de separar em Servicios y Rutas de API V2

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

## Finalizando la modificacion y eliminacion ademas de separar em Servicios y Rutas de API tanto en **Todos** como en **Users** V3

* Models → entidades internas
* Dtos → lo que entra / sale de la API
* Services → lógica
* Endpoints → rutas

```bash
TodoList/
│
├── Models/
│   ├── User.cs
│   └── Todo.cs
│
├── Dtos/
│   ├── CreateUserDto.cs
│   ├── UserResponseDto.cs
│   ├── CreateTodoDto.cs
│   └── UpdateTodoDto.cs
│
├── Services/
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── ITodoService.cs
│   └── TodoService.cs
│
├── Endpoints/
│   ├── UserEndpoints.cs
│   └── TodoEndpoints.cs
│
├── Program.cs
```

`CreateUserDto.cs`
```csharp
namespace TodoList.Dtos;

public class CreateUserDto
{
  public string Name { get; set; } = string.Empty;
}
```


`UserResponseDto.cs`
```csharp
namespace TodoList.Dtos;

public class UserResponseDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
}
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

`IUserService.cs`
```csharp
using TodoList.Dtos;

namespace TodoList.Services;

public interface IUserService
{
  List<UserResponseDto> GetAll();
  UserResponseDto Create(string name);
}
```


`UserService.cs`
```csharp
using TodoList.Models;
using TodoList.Dtos;

namespace TodoList.Services;

public class UserService : IUserService
{
  private readonly List<User> _users;
  private int _nextId = 1;

  public UserService(List<User> users)
  {
    _users = users;
  }

  public List<UserResponseDto> GetAll()
  {
    return _users.Select(user => new UserResponseDto
    {
      Id = user.Id,
      Name = user.Name
    }).ToList();
  }

  public UserResponseDto Create(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new Exception("El nombre es obligatorio");

    var user = new User
    {
      Id = _nextId++,
      Name = name
    };

    _users.Add(user);

    return new UserResponseDto
    {
      Id = user.Id,
      Name = user.Name
    };
  }

}
```


`UserEndpoints.cs`
```csharp
using TodoList.Dtos;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Endpoints;

public static class UserEndpoints
{
  // Es como decir “Quiero que WebApplication tenga un método nuevo llamado MapUserEndpoints()”
  // antes -> UserEndpoints.MapUserEndpoints(app);
  // despues -> app.MapUserEndpoints();
  public static void MapUserEndpoints(this WebApplication app)
  {
    app.MapGet("/users", (IUserService service) =>
    {
      var users = service.GetAll();
      return Results.Ok(users);
    });

    app.MapPost("/users", (CreateUserDto dto, IUserService service) =>
    {
      var user = service.Create(dto.Name);
      return Results.Ok(user);
    });
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
// var nextUserId = 1;

builder.Services.AddSingleton(users);
builder.Services.AddSingleton(todos);
builder.Services.AddSingleton<IUserService, UserService>();
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


app.MapUserEndpoints();
app.MapTodoEndpoints();

app.Run();

```

## Agregamos validacion con FLuentValidation

✅ Declarativa
✅ Reutilizable
✅ Separada de la lógica de negocio
✅ Muy usada en proyectos profesionales
👉 NO valida strings sueltos, valida objetos completos (DTOs).


Evita esto 👇 (validaciones manuales repetidas):
```csharp
if (string.IsNullOrWhiteSpace(dto.Name))
{
    return Results.BadRequest("Name es obligatorio");
}
```

Y lo reemplaza por:

```csharp
RuleFor(x => x.Name)
    .NotEmpty()
    .MinimumLength(3);
```

Para instalarlo de la siguiente pagina [FluentValidation](https://www.nuget.org/packages/FluentValidation) y [DependencyInjectionExtensions](https://www.nuget.org/packages/FluentValidation.DependencyInjectionExtensions). Y luego veremos las dependencias en el `TodoList.csproj`.

```bash
dotnet add package FluentValidation --version 12.1.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 12.1.1
```

Tambien podemos ver los paquetes instalador con el siguiente comando:
```bash
dotnet list package
```

Porque instalamos dos paquetes. 
* FluentValidation
Te da:
 1. AbstractValidator<T>
 2. RuleFor
 3. NotEmpty, MinimumLength, etc
 4. Validate, ValidateAsync
👉 Solo valida, no sabe nada de ASP.NET

* FluentValidation.DependencyInjectionExtensions
Te da:
 1. AddValidatorsFromAssemblyContaining<T>()
 2. Integración con IServiceCollection
 3. Permite inyectar validadores en endpoints

👉 Esto es lo que necesitamos

Para ello la estructura de carpetas que realizaremos es la siguiente;

```bash
TodoList/
│
├── Models/
│   ├── User.cs
│   └── Todo.cs
│
├── Dtos/
│   ├── CreateUserDto.cs
│   ├── UserResponseDto.cs
│   ├── CreateTodoDto.cs
│   └── UpdateTodoDto.cs
│
├── Services/
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── ITodoService.cs
│   └── TodoService.cs
│
├── Endpoints/
│   ├── UserEndpoints.cs <- Cambiaremos este archivo
│   └── TodoEndpoints.cs
│
├── Validators/
│   ├── CreateUserDtoValidator.cs <- Crearemos este para validacion
│
├── Program.cs <- Registraremos los validadores aqui
```

El flujo que seguira nuestra aplicacion sera el siguiente:
```bash
📦 JSON del cliente
        ↓
🧩 DTO (CreateTodoDto)
        ↓
✅ FluentValidation
        ↓
❌ Errores → ValidationProblem (400)
        ↓
✅ OK → Service
        ↓
🧠 Lógica de negocio
        ↓
📤 Respuesta
```


`CreateUserDtoValidator.cs`
```csharp
using FluentValidation;
using TodoList.Dtos;

namespace TodoList.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
  public CreateUserDtoValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("El campo 'name' es obligatorio")
      .MinimumLength(3).WithMessage("El campo 'name' debe tener al menos 3 caracteres");
  }
  
}
```

`UserEndpoints.cs`
```csharp
// Para entender la validacion primero agregamos en el metodo POST
 app.MapPost("/users", async (
      CreateUserDto dto, 
      IValidator<CreateUserDto> validator, 
      IUserService service
    ) =>
    {

      var result = await validator.ValidateAsync(dto);

      if(!result.IsValid)
      {
        return Results.ValidationProblem(
          result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
              g => g.Key,
              g => g.Select(e => e.ErrorMessage).ToArray()
            )
          ); 
      }

      var user = service.Create(dto.Name);
      return Results.Ok(user);
    });
  }
```

`Program.cs`
```csharp
using TodoList.Models;
using TodoList.Dtos;
using TodoList.Services;
using TodoList.Endpoints;
using TodoList.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Para que swagger descubra los endpoints
builder.Services.AddSwaggerGen(); // Para que swagger genere la documentacion

var todos = new List<Todo>();
// var nextTodoId = 1;

var users = new List<User>();
// var nextUserId = 1;

builder.Services.AddSingleton(users);
builder.Services.AddSingleton(todos);
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITodoService, TodoService>();

// Registrar validadores automáticamente
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige automáticamente a HTTPS si alguien hace un request HTTP
app.UseHttpsRedirection();


app.MapUserEndpoints();
app.MapTodoEndpoints();

app.Run();
```

Y finalmente al verlo funcionando y probando en la API veremos resultados de esta manera:
`response`
```json
Error: Bad Request
Response body
Download
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": [
      "El campo 'name' es obligatorio",
      "El campo 'name' debe tener al menos 3 caracteres"
    ]
  }
}
```

### Finalmente agregamos las validaciones correspondientes a nuestro **TODO**

```bash
TodoList/
│
├── Models/
│   ├── User.cs
│   └── Todo.cs
│
├── Dtos/
│   ├── CreateUserDto.cs
│   ├── CreateTodoDto.cs
│   ├── UpdateTodoDto.cs
│
├── Validators/
│   ├── CreateUserDtoValidator.cs
│   ├── CreateTodoDtoValidator.cs
│   └── UpdateTodoDtoValidator.cs
│
├── Services/
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── ITodoService.cs
│   └── TodoService.cs
│
├── Endpoints/
│   ├── UserEndpoints.cs
│   └── TodoEndpoints.cs
│
├── Program.cs

```

Agregando los archvivos de validacion que nos faltaban para nuestro servicio del TODO se volverian de la siguiente manera:

`CreateTodoDtoValidator.cs`
```csharp
using FluentValidation;
using TodoList.Dtos;

namespace Todolist.Validators;

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
{
  public CreateTodoDtoValidator()
  {
    RuleFor(x => x.Title)
      .NotEmpty().WithMessage("El titulo es obligatorio")
      .MinimumLength(10).WithMessage("El titulo debe tener al menos 10 caracteres");

    RuleFor(x => x.UserId)
      .NotEmpty().WithMessage("El campo 'userId' es obligatorio")
      .GreaterThan(0).WithMessage("El userId debe ser mayor a 0");

  }
}
```

`UpdateTodoDtoValidator.cs`
```csharp
using FluentValidation;
using TodoList.Dtos;

namespace TodoList.Validators;

public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
{

  public UpdateTodoDtoValidator()
  {
    RuleFor(x => x.Title)
      .NotEmpty().WithMessage("El titulo es obligatorio")
      .MinimumLength(10).WithMessage("El titulo debe tener al menos 10 caracteres");
  }

}
```

`TodoEndpoints.cs`
```csharp
using TodoList.Dtos;
using TodoList.Services;
using TodoList.Models;
using TodoList.Validators;
using FluentValidation;

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

    // app.MapPost("/todos", (CreateTodoDto dto, ITodoService service) =>
    // {
    //   var todo = service.Create(dto.Title, dto.UserId);

    //   return Results.Ok(todo);
    // });

    app.MapPost("/todos", async (CreateTodoDto dto, ITodoService service,IValidator<CreateTodoDto> validator) =>
    {
      var result = await validator.ValidateAsync(dto);

      Console.WriteLine(result);
      Console.WriteLine(result.Errors);

      if(!result.IsValid)
      {
        var errors = result.Errors
          .GroupBy(e => e.PropertyName)
          .ToDictionary(
            g => g.Key,
            g => g.Select(e => e.ErrorMessage).ToArray()
          );

        return Results.ValidationProblem(errors);
      }

      var todo = service.Create(dto.Title, dto.UserId);

      return Results.Ok(todo);
    });

    // app.MapPut("/todos/{id}", (int id, UpdateTodoDto dto, ITodoService service) =>
    // {
    //   var ok = service.Update(id, dto.Title, dto.IsCompleted);
    //   return ok ? Results.Ok() : Results.NotFound();
    // });

    app.MapPut("/todos/{id}", async (int id, UpdateTodoDto dto, IValidator<UpdateTodoDto> validator,ITodoService service) =>
    {
      var result = await validator.ValidateAsync(dto);

      if(!result.IsValid)
      {
        return Results.ValidationProblem(
          result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
              g => g.Key,
              g => g.Select(e => e.ErrorMessage).ToArray()
            )
          ); 
      }

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
Y el `Program.cs` se volvera de la siguiente manera y notaremos que no tiene cambio alguno puesto que solo basto com registrar la siguiente linea:

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
```

Con esta linea basto para que se registren todos los demas validadores que podamos necesitar en nuestro proyecto.

`Program.cs`
```csharp
using TodoList.Models;
using TodoList.Dtos;
using TodoList.Services;
using TodoList.Endpoints;
using TodoList.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Para que swagger descubra los endpoints
builder.Services.AddSwaggerGen(); // Para que swagger genere la documentacion

var todos = new List<Todo>();
// var nextTodoId = 1;

var users = new List<User>();
// var nextUserId = 1;

builder.Services.AddSingleton(users);
builder.Services.AddSingleton(todos);
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITodoService, TodoService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige automáticamente a HTTPS si alguien hace un request HTTP
app.UseHttpsRedirection();

app.MapUserEndpoints();
app.MapTodoEndpoints();

app.Run();

```

## Agregando un middleware global para manejar errores

Si recordamos en nuestro archivo `TodoService.cs` tenemos una linea que dispara un error al no encontrar el Todo al momento de actualizar
`TodoService.cs`
```csharp
...

    var todo = _todos.Find(t => t.Id == id);
    if (todo == null)
    {
      throw new Exception("Todo no existe"); <- Esta linea
    }
...
```

Esta linea **rompe la aplicacion** lo que no queremos. Asi que para eso lo que podemos hacer es en nuestro archivo `Program.cs` agregar un middleware global que maneje los errores que puedan ocurrir en nuestro proyecto.
El mas sencillo seria de esta forma:

`Program.cs`
```csharp
...
var app = builder.Build();


// Como manejamos errores globales
// ✅ 1. Middleware de errores SIEMPRE ARRIBA
app.UseExceptionHandler(errorApp =>
{
    Console.WriteLine("ErrorApp");
    Console.WriteLine(errorApp);
    
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Ocurrió un error inesperado"
        });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
...
```

Ya si queremos mejorar con clases personalizadas nuestros errores lo haremos de la siguiente manera:

Crearemos la carpeta `Exceptions` y dentro de ella crearemos el archivo o archivos en este caso `NotFoundException.cs` que heredaran de la clase `Exception` y que nos permitirá crear nuestros propios errores personalizados.

`NotFoundException.cs`
```csharp
namespace TodoList.Exceptions;

public class NotFoundException : Exception
{
  public NotFoundException(string message) : base(message) {}
}
```

Y en nuestro `Program.cs` modificaremos la funcion de la siguiente manera:

`Program.cs`
```csharp
using TodoList.Models;
using TodoList.Dtos;
using TodoList.Services;
using TodoList.Endpoints;
using TodoList.Validators;
using FluentValidation;
using TodoList.Exceptions;
using Microsoft.AspNetCore.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Para que swagger descubra los endpoints
builder.Services.AddSwaggerGen(); // Para que swagger genere la documentacion

var todos = new List<Todo>();
// var nextTodoId = 1;

var users = new List<User>();
// var nextUserId = 1;

builder.Services.AddSingleton(users);
builder.Services.AddSingleton(todos);
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITodoService, TodoService>();

// Registrar validadores automáticamente
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

var app = builder.Build();


// Como manejamos errores globales
// ✅ 1. Middleware de errores SIEMPRE ARRIBA
app.UseExceptionHandler(errorApp =>
{    
    errorApp.Run(async context =>
    {
        
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is NotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                error = exception.Message
            });
            return;
        }
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Ocurrió un error inesperado"
        });
    });
});
```

## Agregando BD a nuestro proyecto 

### Migrando primero el servicio de User

Para ello instalaremos las siguientes librerias:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# PERO SI ESTAMOS CON LA VERSION 8.0 DE .NET ENTONCES LO QUE HACEMOS ES LO SIGUIENTE
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

```
| Paquete | Función |
| ------- | ------- |
|EntityFrameworkCore|Núcleo del ORM|
|SqlServer|	Proveedor SQL Server |
|Tools| Migraciones (dotnet ef)|

Ahora instalaremos una herramiento de cli para que podamos ejecutar y crear nuestras migraciones.

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```

Con esto podremos ejecutar lo siguiente:

| Comando                       | Para qué                          |
| ----------------------------- | --------------------------------- |
| `dotnet ef migrations add`    | Generar migraciones (archivos C#) |
| `dotnet ef database update`   | Ejecutar migraciones en la DB     |
| `dotnet ef migrations remove` | Borrar última migración           |
| `dotnet ef dbcontext info`    | Ver DbContext                     |

> Esto unicamente nos servira para nuestras migraciones ya que no esque sera una dependencia o algo que se subira a produccion

Una vez que tengamos todo bien ejecutaremos las migraciones y veremos lo siguiente en nuestra consola:

```bash
PS C:\Users\nirg2\Desktop\EQUIPO\PRACTICAS\ApuntesNET\TodoList> dotnet ef migrations add InitialCreate
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
```

Ya con todo esto veremos que se crea una carpeta de nombre Migrations y dentro de esta encontraremos nuestras migraciones que se usaran para comunicarnos y gestionar nuestra base de datos.

#### Procedemos a modificar nuestros archivos para que se ejecuten ya no almacenando localmente sino en la Base de datos

Creamos el AppDbContext que es una sesión temporal con la base de datos

`TodoList\Context\AppDbContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // EF crea SQL automáticamente a partir de tus 
    // null! esto significa que el compilador que se inicializara en runtime
    public DbSet<User> Users { get; set; } = null!;
}
```

Ahora creamos la cadena de conexion en nuestro archivo

`TodoList\appsettings.json.cs`
```csharp
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TodoListDb;User Id=sa;Password=Password123!;TrustServerCertificate=True"
  }
}
```

Y ahora en nuestro Program.cs registramos la sesión de la base de datos para que asi se pueda usar los servicios de base de datos en nuestro proyecto.

`Program.cs`
```csharp
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using TodoList.Dtos;
using TodoList.Services;
using TodoList.Endpoints;
using TodoList.Validators;
using FluentValidation;
using TodoList.Exceptions;
using TodoList.Context;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Obtenemos la cadena de conexion
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString) // Conectamos a la base de datos
);
builder.Services.AddScoped<IUserService, UserService>(); // Registramos los servicios para que estos puedan ser accedidos a la base de datos

builder.Services.AddEndpointsApiExplorer(); // Para que swagger descubra los endpoints
builder.Services.AddSwaggerGen(); // Para que swagger genere la documentacion

var todos = new List<Todo>();

builder.Services.AddSingleton(todos);
builder.Services.AddSingleton<ITodoService, TodoService>();

// Registrar validadores automáticamente
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

var app = builder.Build();


// Como manejamos errores globales
// ✅ 1. Middleware de errores SIEMPRE ARRIBA
app.UseExceptionHandler(errorApp =>
{    
    errorApp.Run(async context =>
    {
        
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is NotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                error = exception.Message
            });
            return;
        }
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Ocurrió un error inesperado"
        });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige automáticamente a HTTPS si alguien hace un request HTTP
app.UseHttpsRedirection();

app.MapUserEndpoints();
app.MapTodoEndpoints();

app.Run();
```

Y ahora modificamos nuestro servicio como nuestra interfaz
`TodoList\Services\IUserService.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using TodoList.Dtos;
using TodoList.Models;

namespace TodoList.Services;

public interface IUserService
{
  Task<List<User>> GetAllAsync();
  Task<User> CreateAsync(string name);
}
```

`TodoList\Services\UserService.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using TodoList.Dtos;
using TodoList.Context;

namespace TodoList.Services;

public class UserService : IUserService
{
  private readonly AppDbContext _context;

  public UserService(AppDbContext context)
  {
    _context = context;
  }

  public async Task<List<User>> GetAllAsync()
  {
    return await _context.Users.ToListAsync();
  }

  public async Task<User> CreateAsync(string name)
  {
    var user = new User
    {
      Name = name
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return user;
  }

}
```

Tambien modificamos nuestro modelo de usuario

`TodoList\Models\User.cs`
```csharp
namespace TodoList.Models;

public class User
{
  public int Id { get; set; }
  public string Name { get; set; }
}
```

Y por ultimo los endpoints para que sean asincronos y usen los nuevos metodos del servicio

`TodoList\Endpoints\UserEndpoints.cs`
```csharp
using TodoList.Dtos;
using TodoList.Models;
using TodoList.Services;
using TodoList.Validators;
using FluentValidation;

namespace TodoList.Endpoints;

public static class UserEndpoints
{
  public static void MapUserEndpoints(this WebApplication app)
  {
    app.MapGet("/users", async (IUserService service) =>
    {
      var users = await service.GetAllAsync();
      return Results.Ok(users);
    });

    app.MapPost("/users", async (
      CreateUserDto dto, 
      IValidator<CreateUserDto> validator, 
      IUserService service
    ) =>
    {

      var result = await validator.ValidateAsync(dto);

      if(!result.IsValid)
      {
        return Results.ValidationProblem(
          result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
              g => g.Key,
              g => g.Select(e => e.ErrorMessage).ToArray()
            )
          ); 
      }

      var user = await service.CreateAsync(dto.Name);
      return Results.Ok(user);
    });
  }
}
```

Algo que no podemos olvidar es crear nuestra base de datos con ayuda de docker y un archivo:

`TodoList\database\docker-compose.yml`
```yaml
services:
  sqlserver:
    container_name: sqlserver
    image: mcr.microsoft.com/mssql/server:2022-latest

    ports:
      - "1433:1433"

    environment:
      ACCEPT_EULA: "Y"
      MSSQL_SA_PASSWORD: "Password123!"

    volumes:
      - sqlserver-data:/var/opt/mssql
    
volumes:
  sqlserver-data:
    external: false
```

Ahora para subir nuestros cambios a la db ejecutaremos lo siguiente:

```bash
PS C:\Users\nirg2\Desktop\EQUIPO\PRACTICAS\ApuntesNET\TodoList> dotnet ef database update
Build started...
Build succeeded.
System.ArgumentException: Keyword not supported: 'userid'.
   at Microsoft.Data.Common.DbConnectionOptions.ParseInternal(Dictionary`2 parsetable, String connectionString, Boolean buildChain, Dictionary`2 synonyms, Boolean firstKey)
   .
   .
   .
Done.
```



`.cs`
```csharp

```

`.cs`
```csharp

```

`.cs`
```csharp

```

`.cs`
```csharp

```

`.cs`
```csharp

```

`.cs`
```csharp

```

`.cs`
```csharp

```

`.cs`
```csharp

```

`.cs`
```csharp

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
