// Copyright (c) IEvangelist. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosRepository.Extensions;
using Microsoft.Azure.CosmosRepository.Paging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Azure.CosmosRepository
{
    internal partial class InMemoryRepository<TItem>
    {
        /// <inheritdoc/>
        public ValueTask<IPage<TItem>> PageAsync(
            Expression<Func<TItem, bool>>? predicate = null,
            int pageSize = 25,
            string? continuationToken = null,
            bool returnTotal = false,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async ValueTask<IPageQueryResult<TItem>> PageAsync(
            Expression<Func<TItem, bool>>? predicate = null,
            int pageNumber = 1,
            int pageSize = 25,
            bool returnTotal = false,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            IEnumerable<TItem> filteredItems = Items.Values
                .Select(DeserializeItem)
                .ToList();

            Expression<Func<TItem, bool>> typeCheck = item =>
                item.Type == typeof(TItem).Name;

            filteredItems = filteredItems.Where(predicate is not null
                ? predicate.Compose(typeCheck, Expression.AndAlso).Compile()
                : typeCheck.Compile());

            IEnumerable<TItem> enumerable = filteredItems.ToList();

            IEnumerable<TItem> items = enumerable
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            return new PageQueryResult<TItem>(
                returnTotal ? enumerable.Count() : null,
                pageNumber,
                pageSize,
                items.ToList().AsReadOnly(),
                0);
        }
    }
}