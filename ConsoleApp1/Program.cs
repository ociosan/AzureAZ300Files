using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Azure.Documents.Client;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConnectToDBApp
{
    class Program
    {
        public static string connectionstring;
        public static string DatabaseName = "maindb";
        public static string CollectionName = "employee";
        static DocumentClient dc;
        static string endpoint = "yes";
        static string key = "<yes>";
        

        static void Main(string[] args)
        {

            dc = new DocumentClient(new Uri(endpoint), key);

            /*
                InsertOp("Aldo", "Perez");
                InsertOp("Jessica", "Perez");
                InsertOp("Jorge", "Perez");
                InsertOp("Cristina", "Perez");
                InsertOp("Grabriel", "Perez");
                InsertOp("Sol", "Perez");
                InsertOp("Michelle", "Perez");
                InsertOp("Leslie", "Perez");
            */
            QueryOp();

            Console.WriteLine("\n\n");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();

        }

        static void InsertOp(string first, string last)
        {
            EmployeeEntity employeeEntity = new EmployeeEntity()
            {
                FirstName = first,
                LastName = last
            };

            var result = dc.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName),employeeEntity).GetAwaiter().GetResult();
        }


        static void QueryOp()
        {
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            IQueryable<EmployeeEntity> query = dc.CreateDocumentQuery<EmployeeEntity>(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), feedOptions).Where(e => e.LastName == "Perez");

            Console.WriteLine("Names of all cousins:");
            Console.WriteLine("=====================");

            foreach (EmployeeEntity entity in query)
                Console.WriteLine(entity);

        }

    }

    public class EmployeeEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
