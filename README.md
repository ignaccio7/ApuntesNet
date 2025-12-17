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

## 
```csharp

```

## 
```csharp

```

## 
```csharp

```

## 
```csharp

```

## 
```csharp

```

## 
```csharp

```

## 
```csharp

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
