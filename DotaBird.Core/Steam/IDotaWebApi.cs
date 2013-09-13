using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotaBird.Core.Steam
{
    public interface IDotaWebApi
    {
        MatchHistory GetMatchHistory(MatchHistoryRequest request);
        MatchHistory GetMatchHistory();
    }
}
