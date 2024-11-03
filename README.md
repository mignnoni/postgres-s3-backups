# Backup Application for PostgreSQL to AWS S3

This project is a .NET application that generates backups of a PostgreSQL database and automatically uploads the backup files to an AWS S3 bucket. This solution allows you to keep your data secure and accessible in the cloud.

## Environment Variables

To ensure the application runs correctly, you will need to set the following environment variables in your Docker container:

- `AWS_ACCESS_KEY_ID`: Your AWS access key.

- `AWS_SECRET_ACCESS_KEY`: Your AWS secret key.

- `AWS_BUCKET_NAME`: The name of the S3 bucket where the backups will be stored.

- `AWS_REGION`: The AWS region where your S3 bucket is located. Example: `us-east-1`.

- `BUCKET_FOLDER`: The connection string for your PostgreSQL database. Example:

- `DATABASE_URL`: The connection string for your PostgreSQL database.

- `BUCKET_FOLDER`: (Optional) The folder path within the S3 bucket where backup files will be stored. Leave this empty if you want to store backups at the root of the bucket.