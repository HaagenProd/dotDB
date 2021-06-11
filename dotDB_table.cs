using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace dotDB
{
    public class Table{
        
        readonly string tableName;
        private Dictionary<string, type> tableStructure = new Dictionary<string, type>();
        private Dictionary<string, Dictionary<string, Cell>> tableData = new Dictionary<string, Dictionary<string, Cell>>();

        private bool initialized = false;

        private bool useCustomID;
        private int incrementedID = 0;

        public enum type
        {
            Int, Float, String, Bool, Null
        }
        
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
                    Cell newCase = new Cell(tableStructure[key], data[key]);
                    newLine[key] = newCase;
                }
                
                tableData.Add(lineNum, newLine);
                incrementedID++;
            }else{
                throw new Exception("Current table was not initialized");
            }
        }

        public void update_data(string id,string key, string data){
            if (initialized){
                tableData[id][key].editData(data);
            }
        }

        public Dictionary<string, Dictionary<string, Cell>> researchByComparators(string[] args, string[] values, comparator[] comparators){
            Dictionary<string, Dictionary<string, Cell>> results = new Dictionary<string, Dictionary<string, Cell>>();

            foreach (var id in tableData.Keys){
                bool corresponding = false;
                Dictionary<string, Cell> correspondingValues = new Dictionary<string, Cell>();
                for (int argIndex = 0; argIndex < args.Length; argIndex++){
                    type currentArgType = tableData[id][args[argIndex]].getType();
                    switch(comparators[argIndex]){
                        default:
                            corresponding = tableData[id][args[argIndex]].getData() == values[argIndex];
                            break;
                        case comparator.Not:
                            corresponding = tableData[id][args[argIndex]].getData() != values[argIndex];
                            break;
                        case comparator.Sup_Equal:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args[argIndex]].getData()) >= Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args[argIndex]].getData()) >= float.Parse(values[argIndex]);
                            }
                            break;
                        case comparator.Inf_Equal:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args[argIndex]].getData()) <= Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args[argIndex]].getData()) <= float.Parse(values[argIndex]);
                            }
                            break;
                        case comparator.Sup:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args[argIndex]].getData()) > Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args[argIndex]].getData()) > float.Parse(values[argIndex]);
                            }
                            break;
                        case comparator.Inf:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args[argIndex]].getData()) < Int32.Parse(values[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args[argIndex]].getData()) < float.Parse(values[argIndex]);
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
            tableData.Remove(ID);
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