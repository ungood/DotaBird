using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

public static class UriExtensions
{
    // From: http://stackoverflow.com/a/10836145
    public static Uri AddQuery(this Uri uri, string name, string value)
    {
        var ub = new UriBuilder(uri);

        if (value == "")
        {
            name = null;
        }
        // decodes urlencoded pairs from uri.Query to HttpValueCollection
        var queryString = HttpUtility.ParseQueryString(uri.Query);

        queryString.Add(name, value);

        // urlencodes the whole HttpValueCollection
        ub.Query = queryString.ToString().TrimEnd('&');

        return ub.Uri;
    }

    public static Uri AddQuery(this Uri uri, string name, DateTime? value)
    {
        string stringValue = value.ToString();

        return uri.AddQuery(name, stringValue);
    }

    public static Uri AddQuery(this Uri uri, string name, long? value)
    {
        string stringValue = value.ToString();

        return uri.AddQuery(name, stringValue);
    }
}
