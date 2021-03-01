using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain
{
    public static class Ensure
    {
        public static void IsNotNullOrWhiteSpace(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException($"{propertyName} cannot be empty or whitespace.");
        }
    }
}
