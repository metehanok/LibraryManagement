using LibraryManagementAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Tests.Unit.Helper
{
    public class MockDbSet<T> : DbSet<T>, IQueryable<T> where T : class
    {
        private readonly List<T> _data;
        private readonly IQueryable<T> _queryable;

        public MockDbSet(List<T> data)
        {
            _data = data;
            _queryable = data.AsQueryable();
        }

        public override ValueTask<T?> FindAsync(params object[] keyValues)
        {
            var entity = _data.SingleOrDefault(e => keyValues.Contains((e as Book)?.Id));
            return new ValueTask<T?>(entity);
        }

        public Type ElementType => _queryable.ElementType;
        public Expression Expression => _queryable.Expression;
        public IQueryProvider Provider => _queryable.Provider;

        public override IEntityType EntityType => throw new NotImplementedException();

        public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
