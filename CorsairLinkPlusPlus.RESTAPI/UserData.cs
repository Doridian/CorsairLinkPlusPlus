using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.RESTAPI
{
    internal struct UserData
    {
        internal string name;
        internal string password;
        internal bool readOnly;

        internal UserData(string name, string password, bool readOnly)
        {
            this.name = name;
            this.password = password;
            this.readOnly = readOnly;
        }
    }
}
