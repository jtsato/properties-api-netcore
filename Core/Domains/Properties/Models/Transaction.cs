﻿using Core.Commons;

namespace Core.Domains.Properties.Models;

public sealed class Transaction : Enumeration<Transaction>
{
    public static readonly Transaction All = new Transaction(0, "ALL");
    public static readonly Transaction Sale = new Transaction(1, "SALE");
    public static readonly Transaction Rent = new Transaction(2, "RENT");

    private Transaction(int id, string name) : base(id, name)
    {
    }
}