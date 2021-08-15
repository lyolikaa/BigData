using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BigDataEntry
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Getting Data");

            string[] csvlines = File.ReadAllLines("test-task_dataset_summer_products.csv");
            var priceColumn = csvlines[0].Split(',').ToList().IndexOf("price");
            var rating_five_count = csvlines[0].Split(',').ToList().IndexOf("rating_five_count");
            var rating_count = csvlines[0].Split(',').ToList().IndexOf("rating_count");
            var origin_country = csvlines[0].Split(',').ToList().IndexOf("origin_country");

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var query =
            from csvLine in csvlines.ToList().Skip(1)
            let data = CSVParser.Split(csvLine)
            select new
            {
                price = data[priceColumn],
                rating_five_column = data[rating_five_count],
                rating_count = data[rating_count],
                origin_country = data[origin_country]
            };
            var res =
            from row in query
            where !string.IsNullOrWhiteSpace(row.price) && !string.IsNullOrWhiteSpace(row.rating_count) && !string.IsNullOrWhiteSpace(row.rating_five_column) && !string.IsNullOrWhiteSpace(row.rating_count)
            group row by row.origin_country
            into gr
            select new { Average = gr.Average(r => double.Parse(r.price)), FivePercentage = gr.Sum(r => int.Parse(r.rating_five_column)) * gr.Sum(r => int.Parse(r.rating_count)) / 100, gr.Key };
        }
    }
}
