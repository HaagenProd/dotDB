﻿using System;
using System.Collections.Generic;
using dotDB;

class Program
{
    static void Main(string[] args)
    {
        Database db = new Database("MyDataBase");        

        if (db.request("CREATE TABLE MaTable /INT,Prix;STRING,Nom"))
        {
            db.request("SHOW MaTable");

            db.request("INSERT MaTable /Prix,150;Nom,Un écran");
            db.request("INSERT MaTable /Prix,10;Nom,Un Bonbon");

            db.request("SHOW MaTable");

            db.request("UPDATE MaTable 0 /Prix,1000;Nom,Un Super Ecran");

            db.request("SHOW MaTable");
        }
    }
}