using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.APIs.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } =new List<Expression<Func<T, object>>> ();
        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }

        public BaseSpecification(Expression<Func<T, bool>> criteria )
        {
            Criteria = criteria;
        }

        public BaseSpecification()
        {
        }

        public void AddOrderBy (Expression<Func<T, object>> OrderByExpression) {
            OrderBy = OrderByExpression;
                }
        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderByDesc = OrderByDescExpression;
        }
        public void ApplyPagination(int skip, int take)
        {
            Skip= skip;
            Take= take;
            IsPagination = true;
        }
    }
}
