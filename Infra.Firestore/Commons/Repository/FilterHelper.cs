using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace Infra.Firestore.Commons.Repository;

public static class FilterHelper
{
    public static void AddEqualsFilter(
        ICollection<Filter> filters, string fieldName, int value)
    {
        if (value == 0) return;
        filters.Add(Filter.EqualTo(fieldName, value));
    }

    public static void AddEqualsFilter(
        ICollection<Filter> filters, string fieldName, string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        filters.Add(Filter.EqualTo(fieldName, value));
    }
        
    public static void AddLikeFilter(
        ICollection<Filter> filters, string fieldName, string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        filters.Add(Filter.GreaterThanOrEqualTo(fieldName, value));
        filters.Add(Filter.LessThanOrEqualTo(fieldName, value + "\uf8ff"));
    }
        
    public static void AddDateAfterOrEqualFilter(
        ICollection<Filter> filters, string fieldName, DateTime? value)
    {
        if (value == null) return;
        filters.Add(Filter.GreaterThanOrEqualTo(fieldName, value));
    }
        
    public static void AddDateBeforeOrEqualFilter(
        ICollection<Filter> filters, string fieldName, DateTime? value)
    {
        if (value == null) return;
        filters.Add(Filter.LessThanOrEqualTo(fieldName, value));
    }
}