// Copyright (c) IEvangelist. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Azure.CosmosRepository.ChangeFeed
{
    /// <summary>
    /// A processor that can process changes from the changed feed.
    /// </summary>
    public interface IContainerChangeFeedProcessor
    {
        /// <summary>
        /// Starts the processor.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        /// Stops the processor.
        /// </summary>
        /// <returns></returns>
        Task StopAsync();

        /// <summary>
        /// The item types that this processor handlers.
        /// </summary>
        public IReadOnlyList<Type> ItemTypes { get; }
    }
}