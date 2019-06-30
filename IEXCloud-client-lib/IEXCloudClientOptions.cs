namespace IEXCloudClient.NetCore
{
    public class IEXCloudClientOptions
    {
        public IEXCloudClientOptions(IEXCloudClientOptionsEnvironment environment, IEXCloudClientOptionsVersion version, string publicToken, string secretToken)
        {
            if (string.IsNullOrWhiteSpace(publicToken))
            {
                throw new System.ArgumentException("you need a token", nameof(publicToken));
            }

            Environment = environment.ToString().ToLower();
            Version = version.ToString().ToLower();
            PublicToken = publicToken;
            SecretToken = secretToken;
        }

        public readonly string Environment;

        public readonly string Version;

        public readonly string PublicToken;
        public readonly string SecretToken;

        public enum IEXCloudClientOptionsEnvironment
        {
            Cloud,
            Sandbox
        }

        public enum IEXCloudClientOptionsVersion
        {
            Beta,
            V1,
            Latest,
            Stable
        }
    }
}