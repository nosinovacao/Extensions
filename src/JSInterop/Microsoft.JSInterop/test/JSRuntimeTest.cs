// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.JSInterop.Tests
{
    public class JSRuntimeTest
    {
        [Fact]
        public async Task CanHaveDistinctJSRuntimeInstancesInEachAsyncContext()
        {
            var tasks = Enumerable.Range(0, 20).Select(async _ =>
            {
                var jsRuntime = new FakeJSRuntime();
                JSRuntime.SetCurrentJSRuntime(jsRuntime);
                await Task.Delay(50).ConfigureAwait(false);
                Assert.Same(jsRuntime, JSRuntime.Current);
            });

            await Task.WhenAll(tasks);
            Assert.Null(JSRuntime.Current);
        }

        private class FakeJSRuntime : IJSRuntime
        {
            public Task<T> InvokeAsync<T>(string identifier, params object[] args)
                => throw new NotImplementedException();

            public Task<TValue> InvokeAsync<TValue>(string identifier, IEnumerable<object> args, CancellationToken cancellationToken = default) =>
                throw new NotImplementedException();
        }
    }
}
