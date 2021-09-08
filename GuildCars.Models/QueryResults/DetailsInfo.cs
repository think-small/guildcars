namespace GuildCars.Models.QueryResults
{
    public class DetailsInfo
    {
        public int Id { get; set; }
        public DetailType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsKeyFeature { get; set; }

        public Detail ToDetail()
        {
            return new Detail
            {
                Id = Id,
                Type = Type,
                Name = Name,
                Description = Description,
                IsKeyFeature = IsKeyFeature
            };
        }
    }
}
