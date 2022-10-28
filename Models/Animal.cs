namespace TataApp.Models
{
    public class Animal
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string Type { get; set; } = default!;

        public IFormFile Img { get; set; } = default!;
    }
}
