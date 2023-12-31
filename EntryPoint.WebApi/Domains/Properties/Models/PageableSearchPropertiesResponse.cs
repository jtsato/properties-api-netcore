﻿using System.Collections.Generic;
using Core.Commons.Paging;

namespace EntryPoint.WebApi.Domains.Properties.Models;

public class PageableSearchPropertiesResponse : Page<SearchPropertiesInnerResponse>
{
    public PageableSearchPropertiesResponse(IReadOnlyList<SearchPropertiesInnerResponse> content, Pageable pageable) : base(content, pageable)
    {
    }
}