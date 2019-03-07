using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ExampleCloudStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net example of processing data.");
            Console.WriteLine("");
            EvalBlobStorage().GetAwaiter().GetResult();

        }
        public static async Task EvalBlobStorage()
        {
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;

            string storageConnectionString = "XXX";
            if(CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                cloudBlobContainer = blobClient.GetContainerReference("quickstartblobs");
                var x = await cloudBlobContainer.CreateIfNotExistsAsync();
                Console.WriteLine("Created Container {0} new = {1}", cloudBlobContainer.Name, x);
                Console.WriteLine("");
                Console.ReadLine();

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };

                await cloudBlobContainer.SetPermissionsAsync(permissions);
                Console.WriteLine("Set permissions Succesful");
                Console.WriteLine("");
                Console.ReadLine();

                string myPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string localFile = "ExampleBlob " + Guid.NewGuid() + ".txt";
                var sourcefile = Path.Combine(myPath, localFile);
                Console.WriteLine("Put some text here:");
                var input = Console.ReadLine();
                File.WriteAllText(sourcefile, input);

                Console.WriteLine("Temp file = {0}", sourcefile);

                Console.WriteLine("Uploading to Blob storage as blob '{0}'", localFile);

                Console.WriteLine();


                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFile);

                await cloudBlockBlob.UploadFromFileAsync(sourcefile);

                Console.WriteLine("Uploaded");



            }
            else
            {
                Console.WriteLine("No access to the account.");
            }
        }
    }
}
