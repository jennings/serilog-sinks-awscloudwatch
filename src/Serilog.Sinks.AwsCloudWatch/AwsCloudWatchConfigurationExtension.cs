using System;
using Amazon.CloudWatchLogs;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.AwsCloudWatch
{
    public static class AwsCloudWatchConfigurationExtension
    {
        /// <summary>
        /// Activates logging to AWS CloudWatch
        /// </summary>
        /// <param name="loggerConfiguration">The LoggerSinkConfiguration to register this sink with.</param>
        /// <param name="options">Options to be used for the CloudWatch sink. <see cref="CloudWatchSinkOptions"/> for details.</param>
        /// <param name="cloudWatchClient">An AWS CloudWatch client which includes access to AWS and AWS specific settings like the AWS region.</param>
        /// <returns></returns>
        public static LoggerConfiguration AmazonCloudWatch(this LoggerSinkConfiguration loggerConfiguration, CloudWatchSinkOptions options, IAmazonCloudWatchLogs cloudWatchClient)
        {
            // validating input parameters
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrEmpty(options.LogGroupName)) throw new ArgumentException("options.LogGroupName");
            if (cloudWatchClient == null) throw new ArgumentNullException(nameof(cloudWatchClient));

            // create the sink
            ILogEventSink sink = new CloudWatchLogSink(cloudWatchClient, options);

            // register the sink
            return loggerConfiguration.Sink(sink, options.MinimumLogEventLevel);
        }

        /// <summary>
        /// Activates logging to AWS CloudWatch
        /// </summary>
        /// <remarks>This overload is intended to be used via AppSettings integration.</remarks>
        /// <param name="loggerConfiguration">The LoggerSinkConfiguration to register this sink with.</param>
        /// <param name="logGroupName">The log group name to be used in AWS CloudWatch.</param>
        /// <param name="logStreamNameProvider">The log stream name provider.</param>
        /// <param name="textFormatter">The text formatter to use to format log messages.</param>
        /// <param name="amazonClientProvider">The provider used to create the AmazonCloudWatchLogs client used by this sink.</param>
        /// <param name="minimumLogEventLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="batchSizeLimit">The batch size to be used when uploading logs to AWS CloudWatch.</param>
        /// <param name="period">The period to be used when a batch upload should be triggered.</param>
        /// <param name="retryAttempts">The number of attempts to send log data to AWS CloudWatch.</param>
        /// <param name="createLogGroup">If true, this sink will create the log group if it does not exist.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="logGroupName"/> is <see langword="null"/>.</exception>
        public static LoggerConfiguration AmazonCloudWatch(
            this LoggerSinkConfiguration loggerConfiguration,
            string logGroupName,
            ILogStreamNameProvider logStreamNameProvider = null,
            ITextFormatter textFormatter = null,
            IAmazonCloudWatchLogsProvider amazonClientProvider = null,
            LogEventLevel minimumLogEventLevel = CloudWatchSinkOptions.DefaultMinimumLogEventLevel,
            int batchSizeLimit = CloudWatchSinkOptions.DefaultBatchSizeLimit,
            TimeSpan? period = null,
            byte retryAttempts = CloudWatchSinkOptions.DefaultRetryAttempts,
            bool createLogGroup = CloudWatchSinkOptions.DefaultCreateLogGroup)
        {
            if (logGroupName == null) throw new ArgumentNullException(nameof(logGroupName));

            var options = new CloudWatchSinkOptions
            {
                LogGroupName = logGroupName,
                MinimumLogEventLevel = minimumLogEventLevel,
                BatchSizeLimit = batchSizeLimit,
                Period = period ?? CloudWatchSinkOptions.DefaultPeriod,
                LogStreamNameProvider = logStreamNameProvider ?? new DefaultLogStreamProvider(),
                RetryAttempts = retryAttempts,
                TextFormatter = textFormatter,
                CreateLogGroup = createLogGroup
            };

            var client = (amazonClientProvider ?? new DefaultAmazonCloudWatchLogsProvider()).GetClient();

            return loggerConfiguration.AmazonCloudWatch(options, client);
        }

        /// <summary>
        /// Activates logging to AWS CloudWatch
        /// </summary>
        /// <remarks>This overload is intended to be used via AppSettings integration.</remarks>
        /// <param name="loggerConfiguration">The LoggerSinkConfiguration to register this sink with.</param>
        /// <param name="logGroupName">The log group name to be used in AWS CloudWatch.</param>
        /// <param name="logStreamName">The log stream name to use.</param>
        /// <param name="textFormatter">The text formatter to use to format log messages.</param>
        /// <param name="amazonClientProvider">The provider used to create the AmazonCloudWatchLogs client used by this sink.</param>
        /// <param name="minimumLogEventLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="batchSizeLimit">The batch size to be used when uploading logs to AWS CloudWatch.</param>
        /// <param name="period">The period to be used when a batch upload should be triggered.</param>
        /// <param name="retryAttempts">The number of attempts to send log data to AWS CloudWatch.</param>
        /// <param name="createLogGroup">If true, this sink will create the log group if it does not exist.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="logGroupName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="logStreamName"/> is <see langword="null"/>.</exception>
        public static LoggerConfiguration AmazonCloudWatch(
            this LoggerSinkConfiguration loggerConfiguration,
            string logGroupName,
            string logStreamName,
            ITextFormatter textFormatter = null,
            IAmazonCloudWatchLogsProvider amazonClientProvider = null,
            LogEventLevel minimumLogEventLevel = CloudWatchSinkOptions.DefaultMinimumLogEventLevel,
            int batchSizeLimit = CloudWatchSinkOptions.DefaultBatchSizeLimit,
            TimeSpan? period = null,
            byte retryAttempts = CloudWatchSinkOptions.DefaultRetryAttempts,
            bool createLogGroup = CloudWatchSinkOptions.DefaultCreateLogGroup)
        {
            if (logGroupName == null) throw new ArgumentNullException(nameof(logGroupName));
            if (logStreamName == null) { throw new ArgumentNullException(nameof(logStreamName)); }

            var options = new CloudWatchSinkOptions
            {
                LogGroupName = logGroupName,
                MinimumLogEventLevel = minimumLogEventLevel,
                BatchSizeLimit = batchSizeLimit,
                Period = period ?? CloudWatchSinkOptions.DefaultPeriod,
                LogStreamNameProvider = new ConstantLogStreamNameProvider(logStreamName),
                RetryAttempts = retryAttempts,
                TextFormatter = textFormatter,
                CreateLogGroup = createLogGroup
            };

            var client = (amazonClientProvider ?? new DefaultAmazonCloudWatchLogsProvider()).GetClient();

            return loggerConfiguration.AmazonCloudWatch(options, client);
        }
    }
}
