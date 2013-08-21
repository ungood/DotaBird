using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IWebClient {
    string Get(Uri uri);
}
