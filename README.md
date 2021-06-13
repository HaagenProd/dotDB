# dotDB
dotDB is a .NET Database Service WIP project.
I'm working on this project on my free time, dotDB is not ready for use yet. 

> I don't know if what I'm doing is done efficiently, but I started this project to improve myself, and maybe create something that can be used in other projects!

## The purpose of the project
The goal is to create a database system usable in C#, either through "queries" or APIs.
The intended use case of dotDB is mainly for local databases.
Maybe later, it will be possible to use dotDB for databases in the cloud.

> For the moment, no other functionalities other than the basic ones of a database are planned. 

## What can be done?
- [x] Creating Databases
- [x] Adding/Removing Tables to the Database
- [x] Adding/Removing Data in the tables
- [x] Editing existing data in a table
- [ ] Finding specific data and get it to use it in a program 
- [ ] Loading/Saving databases

## Types of data that the database can contain
### Int32 
```C# 
  dotDB.Table.type.Int
```
### Float
```C# 
  dotDB.Table.type.Float
```
### String
```C# 
  dotDB.Table.type.String
```
### Boolean
```C# 
  dotDB.Table.type.Bool
```

> Storing custom Objects is a little tricky

## Working exemples

>Examples are subject to change with updates 

### 'Request' style (WIP)
```C#
using dotDB;

Database db = new Database("MyDatabase");

db.edit("CREATE TABLE MyTable /INT,Something;STRING,Other"); //Request to create a new table 'MyTable'

db.edit("INSERT MyTable /Something,1025;Other,Hello"); //Request to Insert data into 'MyTable' table

db.edit("SHOW MyTable"); //Request to show in the Console 'MyTable' table

db.edit("UPDATE MyTable 0 /Something,10"); //Request to Update the 'Something' value in 'MyTable' table
```
### 'API' style (WIP)
```C#
Database db = new Database("MyDatabase");

Dictionary<string, Table.type> tableStructure = new Dictionary<string, Table.type>() { //Create tableStructure
    {"Something", Table.type.Int },
    {"Other", Table.type.String }
};

db.add_table("MyTable", tableStructure); //Initialize the new Table

db.add_data("MyTable", new Dictionary<string, string>() //Add data to 'MyTable' table.
{
    {"Something", "100"},
    {"Other", "I love it" }
});

db.remove_table("MyTable"); //Remove 'MyTable' table from 'MyDatabase' database.
```

## Support me !
<a href="https://www.buymeacoffee.com/Doomiprane" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-blue.png" width="20%" height="20%"></a>
