/*
	dotDB is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    dotDB is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dotDB.  If not, see <https://www.gnu.org/licenses/>
*/

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

		public bool edit(string request)
        {
			string[] keysAndValues_ = request.Split("/");
			string[] keysAndValues = keysAndValues_.Length == 2 ? keysAndValues_[1].Split(";") : new string[0];

			string[] split = request.Split(" ");
			
			Table currentTable;
			string tableName;

			switch (split[0])
            {
				default:
					throw new ArgumentException("Current request is unknown");
				case "CREATE":
                    switch (split[1])
                    {
						default:
							throw new ArgumentException("Current type is unknown");
						case "TABLE": //CREATE TABLE *table* /*type*,*name*; ... 
							tableName = split[2];
							List<string> structure = new List<string>();
							Dictionary<string, Table.type> newStructure = new Dictionary<string, Table.type>();
							bool useCustomID = false;

							if (split[3] == "True")
                            {
								useCustomID = true;
							}

							for (int i = 0; i < keysAndValues.Length; i++)
                            {
								structure.Add(keysAndValues[i]);
                            }

							foreach(var composant in structure)
                            {
								string[] composantSplit = composant.Split(","); //1 is key / 0 is type
								newStructure.Add(composantSplit[1], interpretType(composantSplit[0]));
                            }

							add_table(tableName, newStructure, useCustomID);

							return true;
                    }

				case "DELETE": //DELETE *table* (*key* ...)
					if (db_tables.ContainsKey(split[1]))
                    {
						currentTable = db_tables[split[1]];
                    }
                    else
                    {
						throw new ArgumentException(split[1] + " does not exists in " + db_Name + " database");
                    }

					switch (split.Length)
                    {
						default:
							throw new ArgumentException("Not enough argument in request");
						case 3:
							if (currentTable.hasID(split[2]))
                            {
								currentTable.remove_data(split[2]);
								return true;
							}
                            else
                            {
								throw new ArgumentException(split[1] + " has no lign with ID " + split[2]);
                            }
						case 2:
							remove_table(split[1]);
							return true;
                    }

				case "UPDATE": //UPDATE *table* *ID* /*key*;*value* ...
					if (db_tables.ContainsKey(split[1]))
                    {
						currentTable = db_tables[split[1]];

						if (currentTable.hasID(split[2]))
                        {
							List<string> keysToUpdate = new List<string>();

							foreach (var keyToUpdate in keysAndValues)
							{
								string[] KeySplit = keyToUpdate.Split(",");
								if (currentTable.hasKeyInStructure(KeySplit[0]))
                                {
									currentTable.update_data(split[2], KeySplit[0], KeySplit[1]);
								}
                                else
                                {
									throw new ArgumentException(currentTable.getTableName() + " table has no key '" + KeySplit[0] + "' in it's structure");
								}
							}
						}
                        else
                        {
							throw new ArgumentException(currentTable.getTableName() + " has no ID '" + split[2] + "'");
                        }

						return true;
                    }
                    else
                    {
						throw new ArgumentException(split[1] + " does not exists in " + db_Name + " database");
                    }

				case "INSERT": //INSERT *table* /*key*;*val ...
					tableName = split[1];

					if (db_tables.ContainsKey(tableName))
                    {
						currentTable = db_tables[tableName];

						Dictionary<string, string> newData = new Dictionary<string, string>();

						foreach (var arg in keysAndValues)
                        {
							string[] args = arg.Split(",");

							newData.Add(args[0], args[1]);
                        }

						currentTable.add_data(newData);

						return true;
                    }
                    else
                    {
						throw new ArgumentException(split[1] + " does not exists in " + db_Name + " database");
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
			Table newTable = new Table(tableName, tableStructure, useCustomID);
			db_tables.Add(tableName, newTable);
		}

		public void add_data(string table, Dictionary<string, string> data, string customID = null)
        {
			Table currentTable;

			if (db_tables.ContainsKey(table))
            {
				currentTable = db_tables[table];

				currentTable.add_data(data, customID);
            }
            else
            {
				throw new ArgumentException(db_Name + " does not have '" + table + "' table");
            }
        }

		public void update_data(string table, string id, Dictionary<string, string> newData)
        {
			Table currentTable;

			if (db_tables.ContainsKey(table))
            {
				currentTable = db_tables[table];
				
				foreach(var key in newData.Keys)
                {
                    try
                    {
						currentTable.update_data(id, key, newData[key]);
					}catch(Exception e)
                    {
						Console.WriteLine(e);
                    }
                }
			}
            else
            {
				throw new ArgumentException(db_Name + " does not have '" + table + "' table");
			}
        }

		public void remove_table(string tableName)
		{
			db_tables.Remove(tableName);
		}

		public List<Data> find(string table, string[] keysToReturn, string[] keys, Table.comparator comparators, string[] values)
        {
			Table currentTable;

            if (db_tables.ContainsKey(table))
            {
				currentTable = db_tables[table];

				throw new NotImplementedException("WIP");
            }
            else
            {
				throw exceptionThrower.exception_NoCorrespondingTableInDB(db_Name, table);
            }
        }

		public List<Data> find(string request)
        {
			string[] splitedRequest = request.Split(",");
			
			string[] fct = splitedRequest[0].Split(" ");

			Table selectedTable = db_tables[fct[1]];
			string[] keysToReturn = splitedRequest[1].Split(";");
			string[] conditions = splitedRequest[2].Split("/");

			List<string> keys = new List<string>();
			List<Table.comparator> comparators = new List<Table.comparator>();
			List<string> values = new List<string>();

			Dictionary<string, Dictionary<string, Cell>> result = new Dictionary<string, Dictionary<string, Cell>>();

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

				Dictionary<string, Dictionary<string, Cell>> firstResult = selectedTable.researchByComparators(keys.ToArray(), values.ToArray(), comparators.ToArray());
				Dictionary<string, Dictionary<string, Cell>> finalResult = new Dictionary<string, Dictionary<string, Cell>>();
				
				if (keysToReturn[0] == "*")
                {
					result = firstResult;
                }
                else
                {
					foreach (var id in firstResult.Keys)
					{
						finalResult.Add(id, new Dictionary<string, Cell>());

						foreach (var key in keysToReturn)
						{
							finalResult[id].Add(key, firstResult[id][key]);
						}
					}

					result = finalResult;
				}				
			}

			List<Data> results = new List<Data>();

			foreach (var searchResult in result.Keys)
            {
				results.Add(new Data(searchResult, result[searchResult]));
            }

			return results;
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