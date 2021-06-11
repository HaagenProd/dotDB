using System;
using System.Collections.Generic;
using dotDB;

class Program
{
    static void Main(string[] args)
    {
        Database db = new Database("MyDataBase");        

        if (db.request("CREATE TABLE MaTable INT,Prix STRING,Nom"))
        {
            db.request("SHOW MaTable");
        }
    }
}