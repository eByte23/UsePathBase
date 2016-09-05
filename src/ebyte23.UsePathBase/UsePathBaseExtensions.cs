// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder.Extensions;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class UsePathBaseExtensions
    {
        private static readonly string ServerPath = "APPL_PATH";
        /// <summary>
        /// Adds a middleware that extracts the specified path base from request path and postpend it to the request path base.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="pathBase">The path base to extract.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UsePathBase(this IApplicationBuilder app, PathString pathBase, bool isOptional = false)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Strip trailing slashes
            pathBase = pathBase.Value?.TrimEnd('/');

            return app.UseMiddleware<UsePathBaseMiddleware>(pathBase, isOptional);
        }

        public static IApplicationBuilder UsePathBaseEnvironment(this IApplicationBuilder app, bool isOptional = false)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var path = Environment.GetEnvironmentVariable($"ASPNETCORE_{ServerPath}");

            return app.UsePathBase(path, isOptional);
        }
    }

    public static class PathExtentions
    {
        public static bool StartsWithSegments(this PathString pathStr,PathString other, out PathString matched, out PathString remaining)
        {
            return StartsWithSegments(pathStr, other, StringComparison.OrdinalIgnoreCase, out matched, out remaining);
        }

        public static bool StartsWithSegments(PathString pathStr, PathString other, StringComparison comparisonType, out PathString matched, out PathString remaining)
        {
            var value1 = pathStr.Value ?? string.Empty;
            var value2 = other.Value ?? string.Empty;
            if (value1.StartsWith(value2, comparisonType))
            {
                if (value1.Length == value2.Length || value1[value2.Length] == '/')
                {
                    matched = new PathString(value1.Substring(0, value2.Length));
                    remaining = new PathString(value1.Substring(value2.Length));
                    return true;
                }
            }
            remaining = PathString.Empty;
            matched = PathString.Empty;
            return false;
        }
    }
}