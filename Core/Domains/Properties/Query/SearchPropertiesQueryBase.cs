using System.Collections.Generic;

namespace Core.Domains.Properties.Query;

public class SearchPropertiesQueryBase
{
    public List<string> Types { get; init; }
    public string Status { get; init; }

    protected SearchPropertiesQueryBase(List<string> types, string status)
    {
        Types = types;
        Status = status;
    }
}