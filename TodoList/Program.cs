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
