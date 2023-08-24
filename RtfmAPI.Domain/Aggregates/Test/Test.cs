using RftmAPI.Domain.Primitives;

namespace RftmAPI.Domain.Aggregates.Test;

public class Test : AggregateRoot
{
    public string Name { get; private set; }

    public Test(string name)
    {
        Name = name;
    }
}