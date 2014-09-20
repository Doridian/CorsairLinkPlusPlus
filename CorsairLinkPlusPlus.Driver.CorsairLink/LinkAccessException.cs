using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.CorsairLink
{
    class LinkAccessException : Exception
    {
        public LinkAccessException(string message) : base(message) { }
    }
}
