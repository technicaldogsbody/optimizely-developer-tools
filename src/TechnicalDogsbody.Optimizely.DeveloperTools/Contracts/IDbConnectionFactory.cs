using System.Data;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Contracts
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}