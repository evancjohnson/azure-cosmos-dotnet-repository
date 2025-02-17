// Copyright (c) IEvangelist. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosRepository.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Azure.CosmosRepository
{
    internal partial class InMemoryRepository<TItem>
    {
        /// <inheritdoc/>
        public async ValueTask<int> CountAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return Items.Values.Select(DeserializeItem).Count(item => item.Type == typeof(TItem).Name);
        }

        /// <inheritdoc/>
        public async ValueTask<int> CountAsync(Expression<Func<TItem, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return Items.Values.Select(DeserializeItem).Count(predicate.Compose(
                item => item.Type == typeof(TItem).Name, Expression.AndAlso).Compile());
        }
    }
}