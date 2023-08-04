using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Commons;

public class Enumeration<T> where T : Enumeration<T>
{
    public int Id { get; }
    public string Name { get; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public static IEnumerable<T> GetAll()
    {
        FieldInfo[] fieldInfos = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fieldInfos
            .Select(fieldInfo => fieldInfo.GetValue(null))
            .Cast<T>();
    }

    public static Optional<T> GetByName(string name)
    {
        FieldInfo[] fieldInfos = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return Optional<T>.Of(fieldInfos
            .Select(fieldInfo => fieldInfo.GetValue(null))
            .Cast<T>()
            .FirstOrDefault(it => it.Name.Equals(name))
        );
    }

    public bool Is(T enumeration)
    {
        return Name.Equals(enumeration.Name);
    }

    public bool IsNot(T enumeration)
    {
        return !Is(enumeration);
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
    }
}