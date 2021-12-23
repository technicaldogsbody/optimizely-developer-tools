using System.Data;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}