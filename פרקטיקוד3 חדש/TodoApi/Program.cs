using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ודא שהוספת את החבילה Microsoft.EntityFrameworkCore.SqlServer
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB")));
    builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("tododb"), 
     ServerVersion.Parse("8.0.41-mysql"))); 
// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB")));
// הוספת CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// הפעלת CORS
app.UseCors("AllowAll");
// שליפת כל המשימות
app.MapGet("/items", async (ToDoDbContext context) =>
{
    return await context.Items.ToListAsync();
});
// הוספת משימה חדשה

app.MapPost("/items", async (ToDoDbContext context, Item item) =>
{   item.IsComplete=false;
    context.Items.Add(item);
    await context.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});


// עדכון משימה
app.MapPut("/items/{id}", async (ToDoDbContext context, int id, Item item) =>
{
    if (id != item.Id)
    {
        return Results.BadRequest("ID mismatch.");
    }

    var existingItem = await context.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }

    existingItem.IsComplete = item.IsComplete; // עדכון הסטטוס לפי מה שנשלח
    context.Entry(existingItem).State = EntityState.Modified;

    try
    {
        await context.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
        return Results.InternalServerError(); // החזרת קוד שגיאה 500
    }

    return Results.NoContent();
});
// מחיקת משימה
app.MapDelete("/items/{id}", async (ToDoDbContext context, int id) =>
{
    var item = await context.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }

    context.Items.Remove(item);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" }, 
   
                          () => "This is an options or head request ");

// הפעלת Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // אם אתה רוצה להציג את Swagger ב-root
});
app.Run();
