using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications
{
    public class BaseSpecifiaction<T> : ISpecification<T>
    {
        public BaseSpecifiaction()
        {
        }

        public BaseSpecifiaction(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Inculdes { get; } = new List<Expression<Func<T, object>>>();
        protected void AddInclude(Expression<Func<T,object>> includeExpression)
        {
            Inculdes.Add(includeExpression);
        }
    }
}
