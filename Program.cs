using System;
using System.Collections.Generic;

namespace dotDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table("test", new Dictionary<string, Table.type>() 
            {
                {"val0", Table.type.Int},
                {"val1", Table.type.String},
                {"val2", Table.type.Bool}
            });

            table.add_data(new Dictionary<string, string>() {
                {"val0", 72.ToString()},
                {"val1", "bonjour bonjour"},
                {"val2", true.ToString()}
            });

            table.add_data(new Dictionary<string, string>() {
                {"val0", 103.ToString()},
                {"val1", "c'est mon system de base de données"},
                {"val2", true.ToString()}
            });

            table.add_data(new Dictionary<string, string>() {
                {"val0", 51.ToString()},
                {"val1", "je suis content"},
                {"val2", false.ToString()}
            });

            table.showTable();

            foreach (var result in table.researchByComparators("val0,val1", "52,bonjour bonjour", new Table.comparator[]{Table.comparator.Sup, Table.comparator.Equal}).Keys){
                Console.WriteLine(result);
            }

            table.update_data("2","val1", "FROMAGE");

            table.showTable();
        }
    }
}