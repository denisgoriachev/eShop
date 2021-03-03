using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Persistance
{
    public interface IUnique
    {
        Guid Id { get; }
    }
}
