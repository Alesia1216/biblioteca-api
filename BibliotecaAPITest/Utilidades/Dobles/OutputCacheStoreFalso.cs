using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OutputCaching;

namespace BibliotecaAPITest.Utilidades.Dobles
{
    internal class OutputCacheStoreFalso : IOutputCacheStore
    {
        public ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
           return ValueTask.CompletedTask;
        }

        public ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
