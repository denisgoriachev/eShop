using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain
{
    /// <summary>
    /// Defines the base interface for domain events. Every derrived type should be placed inside static class with name "Vn", where n - any positive integer which
    /// represents the version of domain event.
    /// </summary>
    public interface IDomainEvent
    {

    }
}
