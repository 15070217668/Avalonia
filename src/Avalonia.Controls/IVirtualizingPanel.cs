﻿// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;

namespace Avalonia.Controls
{
    /// <summary>
    /// A panel that can be used to virtualize items.
    /// </summary>
    public interface IVirtualizingPanel : IPanel
    {
        /// <summary>
        /// Gets a value indicating whether the panel is full.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Gets the number of items that can be removed while keeping the panel full.
        /// </summary>
        int OverflowCount { get; }

        /// <summary>
        /// Gets the direction of scroll.
        /// </summary>
        Orientation ScrollDirection { get; }

        /// <summary>
        /// Gets the average size of the materialized items in the direction of scroll.
        /// </summary>
        double AverageItemSize { get; }

        /// <summary>
        /// Gets or sets a size in pixels by which the content is overflowing the panel, in the
        /// direction of scroll.
        /// </summary>
        /// <remarks>
        /// This may be non-zero even when <see cref="OverflowCount"/> is zero if the last item
        /// overflows the panel bounds.
        /// </remarks>
        double PixelOverflow { get; }

        /// <summary>
        /// Gets or sets the current pixel offset of the items in the direction of scroll.
        /// </summary>
        double PixelOffset { get; set; }
    }
}
