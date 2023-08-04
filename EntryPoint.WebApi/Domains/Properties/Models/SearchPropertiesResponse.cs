using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EntryPoint.WebApi.Domains.Properties.Models;

[ExcludeFromCodeCoverage]
public class SearchPropertiesResponse
{
    public IReadOnlyList<SearchPropertiesInnerResponse> Content { get; init; }

    public SearchPropertiesResponse(IReadOnlyList<SearchPropertiesInnerResponse> content)
    {
        Content = content;
    }

    public override string ToString()
    {
        return $"{nameof(Content)}: {Content}";
    }
}