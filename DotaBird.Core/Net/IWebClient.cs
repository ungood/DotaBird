using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotaBird.Core.Net
{
    public interface IWebClient
    {
        string Get(Uri uri);
    }
}
