using System;
using System.Collections.Generic;
using System.Linq;

namespace PRM.Domain.Products.Extensions
{
    public static class ProductErrorMessagesExtensions
    {
        public static string GetProductsWithErrorMessage(this List<Product> productsToRent, Func<Product, bool> errorCondition, string productErrorMessage)
        {
            var productsWithError = productsToRent.Where(errorCondition).ToList();
            var exceptionMessage = productErrorMessage;
                
            foreach (var product in productsWithError)
            {
                exceptionMessage += $" \n {product.Code} - {product.Name} - {product.Description} - {product.Status}";
            }

            return exceptionMessage;
        }
    }
}