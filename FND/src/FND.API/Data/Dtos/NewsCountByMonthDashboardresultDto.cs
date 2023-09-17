using FND.API.Entities;

namespace FND.API.Data.Dtos
{
    public class NewsCountByMonthDashboardresultDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int FakeCount { get; set; }
        public int RealCount { get; set; }
    }
}
