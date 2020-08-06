namespace ErrorCentral.UnitTests.Builders
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
