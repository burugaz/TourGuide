using Microsoft.EntityFrameworkCore;
using TourGuide.Web.Data;
using TourGuide.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tourguide.db"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cities}/{action=Index}/{id?}");

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Cities.Any())
    {
        var city1 = new City
        {
            Name = "Санкт-Петербург",
            Region = "Северо-Западный федеральный округ",
            Population = 5383968,
            History = "Основан в 1703 году Петром I.",
            CoatOfArmsUrl = "https://upload.wikimedia.org/wikipedia/commons/6/6f/Coat_of_arms_of_Saint_Petersburg_%28Lesser%29.svg",
            PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/3/33/Saint_Petersburg_skyline.jpg",
            Attractions = new List<Attraction>
            {
                new Attraction
                {
                    Name = "Эрмитаж",
                    Description = "Один из крупнейших художественных музеев мира.",
                    History = "Основан в 1764 году.",
                    PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/5/5b/Hermitage_Museum.jpg",
                    OpeningHours = "10:30 - 18:00",
                    Price = "20 USD"
                },
                new Attraction
                {
                    Name = "Исаакиевский собор",
                    Description = "Один из крупнейших соборов России.",
                    History = "Построен в 1858 году.",
                    PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/9/93/Saint_Isaac%27s_Cathedral.jpg",
                    OpeningHours = "10:30 - 18:00",
                    Price = "Free"
                }
            }
        };

        var city2 = new City
        {
            Name = "Москва",
            Region = "Центральный федеральный округ",
            Population = 12506468,
            History = "История насчитывает многие века.",
            CoatOfArmsUrl = "https://upload.wikimedia.org/wikipedia/commons/6/6f/Coat_of_Arms_of_Moscow.png",
            PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/6/69/Moscow_City.jpg",
            Attractions = new List<Attraction>
            {
                new Attraction
                {
                    Name = "Красная площадь",
                    Description = "Главная площадь страны.",
                    History = "Центр политической и общественной жизни.",
                    PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a4/Red_Square%2C_Moscow.jpg",
                    OpeningHours = "Круглосуточно",
                    Price = "Free"
                }
            }
        };

        db.Cities.AddRange(city1, city2);
        db.SaveChanges();
    }
}

app.Run();