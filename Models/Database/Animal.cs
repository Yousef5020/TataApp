namespace TataApp.Models.Database
{
    public class Animal
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string Type { get; set; } = default!;

        public string ImgUrl { get; set; } = default!;
    }
}
