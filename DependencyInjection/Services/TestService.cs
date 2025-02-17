
namespace DependencyInjection;

public class TestService : ITestService
{
    private readonly Guid guid;
    public TestService()
    {
        guid = Guid.NewGuid();
    }
    public Guid GetGuid()
    {
        return guid;
    }
}
