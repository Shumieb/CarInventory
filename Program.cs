using CarInventory;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CarDB>(opt => opt.UseInMemoryDatabase("CarList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

RouteGroupBuilder cars = app.MapGroup("/cars");

cars.MapGet("/", GetAllCars);

cars.MapPost("/", CreateCar);

cars.MapGet("/{id}", GetCar);

cars.MapDelete("/{id}", DeleteCar);

app.Run();

static async Task<IResult> GetAllCars(CarDB db)
{
    return TypedResults.Ok(await db.Cars.Select(x => new CarDTO(x)).ToArrayAsync());
}

static async Task<IResult> CreateCar(Car car,  CarDB db)
{
    db.Cars.Add(car);
    await db.SaveChangesAsync();

    var dto = new CarDTO(car);
    return TypedResults.Created($"/cars/{dto.Id}", dto);
}

static async Task<IResult> GetCar(int id, CarDB db)
{
    return await db.Cars.FindAsync(id)
        is Car car
            ? TypedResults.Ok(new CarDTO(car))
            : TypedResults.NotFound();
}

static async Task<IResult> DeleteCar(int id, CarDB db)
{
    if(await db.Cars.FindAsync(id) is Car car)
    {
        db.Cars.Remove(car);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NoContent();
}
