using CrudWithDotnet.Models;
using System.Collections.Generic;
using System.Linq;

namespace CrudWithDotnet.Services
{
    public class ProductService
    {

        private List<Product> _products = new List<Product>
        {
            new Product{Id = 1,Price = 49.90M, ProductName = "Rice cooker"},
            new Product{Id = 2, Price = 2999, ProductName="Samsung Galaxy S20 Ultra 512Gb"},
            new Product{Id = 3, Price = 25, ProductName="Pokemon booster pack"},
            new Product{Id = 4, Price= 999.99M,ProductName="Map of the world"}
        };

        public IEnumerable<Product> GetAll() => _products;

        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void AddProduct(Product product)
        {
            var newId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            product.Id = newId;
            _products.Add(product);
        }

        public void deleteProduct(int id)
        {
            var product = GetById(id);
            if(product != null)
            {
                _products.Remove(product);
            }
        }

        public void updateProduct(Product product)
        {
            var existingProduct = GetById(product.Id);
            if(existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Price = product.Price;
            }
        }
    }
}