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
using System.Linq;
using System.IO;

namespace dotDB
{
    /// <summary>
    /// Class representing a database table
    /// </summary>
    public class Table{
        
        readonly string tableName;
        private Dictionary<string, type> tableStructure = new Dictionary<string, type>();
        private Dictionary<string, Dictionary<string, Cell>> tableData = new Dictionary<string, Dictionary<string, Cell>>();

        private bool initialized = false;

        private bool useCustomID;
        private int incrementedID = 0;

        /// <summary>
        /// Type of data
        /// </summary>
        public enum type
        {
            Int, Float, String, Bool, Null
        }
        
        /// <summary>
        /// Comparators for the research
        /// </summary>
        public enum comparator{
            Sup_Equal, Inf_Equal, Equal, Sup, Inf, Not
        }

        public Table(string Name, Dictionary<string, type> structure, bool useCustomID_ = false){
            tableName = Name;
            tableStructure = structure;
            useCustomID = useCustomID_;

            initialized = true;
        }

        public void add_data(Dictionary<string, string> data, string customID = null){

            if (initialized){
                string lineNum = useCustomID ? customID: incrementedID.ToString();
                
                Dictionary<string, Cell> newLine = new Dictionary<string, Cell>();

                foreach (var key in tableStructure.Keys){
                    newLine.Add(key, new Cell(tableStructure[key], "N/A"));
                }
                
                foreach (var key in data.Keys){
                    if (tableStructure.ContainsKey(key))
                    {
                        Cell newCase = new Cell(tableStructure[key], data[key]);
                        newLine[key] = newCase;
                    }
                    else
                    {
                        throw exceptionThrower.exception_NoKeyInStructure(tableName, key);
                    }
                }
                
                tableData.Add(lineNum, newLine);
                incrementedID++;
            }else{
                throw exceptionThrower.exception_TableNotInitilized(tableName);
            }
        }

        public void update_data(string id,string key, string data){
            if (initialized){
                if (tableStructure.ContainsKey(key))
                {
                    tableData[id][key].editData(data);
                }
                else
                {
                    throw exceptionThrower.exception_NoKeyInStructure(tableName, key);
                }
            }
            else
            {
                throw exceptionThrower.exception_TableNotInitilized(tableName);
            }
        }

        public Dictionary<string, Dictionary<string, Cell>> researchByComparators(string[] keys, string[] values, comparator[] comparators){
            Dictionary<string, Dictionary<string, Cell>> results = new Dictionary<string, Dictionary<string, Cell>>();

            for (int i = 0; i < keys.Length; i++)
            {
                if (!tableStructure.ContainsKey(keys[i]))
                {
                    throw exceptionThrower.exception_NoKeyInStructure(tableName, keys[i]);
                }

                Table.type dataType = tableStructure[keys[i]];

                if (dataType == Table.type.Int)
                {
                    int value;
                    if (!Int32.TryParse(values[i], out value))
                    {
                        throw exceptionThrower.exception_valTypeNotCorresponding(dataType.ToString(), values[i]);
                    }
                }
                else if (dataType == Table.type.Float)
                {
                    float value;
                    if (!float.TryParse(values[i], out value))
                    {
                        throw exceptionThrower.exception_valTypeNotCorresponding(dataType.ToString(), values[i]);
                    }
                }
                else if (dataType == Table.type.Bool)
                {
                    bool value;
                    if (!Boolean.TryParse(values[i], out value))
                    {
                        throw exceptionThrower.exception_valTypeNotCorresponding(dataType.ToString(), values[i]);
                    }
                }
            }

            foreach (var id in tableData.Keys){
                bool corresponding = false;
                
                Dictionary<string, Cell> correspondingValues = new Dictionary<string, Cell>();
               
                for (int argIndex = 0; argIndex < keys.Length; argIndex++){
                    type currentArgType = tableData[id][keys[argIndex]].getType();
                    switch(comparators[argIndex]){
                        default:
                            corresponding = tableData[id][keys[argIndex]].getData() == values[argIndex];
                            break;
                        case comparator.Not:
                            corresponding = tableData[id][keys[argIndex]].getData() != values[argIndex];
                            break;
                        case comparator.Sup_Equal:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][keys[argIndex]].getData()) >= Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][keys[argIndex]].getData()) >= float.Parse(values[argIndex]);
                            }
                            break;
                        case comparator.Inf_Equal:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][keys[argIndex]].getData()) <= Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][keys[argIndex]].getData()) <= float.Parse(values[argIndex]);
                            }
                            break;
                        case comparator.Sup:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][keys[argIndex]].getData()) > Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][keys[argIndex]].getData()) > float.Parse(values[argIndex]);
                            }
                            break;
                        case comparator.Inf:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][keys[argIndex]].getData()) < Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][keys[argIndex]].getData()) < float.Parse(values[argIndex]);
                            }
                            break;
                    }
                }

                if(corresponding){
                    foreach (var key in tableData[id].Keys){
                        correspondingValues.Add(key, tableData[id][key]);
                    }

                    results.Add(id, correspondingValues);
                }
            }
            return results;
        }

        public bool isInitialized()
        {
            return initialized;
        }

        public bool hasID(string id)
        {
            return tableData.ContainsKey(id);
        }

        public bool hasKeyInStructure(string key)
        {
            return tableStructure.ContainsKey(key);
        }

        public string getTableName()
        {
            return tableName;
        }

        public void remove_data(string ID){

            if (tableData.ContainsKey(ID))
            {
                tableData.Remove(ID);
            }
            else
            {
                throw exceptionThrower.exception_NoIDInTable(tableName, ID);
            }
        }

        public void showTable(){
            string types_ = "ID";

            foreach (var key in tableStructure.Keys){
                types_ += "| " + key + " : " + tableStructure[key].ToString();
            }

            Console.WriteLine(types_);

            foreach(var key in tableData.Keys){
                string vals = key + " ";

                foreach (var key_ in tableData[key].Keys){
                    vals += "| " + tableData[key][key_].getData() + " ";
                }

                Console.WriteLine(vals);
            }
        }
    }
}