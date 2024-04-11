namespace SalesGraph.Core.DataAccess.Models.Infrastructure
{
    public class GroupedSaleItem
    {
        public string PeriodStartDate { get; set; }
        public int TotalSaleCount { get; set; }
        public decimal TotalSalesSum { get; set; }
    }

    public class GraphResult
    {
        public string[] X { get; private set; }
        public int[] Y { get; private set; }
        public decimal[] Z { get; private set; }

        public static GraphResult FromGroupedSaleItems(IEnumerable<GroupedSaleItem> items)
        {
            var result = new GraphResult
            {
                X = items.Select(i => i.PeriodStartDate).ToArray(),
                Y = items.Select(i => i.TotalSaleCount).ToArray(),
                Z = items.Select(i => i.TotalSalesSum).ToArray()
            };

            return result;
        }
    }
}
