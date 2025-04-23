using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace LibraryManagementWebAPI.Tests.Helpers
{

    // IAsyncQueryProvider implementasyonu

    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider,IQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> Execute<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        public  Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            // Synchronous Execute metodunu çağırarak sonucu döndürüyoruz
            var result = _inner.Execute<TResult>(expression);
            return  Task.FromResult(result);
        }
        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        // Buradaki ExecuteAsync metodunun doğru imzaya sahip olmasına dikkat et.
        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return _inner.Execute<TResult>(expression);
        }

        //TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IQueryable<T> _inner;
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable) // Gerçek veriyi baz alarak oluşturduk
        {
            _inner = enumerable.AsQueryable();
        }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        {
            _inner = new List<T>().AsQueryable();
        }

        public IQueryProvider Provider => new TestAsyncQueryProvider<T>(this.AsQueryable().Provider); // Asenkron sorgu sağlayıcısı
        public Type ElementType => _inner.ElementType;
        public Expression Expression => _inner.Expression;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(_inner.GetEnumerator());
        }
    }


    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _innerEnumerator;

        public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
        {
            _innerEnumerator = innerEnumerator;
        }

        public T Current => _innerEnumerator.Current;

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.FromResult(_innerEnumerator.MoveNext());
        }

        public ValueTask DisposeAsync() /*=> new ValueTask(Task.CompletedTask);*/
        {
            _innerEnumerator.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
