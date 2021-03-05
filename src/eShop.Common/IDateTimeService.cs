using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Common
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}
