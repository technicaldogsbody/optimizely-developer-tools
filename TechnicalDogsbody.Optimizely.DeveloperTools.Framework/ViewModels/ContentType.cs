namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.ViewModels
{
    public record ContentType
    {
        public int Id { get; internal init; }
        public string Name { get; internal init; }
        public string ModelType { get; internal init; }
    }
}