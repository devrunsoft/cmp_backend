using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace CMPNatural.Core.Entities
{
	public partial class ChatClientSession
	{
        public long Id { get; set; }
        public long ClientId { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        public virtual ICollection<ChatSession> ChatSession { get; set; }
        public Company Company { get; set; }
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<TOuter, bool>> ApplyToNavigation<TOuter, TInner>(
            Expression<Func<TInner, bool>> innerFilter,
            Expression<Func<TOuter, TInner?>> navigationExpr
        ) where TInner : class
        {
            var param = navigationExpr.Parameters[0];
            var innerInvoke = Expression.Invoke(innerFilter, navigationExpr.Body);
            var nullCheck = Expression.NotEqual(navigationExpr.Body, Expression.Constant(null, typeof(TInner)));

            var combined = Expression.AndAlso(nullCheck, innerInvoke);
            return Expression.Lambda<Func<TOuter, bool>>(combined, param);
        }
    }
}

