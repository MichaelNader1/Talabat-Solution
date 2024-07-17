using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Order;

namespace Talabat.Repository.Data
{
    public class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext _context)

        {

            if (_context.ProductCategories.Count() == 0)
            {
                var CategoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);
                if (Categories?.Count() > 0)
                {
                    foreach (var category in Categories)
                    {
                        _context.Set<ProductCategory>().Add(category);
                    }
                    await _context.SaveChangesAsync();
                }
            }


            if (_context.ProductBrands.Count()==0)
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if(Brands?.Count()>0 )
                {
                    foreach(var Brand in Brands) { 
                    _context.Set<ProductBrand>().Add(Brand);
                    }
                    await _context.SaveChangesAsync();
                }
            }


            if (_context.Products.Count() == 0)
            {
                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count() > 0)
                {
                    foreach (var Product in Products)
                    {
                        _context.Set<Product>().Add(Product);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.DeliveryMethod.Count() == 0)
            {
                var DeliveryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                if (DeliveryMethods?.Count() > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        _context.Set<DeliveryMethod>().Add(DeliveryMethod);
                    }
                    await _context.SaveChangesAsync();
                }
            }



        }
    }
}
