using Talabat.Core.Entities;

namespace Talabat.APIs.Specifications.ProductsSpecs
{
    public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductsSpecParams productsSpec) : base(P =>

            (!productsSpec.BrandId.HasValue || P.BrandId == productsSpec.BrandId.Value) && (!productsSpec.CategoryId.HasValue || P.CategoryId == productsSpec.CategoryId.Value)
        )
        {
        }
    }
}
