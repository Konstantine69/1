using System;
using System.Collections.Generic;

namespace XMLValidation
{
    public class AlpinistDiary
    {
        public List<Climb> Climbs { get; set; }

        public AlpinistDiary()
        {
            Climbs = new List<Climb>();
        }
    }

    public class Climb
    {
        public string PeakName { get; set; }
        public int Height { get; set; }
        public string Country { get; set; }
        public DateTime VisitDate { get; set; }
        public string ClimbTime { get; set; }
        public string DifficultyCategory { get; set; }
    }
}
