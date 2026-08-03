using System.Collections.Generic;
using Core.Commons.Paging;

namespace EntryPoint.WebApi.Domains.Properties.Models;

public class PageableSearchPropertiesResponse(IReadOnlyList<SearchPropertiesInnerResponse> content, Pageable pageable) : Page<SearchPropertiesInnerResponse>(content, pageable);