using System;
using System.Collections.Generic;

namespace dotDB
{
    class Program
    {
        static void Main(string[] args)
        {
            dotDB_table table = new dotDB_table("test", new Dictionary<string, dotDB_table.types>() 
            {
                {"val0", dotDB_table.types.Int},
                {"val1", dotDB_table.types.String},
                {"val2", dotDB_table.types.Bool}
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
        }
    }
}