using System.Collections.Generic;
using System.Linq;
using JinnDev.Order.Data.Core;
using JinnDev.Order.Data.Entities;

namespace JinnDev.Order.Data
{
    public class ProductRepo : IProductRepo
    {
        public ProductRepo(string connString) { }

        public List<ProductEntity> GetProductsByIDs(List<int> ids) 
            => ids.Select(x => new ProductEntity { Id = x }).ToList();
    }
}