using System;
using System.Diagnostics.CodeAnalysis;
using Infra.MongoDB.Commons.Repository;

namespace IntegrationTest.Infra.MongoDB.Commons;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class DummyEntity : Entity
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public DateTime BirthDate { get; init; }

    public DummyEntity(int id, string name, string surname, string birthDateAsString)
    {
        Id = id;
        Name = name;
        Surname = surname;
        bool isValid = DateTime.TryParse(birthDateAsString, out DateTime dateTime);
        BirthDate = isValid ? dateTime : DateTime.MinValue; 
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((DummyEntity) obj);
    }

    [ExcludeFromCodeCoverage]
    private bool Equals(DummyEntity other)
    {
        return Id == other.Id
               && Name == other.Name
               && Surname == other.Surname
               && BirthDate.Equals(other.BirthDate);
    }

    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Surname, BirthDate);
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Surname)}: {Surname}, {nameof(BirthDate)}: {BirthDate}";
    }
}