namespace Carat.Data.Entities
{
    public class Work
    {
        public int Id { get; set; }
        public int CurriculumItemId { get; set; }
        public int WorkTypeId { get; set; }
        public double TotalHours { get; set; }
    }
}
