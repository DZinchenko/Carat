namespace Carat.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double StaffUnit { get; set; } = 1.00;
        public int PositionId { get; set; }
        public string Rank { get; set; } = "-";
        public string Degree { get; set; } = "-";
        public string OccupForm { get; set; } = "Штатний";
        public string Note { get; set; }
    }
}
