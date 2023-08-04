using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Commons.Paging;

namespace EntryPoint.WebApi.Domains.Properties.Models;

[ExcludeFromCodeCoverage]
public sealed class PageableSearchPropertiesResponse : Page<SearchPropertiesInnerResponse>
{
    public PageableSearchPropertiesResponse(IReadOnlyList<SearchPropertiesInnerResponse> content, Pageable pageable) : base(content, pageable)
    {
    }
}