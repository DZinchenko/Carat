namespace Carat.Data.Entities
{
    public class TAItem
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int WorkId { get; set; }
        public double WorkHours { get; set; }
        public uint Course { get; set; }
        public string EducType { get; set; }
        public string EducForm { get; set; }
        public uint Semestr { get; set; }
        public string EducLevel { get; set; }
    }
}
