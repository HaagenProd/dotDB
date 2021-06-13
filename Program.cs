using System;
using System.Collections.Generic;
using dotDB;

class Program
{
    static void Main(string[] args)
    {
        Database db = new Database("MyDataBase");

        if (db.edit("CREATE TABLE MyTable /INT,Prix;STRING,Nom"))
        {
            db.edit("SHOW MyTable");

            db.edit("INSERT MyTable /Prix,150;Nom,Un écran");
            db.edit("INSERT MyTable /Prix,10;Nom,Un Bonbon");

            db.edit("SHOW MyTable");

            db.edit("UPDATE MyTable 0 /Prix,1000;Nom,Un Super Ecran");

            var result = db.find("FINDAND MyTable,*,Prix;==;1000");

            Console.WriteLine(result.Count);

            db.edit("SHOW MyTable");
        }

        /*Dictionary<string, Table.type> tableStructure = new Dictionary<string, Table.type>() { //Create tableStructure
            {"Something", Table.type.Int },
            {"Other", Table.type.String }
        };

        db.add_table("MyTable", tableStructure); //Initialize the new Table

        db.add_data("MyTable", new Dictionary<string, string>() //Add data to 'MyTable' table.
        {
            {"Something", "100"},
            {"Other", "I love it" }
        });

        db.add_data("MyTable", new Dictionary<string, string>()
        {
            {"Something", "6789" },
            {"Other", "This is working" }
        });

        db.update_data("MyTable", "1", new Dictionary<string, string>()
        {
            {"Other", "And it's still working" }
        });

        db.remove_table("MyTable"); //Remove the new Table*/
    }
}