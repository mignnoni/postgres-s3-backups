using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using PostgreSQL.Backup.AWS.S3;
using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var env = new EnvVariables();

            var credentials = new BasicAWSCredentials(env.AWSAccessKeyId, env.AWSSecretAccessKey);

            var region = Amazon.RegionEndpoint.GetBySystemName(env.AWSRegion);

            using var _s3Client = new AmazonS3Client(credentials, region);

            var fileName = $"{DateTime.UtcNow:dd_MM_yyyy'_at_'HH_mm_ss}_backup.tar.gz";

            string filePath = Path.Combine(Path.GetTempPath(), fileName);

            string arguments = $"--dbname={env.DatabaseUrl} --format=tar | gzip > {filePath}";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "sh",
                Arguments = $"-c \"pg_dump {arguments}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                var error = await process.StandardError.ReadToEndAsync();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Backup completed successfully.");
                }
                else
                {
                    Console.WriteLine($"Backup failed: {error}");
                    return;
                }
            }

            using var transferUtility = new TransferUtility(_s3Client);

            string? folder = env.BucketFolder;

            if (!string.IsNullOrWhiteSpace(folder))
            {
                folder = $"{folder.Trim()}/";
            }

            var uploadRequest = new TransferUtilityUploadRequest
            {
                FilePath = filePath,
                Key = folder + fileName,
                BucketName = env.AWSBucketName,
                CannedACL = S3CannedACL.Private
            };

            if (File.Exists(filePath))
            {
                await transferUtility.UploadAsync(uploadRequest);

                File.Delete(filePath);

                Console.WriteLine("File uploaded successfully.");
            }
            else
            {
                Console.WriteLine("Backup file does not exist, upload aborted.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}