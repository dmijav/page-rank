using PageRankSort.DTO;
using System.Collections.Generic;
using System.Linq;

namespace PageRankSort
{
    class CalculateHandler
    {
        private const int baseRank = 1;
        private const float df = 0.85f;
        private const int cycleCount = 5;
        private List<PageRankDto> data;

        public Dictionary<string, float> Calculate(string input)
        {
            var result = new GetXlsDataHandler().Get(input);
            return CalculateBase(result);
        }

        public Dictionary<string, float> CalculateBase(List<PageRankDto> _data)
        {
            data = _data;
            SetBaseRank();
            CalculateInner();
            SortPageRank();
            return GetResultView();
        }

        private Dictionary<string, float> GetResultView()
        {
            var result = new Dictionary<string, float>();
            foreach (var el in data) {
                result.Add(el.GetPageName(), el.Weight);
            }
            return result;
        }

        private void SortPageRank()
        {
            data = data.OrderByDescending(item => item.Weight).ToList();
        }

        private void CalculateInner(int count = 0)
        {
            if (count >= cycleCount) return;
            foreach (var el in data)
            {
                var list = data
                           .Where(x => x.PageNumber != el.PageNumber && x.Votes.Contains(el.PageNumber));
                var sumRanks = SumRanks(list);
                el.WeightTemp = (1 - df) + df * sumRanks;
            }
            foreach (var el in data)
            {
                el.Weight = el.WeightTemp;
            }
            count++;
            CalculateInner(count);
        }

        private float SumRanks(IEnumerable<PageRankDto> list)
        {
            float result = 0f;
            foreach (var el in list)
            {
                result += el.Weight / el.Votes.Count();
            }
            return result;
        }

        private void SetBaseRank()
        {
            foreach (var el in data)
            {
                el.Weight = baseRank;
            }
        }
    }
}