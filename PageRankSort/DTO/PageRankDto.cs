using System.Collections.Generic;

namespace PageRankSort.DTO
{
    public class PageRankDto
    {
        public int PageNumber { get; set; }
        public List<int> Votes { get; set; }
        public float Weight { get; set; }
        public float WeightTemp { get; set; }
        private const string PageText = "Web Page ";
        public string GetPageName()
        {
            return PageText + PageNumber;
        }
    }
}
