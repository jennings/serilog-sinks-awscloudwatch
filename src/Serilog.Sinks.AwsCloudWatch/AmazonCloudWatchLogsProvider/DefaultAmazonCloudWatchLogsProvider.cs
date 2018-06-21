using Amazon.CloudWatchLogs;

namespace Serilog.Sinks.AwsCloudWatch
{
    /// <summary>
    /// Returns a default instance of AmazonCloudWatchLogsClient.
    /// The client's configuration must be specified via configuration or the user profile.
    /// See the <see href="https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html"/>AWS.NET client documentation</see> for details.
    /// </summary>
    class DefaultAmazonCloudWatchLogsProvider : IAmazonCloudWatchLogsProvider
    {
        public IAmazonCloudWatchLogs GetClient()
        {
            return new AmazonCloudWatchLogsClient();
        }
    }
}