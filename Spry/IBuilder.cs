namespace Spry
{
    public abstract class Builder
    {
        internal abstract string Build();
    }

    public abstract class Buildable
    {
        internal abstract string BuildImpl();
    }
}
