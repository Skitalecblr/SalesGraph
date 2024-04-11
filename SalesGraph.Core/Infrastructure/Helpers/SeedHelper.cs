using SalesGraph.Core.DataAccess.Models.Entities;

namespace SalesGraph.Core.Infrastructure.Helpers
{
    public static class SeedHelper
    {
        //Generate products based on static product list
        public static List<Product> GetProducts()
        {
            string[] productNames = {
                "iPhone 13",
                "Samsung Galaxy S21",
                "Google Pixel 6",
                "OnePlus 10",
                "Xiaomi Mi 12",
                "Sony Xperia 5 III",
                "Huawei P50 Pro",
                "LG Velvet 2 Pro",
                "Motorola Edge+",
                "Asus ROG Phone 5",
                "Nokia 9.3 PureView",
                "Oppo Find X5",
                "Realme GT",
                "Vivo X70 Pro+",
                "Lenovo Legion Phone 2 Pro",
                "ZTE Axon 30 Ultra",
                "HTC Desire 21 Pro",
                "BlackBerry Key3",
                "Alcatel 3L (2021)",
                "TCL 20 Pro 5G",
                "Fairphone 4",
                "Poco X4 Pro",
                "CAT S62 Pro",
                "Redmi Note 11",
                "Meizu 18",
                "Gigaset GS4",
                "Sharp Aquos R6",
                "Tecno Camon 18",
                "Infinix Zero X Pro",
                "Xolo ZX Pro",
                "Micromax In 1b",
                "Lava Agni 2",
                "Panasonic Eluga I8",
                "Honor 50",
                "Vodafone Smart E10",
                "YotaPhone 3",
                "LeEco Le 3 Pro",
                "ZUK Edge",
                "Essential Phone PH-1",
                "Razer Phone 3",
                "Black Shark 4",
                "Nubia Red Magic 7",
                "Palm Phone",
                "Vertu Aster P",
                "CAT B40",
                "Sonim XP8",
                "Kyocera DuraXE",
                "Caterpillar CAT S42"
            };

            Random random = new Random();

            return productNames.Select(pn => new Product()
            {
                ProductId = Guid.NewGuid(),
                Name = pn,
                Cost = (decimal)(random.NextDouble() * (600 - 100) + 100)
            }).ToList();
        }

        //Generate sales statistics based on product list and date range
        public static List<SaleItem> GetSales(List<Product> products, DateTime startDate, DateTime endDate)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products), "Products cannot be empty");
            
            var result = new List<SaleItem>();
            var productsCount = products.Count();

            foreach (DateTime date in EachDay(startDate, endDate))
            {
                var todaySalesCount = (new Random()).Next(Constants.Generator.MinSalesPerDay,
                    Constants.Generator.MaxSalesPerDay);

                for (var i = 0; i < todaySalesCount; i++)
                {
                    var productIndex = new Random().Next(0, productsCount - 1);
                    result.Add(new SaleItem()
                    {
                        SaleItemId = Guid.NewGuid(),
                        SaleDate = date.Date,
                        ProductId = products[productIndex].ProductId,
                        Quantity = new Random().Next(1, Constants.Generator.MaxQuantity)
                    });
                }
            }

            return result;
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
    }
}
