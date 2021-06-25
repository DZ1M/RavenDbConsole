using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDbConsole.Raven
{
    public static class DocumentStoreHolder
    {
        // Simular um Singleton

        private static readonly Lazy<IDocumentStore> LazyStore = new Lazy<IDocumentStore>(() =>
        {
            IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080/" },
                Database = "shop"
            };

            store.Initialize();
            return store;
        });
        public static IDocumentStore Store => LazyStore.Value;
    }
}
