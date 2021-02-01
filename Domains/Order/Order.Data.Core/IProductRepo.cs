using JinnDev.Order.Data.Entities;
using System.Collections.Generic;

namespace JinnDev.Order.Data.Core
{
    public interface IProductRepo
    {
        List<ProductEntity> GetProductsByIDs(List<int> ids);
    }
}