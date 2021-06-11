using System;
using System.Collections.Generic;

namespace dotDB
{
	public class Database
	{
		private string db_Name;
		private Dictionary<string, Table> db_tables = new Dictionary<string, Table>();

		public Database(string Name)
		{
			db_Name = Name;
		}

		public bool request(string request)
        {
			string[] split = request.Split(" ");

            switch (split[0])
            {
				default:
					throw new ArgumentException("Current request is unknown");
				case "CREATE":
                    switch (split[1])
                    {
						default:
							throw new ArgumentException("Current type is unknown");
						case "TABLE": //CREATE TABLE *table* *type*,*name* ... 
							string tableName = split[2];
							List<string> structure = new List<string>();
							Dictionary<string, Table.type> newStructure = new Dictionary<string, Table.type>();

							for (int i = 3; i < split.Length; i++)
                            {
								structure.Add(split[i]);
                            }

							foreach(var composant in structure)
                            {
								string[] composantSplit = composant.Split(","); //1 is key / 0 is type
								newStructure.Add(composantSplit[1], interpretType(composantSplit[0]));
                            }

							db_tables.Add(tableName, new Table(tableName, newStructure));

							return true;
                    }

				case "SHOW":

                    switch (split[1])
                    {
						case "*":
							string result = "";

							foreach(var table in db_tables.Keys)
                            {
								result += table + " ";
                            }

							Console.WriteLine(result);
							return true;

						default:
							if (db_tables.ContainsKey(split[1]))
							{
								db_tables[split[1]].showTable();
							}
							else
							{
								throw new ArgumentException(split[1] + " table does not exists in this Database");
							}
							return true;
					}
			}
        }

		public void add_table(string tableName, Dictionary<string, Table.type> tableStructure, bool useCustomID = false)
		{
			Table newTable = new dotDB.Table(tableName, tableStructure, useCustomID);
			db_tables.Add(tableName, newTable);
		}

		public void remove_table(string tableName)
		{
			db_tables.Remove(tableName);
		}

		public Dictionary<string, Dictionary<string, Case>> research(string request)
        {
			string[] splitedRequest = request.Split(",");
			
			string[] fct = splitedRequest[0].Split(" ");

			Table selectedTable = db_tables[fct[1]];
			string[] keysToReturn = splitedRequest[1].Split(";");
			string[] conditions = splitedRequest[2].Split("/");

			List<string> keys = new List<string>();
			List<Table.comparator> comparators = new List<Table.comparator>();
			List<string> values = new List<string>();

			Dictionary<string, Dictionary<string, Case>> result = new Dictionary<string, Dictionary<string, Case>>();

			if (fct[0] == "FINDAND")
            {
				//FINDAND *table*,*key*;...,*key*;*comparator*;*value*/*key*;*comparator*;*value*...
				
				foreach(var condition in conditions)
                {
					string[] split = condition.Split(";");
					
					keys.Add(split[0]);
					comparators.Add(interpretComparator(split[1]));
					values.Add(split[2]);
				}

				Dictionary<string, Dictionary<string, Case>> firstResult = selectedTable.researchByComparators(keys.ToArray(), values.ToArray(), comparators.ToArray());
				Dictionary<string, Dictionary<string, Case>> finalResult = new Dictionary<string, Dictionary<string, Case>>();
				
				if (keysToReturn[0] == "*")
                {
					result = firstResult;
                }
                else
                {
					foreach (var id in firstResult.Keys)
					{
						finalResult.Add(id, new Dictionary<string, Case>());

						foreach (var key in keysToReturn)
						{
							finalResult[id].Add(key, firstResult[id][key]);
						}
					}

					result = finalResult;
				}				
			}

			return result;
        }

		private Table.type interpretType(string type)
        {
			switch (type.ToLower())
			{
				default:
					throw new ArgumentException(type + " Current type is unknown");
				case "int":
					return Table.type.Int;
				case "float":
					return Table.type.Float;
				case "string":
					return Table.type.String;
				case "bool":
					return Table.type.Bool;
				case "null":
					return Table.type.Null;
			}
		}

		private Table.comparator interpretComparator(string comparator)
        {
			switch (comparator)
            {
				default:
					return Table.comparator.Equal;
				case ">=":
					return Table.comparator.Sup_Equal;
				case "<=":
					return Table.comparator.Inf;
				case ">":
					return Table.comparator.Sup;
				case "<":
					return Table.comparator.Inf;
				case "!":
					return Table.comparator.Not;
            }
        }
	}
}