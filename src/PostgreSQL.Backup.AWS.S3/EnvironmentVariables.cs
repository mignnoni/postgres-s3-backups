namespace PostgreSQL.Backup.AWS.S3;

public class EnvVariables
{
	public EnvVariables()
	{
        AWSAccessKeyId = GetRequiredValueFromEnv("AWS_ACCESS_KEY_ID");
        AWSSecretAccessKey = GetRequiredValueFromEnv("AWS_SECRET_ACCESS_KEY");
        AWSRegion = GetRequiredValueFromEnv("AWS_REGION");
        AWSBucketName = GetRequiredValueFromEnv("AWS_BUCKET_NAME");
        DatabaseUrl = GetRequiredValueFromEnv("DATABASE_URL");
        BucketFolder = Environment.GetEnvironmentVariable("BUCKET_FOLDER");
    }

    public string AWSAccessKeyId { get; }
    public string AWSSecretAccessKey { get; }
    public string AWSRegion { get; }
    public string AWSBucketName { get; }
    public string DatabaseUrl { get; }
    public string? BucketFolder { get; }

    private static string GetRequiredValueFromEnv(string key)
    {
        string? value = Environment.GetEnvironmentVariable(key);

        if (string.IsNullOrEmpty(value))
            throw new Exception($"The following required environment variable was not provided: {key}");

        return value;
    }
}
