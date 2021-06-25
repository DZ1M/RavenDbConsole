using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using RavenDbConsole.Models;
using RavenDbConsole.Raven;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RavenDbConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateProduct(name:"Apples", price: 10.80);
            //CreateProduct(name:"Abacaxi", price: 6);
            //CreateProduct(name:"Café", price: 8.99);

            //GetProduct("products/2-A");

            //GetAllProducts();

            //GetAllProductsPaginated(1, 3);

            //CreateCartLine("richard");

            AddProductToCart("richard", "products/3-A", 5);
        }
         
        // Raven é no relacional mas com caracteristicas de relacional
        // Conexão é baseada em Unit of Work Pattern
        // Ele é por sessão, então somente vai abrir uma conexão no banco no SaveChanges.
        // Todas operações do RavenDB são asincronas, ex: .ToList() é asincrono no Raven

        // RavenDB armazena consultas em cache agressivo, então se eu repetir a consulta, ela ja vai ta cacheada.
        // No caso, se não tiver novos produtos no banco, ele ja identifica e traz os dados em cache da consulta anterior.



        static void CreateProduct(string name, double price)
        {
            Product p = new Product
            {
                Name = name,
                Price = price
            };

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(p);
                session.SaveChanges();
            }
        }

        static void GetProduct(string id)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                Product p = session.Load<Product>(id);
                Console.WriteLine($"Produto: {p.Name}  \t\t Preço: {p.Price}");
            }
        }
        static void GetAllProducts()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                List<Product> all = session.Query<Product>().ToList();
                foreach (var p in all)
                {

                    Console.WriteLine($"Produto: {p.Name}  \t\t Preço: {p.Price}");
                }
            }
        }
        static void GetAllProductsPaginated(int pageNumber, int pageSize)
        {
            // se passar 1 e 3, vou pular o zero
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                List<Product> page = session.Query<Product>()
                    .Statistics(out QueryStatistics stats) // pega o total de results pra não fazer a chamada do count
                    .Skip(skip).Take(take).ToList();

                Console.WriteLine($"Showing results {skip + 1} to {skip + pageSize} of {stats.TotalResults}");
                foreach (var p in page)
                {

                    Console.WriteLine($"Produto: {p.Name}  \t\t Preço: {p.Price}");
                }

                Console.WriteLine($"This was produced in {stats.DurationInMs} ms");
            }
        }


        static void CreateCartLine(string costumer)
        {
            Cart cart = new Cart();
            cart.Costumer = costumer;
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(cart);
                session.SaveChanges();
            }
        }
        static void AddProductToCart(string costumer, string productId, int qnt)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                // GET BY Customer in Cart
                var cart = session.Query<Cart>().Single(x => x.Costumer == costumer);
                // Get by id
                Product p = session.Load<Product>(productId);

                cart.Lines.Add(new CartLine {
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    Quantity = qnt
                });
                
                // Salva as alterações, pois ja busquei a sessin do cart
                // caso eu não mudar nada no cart, o savechanges vai entender q nao foi alterado e nao vai mandar nada pro db.

                session.SaveChanges();
            }
        }
    }
}
