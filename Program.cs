using System;
using System.Collections.Generic;

namespace dotDB
{
    class Program
    {
        static void Main(string[] args)
        {
            dotDB_table table = new dotDB_table("test", new Dictionary<string, dotDB_table.type>() 
            {
                {"val0", dotDB_table.type.Int},
                {"val1", dotDB_table.type.String},
                {"val2", dotDB_table.type.Bool}
            });

            table.add_data(new Dictionary<string, string>() {
                {"val0", 103.ToString()},
                {"val1", "bonjour bonjour"},
                {"val2", true.ToString()}
            });

            table.add_data(new Dictionary<string, string>() {
                {"val0", 103.ToString()},
                {"val1", "bonjour bonjour"},
                {"val2", true.ToString()}
            });


            table.add_data(new Dictionary<string, string>() {
                {"val0", 51.ToString()},
                {"val1", "je suis content"},
                {"val2", false.ToString()}
            });

            table.showTable();

            foreach (var result in table.search("val0", "10", new dotDB_table.comparator[]{dotDB_table.comparator.Inf}).Keys){
                Console.WriteLine(result);
            }
        }
    }
}