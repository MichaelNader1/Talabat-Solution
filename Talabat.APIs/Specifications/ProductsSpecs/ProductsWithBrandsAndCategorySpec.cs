using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.APIs.Specifications.ProductsSpecs
{
    public class ProductsWithBrandsAndCategorySpec : BaseSpecification<Product>
    {
        public ProductsWithBrandsAndCategorySpec(ProductsSpecParams productsSpec) : base(P =>
            (string.IsNullOrEmpty(productsSpec.Search)||P.Name.ToLower().Contains(productsSpec.Search))&&
            (!productsSpec.BrandId.HasValue || P.BrandId == productsSpec.BrandId.Value) && (!productsSpec.CategoryId.HasValue || P.CategoryId == productsSpec.CategoryId.Value)
        )
        {
            Includes.Add(P=>P.Brand);
            Includes.Add(P=>P.Category);
            if (!string.IsNullOrEmpty(productsSpec.Sort))
            {
                switch(productsSpec.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p=>p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;

                }
            }
            else
            {
                AddOrderBy(p => p.Name);

            }

            ApplyPagination(productsSpec.PageSize * (productsSpec.PageIndex - 1), productsSpec.PageSize);

        }



        public ProductsWithBrandsAndCategorySpec(int id) : base(p=>p.Id==id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }



    }
}
