using System;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SQLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestQuery().Wait();
        }

        public static async Task TestQuery()
        {
            var connectionStr =
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KasperTest;" +
                "Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
                "TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var conn = new SqlConnection(connectionStr);

            var readAll = @"SELECT * from Person";
            var spesificQuery = @"SELECT FirstName, LastName, Id from Person 
                                WHERE FirstName = @spes";

            var addOne = @"INSERT INTO Person
                            (FirstName, LastName)
                            VALUES(@FirstName, @LastName)";

            var updateOne = @"UPDATE Person
                              SET LastName = @LastName
                              WHERE Id = @Id";
            //var rowsAffected = await conn.ExecuteAsync(addOne, new {FirstName = "AddTest", LastName = "YesSir"});
            //var updateTerje = await conn.ExecuteAsync(updateOne, new {LastName = "Kolderup", Id = 8});
            var persons = await conn.QueryAsync<Person>(readAll);
            persons.ToList().ForEach(x => Console.WriteLine($"Id={x.Id}, {x.FirstName} {x.LastName}"));
            Console.WriteLine();
            var teachers = await conn.QueryAsync<Person>(spesificQuery, new {spes = @"Terje"});
            teachers.ToImmutableList().ForEach(x => Console.WriteLine($"Id={x.Id}, {x.FirstName} {x.LastName}"));
        }
    }
}
