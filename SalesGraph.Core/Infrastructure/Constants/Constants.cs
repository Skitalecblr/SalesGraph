namespace SalesGraph.Core.Infrastructure.Constants
{
    public static class Generator
    {
        public static readonly int MaxSalesPerDay = 50;
        public static readonly int MinSalesPerDay = 20;
        public static DateTime MinDateTime = DateTime.Now.AddYears(-1);
        public static int MaxQuantity = 4;
    }
}
