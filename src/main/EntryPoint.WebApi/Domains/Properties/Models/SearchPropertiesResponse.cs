using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EntryPoint.WebApi.Domains.Properties.Models;

[ExcludeFromCodeCoverage]
public class SearchPropertiesResponse(IReadOnlyList<SearchPropertiesInnerResponse> content)
{
    public IReadOnlyList<SearchPropertiesInnerResponse> Content { get; init; } = content;

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{nameof(Content)}: {Content}";
    }
}