using Amazon.CloudWatchLogs;

namespace Serilog.Sinks.AwsCloudWatch
{
    public interface IAmazonCloudWatchLogsProvider
    {
        IAmazonCloudWatchLogs GetClient();
    }
}