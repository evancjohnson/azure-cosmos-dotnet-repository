﻿// Copyright (c) IEvangelist. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Azure.CosmosRepository.Attributes;
using Microsoft.Azure.CosmosRepository.Builders;
using Microsoft.Azure.CosmosRepository.Options;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.CosmosRepository.Providers
{
    /// <inheritdoc cref="Microsoft.Azure.CosmosRepository.Providers.ICosmosContainerNameProvider" />
    class  DefaultCosmosContainerNameProvider : ICosmosContainerNameProvider
    {
        private readonly IOptions<RepositoryOptions> _options;

        public DefaultCosmosContainerNameProvider(IOptions<RepositoryOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public string GetContainerName<TItem>() where TItem : IItem =>
            GetContainerName(typeof(TItem));

        public string GetContainerName(Type itemType)
        {
            Type attributeType = typeof(ContainerAttribute);

            Attribute attribute =
                Attribute.GetCustomAttribute(itemType, attributeType);

            ContainerOptionsBuilder? optionsBuilder = _options.Value.GetContainerOptions(itemType);

            if (optionsBuilder is { } && string.IsNullOrWhiteSpace(optionsBuilder.Name) is false)
            {
                return optionsBuilder.Name!;
            }

            return attribute is ContainerAttribute containerAttribute
                ? containerAttribute.Name
                : itemType.Name;
        }
    }
}
