# dotDB
dotDB is a .NET Database Service WIP project.
I'm working on this project on my free time, dotDB is not ready for use yet. 
> I don't know if what I'm doing is done efficiently, but I started this project to improve myself, and maybe create something that can be used in other projects!

## The purpose of the project
The goal is to create a database system usable in C#, either through "queries" or APIs.

## Working exemples
### 'Request' style (WIP)
```C#
using dotDB;

Database db = new Database("MyDatabase");

db.request("CREATE TABLE MyTable /INT,Something;STRING,Other"); //Request to create a new table 'MyTable'

db.request("INSERT MyTable /Something,1025;Other,Hello"); //Request to Insert data into 'MyTable' table

db.request("SHOW MyTable"); //Request to show in the Console 'MyTable' table

db.request("UPDATE MyTable 0 /Something,10"); //Request to Update the 'Something' value in 'MyTable' table
```
### 'API' style (WIP)
```C#
Database db = new Database("MyDatabase");

Dictionary<string, Table.type> tableStructure = new Dictionary<string, Table.type>() { //Create tableStructure
    {"Something", Table.type.Int },
    {"Other", Table.type.String }
};

db.add_table("MyTable", tableStructure); //Initialize the new Table

db.remove_table("MyTable"); //Remove the new Table
```

## Support me !
<a href="https://www.buymeacoffee.com/Doomiprane" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-blue.png" width="20%" height="20%"></a>
