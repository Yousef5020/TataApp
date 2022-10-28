using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TataApp.Data;
using TataApp.Models.Database;

namespace TataApp.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly TataDbContext context;

        public AnimalsController(TataDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            Animal[] animals = await context.Animals.ToArrayAsync();

            HttpContext.Session.SetString("123", "Hi");

            return View(animals);
        }

        [HttpGet("{controller}/GetAnimalPhoto/{id}")]
        public async Task<IActionResult> GetAnimalPhoto(Guid id, CancellationToken token)
        {
            string? animalImgUrl = await context.Animals.Where(a => a.Id == id).Select(x => x.ImgUrl).FirstOrDefaultAsync(token);

            if (animalImgUrl is null)
            {
                return NotFound();
            }

            var file = await System.IO.File.ReadAllBytesAsync(animalImgUrl, token);

            return File(file, "image/png");
        }

        [HttpGet]
        public IActionResult AddAnimal()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> AddAnimal(Models.Animal animal, CancellationToken token)
        {
            var newAnimal = await context.Animals.AddAsync(new Animal
            {
                Name = animal.Name,
                Description = animal.Description,
                Type = animal.Type,
            }, token);

            var dir = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\files\{newAnimal.Entity.Id}");

            if (!dir.Exists)
                dir.Create();

            using var file = System.IO.File.Create(@$"{dir.FullName}\{animal.Img.FileName}");

            await animal.Img.CopyToAsync(file, token);

            newAnimal.Entity.ImgUrl = file.Name;

            await context.SaveChangesAsync(token);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditAnimal(Guid id, CancellationToken token)
        {
            string s = HttpContext.Session.GetString("123") ?? "";

            Models.Animal? animal = await context.Animals
                .Where(animal => animal.Id == id)
                .Select(animal => new Models.Animal
                {
                    Id = animal.Id,
                    Description = animal.Description,
                    Name = animal.Name,
                    Type = animal.Type,
                })
                .FirstOrDefaultAsync(token);

            if (animal is null)
                return NotFound();

            return View(animal);
        }

        [HttpPost]
        public async Task<IActionResult> EditAnimal(Models.Animal formAnimal, CancellationToken token)
        {
            Animal? animal = await context.Animals.FirstOrDefaultAsync(animal => animal.Id == formAnimal.Id, token);

            if (animal is null)
                return NotFound();

            animal.Name = formAnimal.Name;
            animal.Type = formAnimal.Type;
            animal.Description = formAnimal.Description;

            if (formAnimal.Img is not null && formAnimal.Img.Length > 0)
            {
                var dir = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\files\{animal.Id}");

                if (!dir.Exists)
                    dir.Create();

                System.IO.File.Delete(animal.ImgUrl);

                using var file = System.IO.File.Create(@$"{dir.FullName}\{formAnimal.Img.FileName}");

                await formAnimal.Img.CopyToAsync(file, token);

                animal.ImgUrl = file.Name;
            }

            await context.SaveChangesAsync(token);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAnimal(Guid id, CancellationToken token)
        {
            Animal? animal = await context.Animals
                .Where(animal => animal.Id == id)
                .FirstOrDefaultAsync(token);

            if (animal is null)
                return NotFound();

            context.Animals.Remove(animal);

            await context.SaveChangesAsync(token);

            System.IO.File.Delete(animal.ImgUrl);

            return RedirectToAction(nameof(Index));
        }
    }
}
