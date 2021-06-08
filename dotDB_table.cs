using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace dotDB
{
    public class dotDB_table{
        
        readonly string tableName;
        private Dictionary<string, type> tableStructure = new Dictionary<string, type>();
        private Dictionary<string, Dictionary<string, dotDB_case>> tableData = new Dictionary<string, Dictionary<string, dotDB_case>>();

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

        public dotDB_table(string Name, Dictionary<string, type> structure, bool useCustomID_ = false){
            tableName = Name;
            tableStructure = structure;
            useCustomID = useCustomID_;

            initialized = true;
        }

        public void add_data(Dictionary<string, string> data, string customID = null){

            if (initialized){
                string lineNum = useCustomID ? customID: incrementedID.ToString();
                
                Dictionary<string, dotDB_case> newLine = new Dictionary<string, dotDB_case>();
                bool hasException = false;
                
                foreach (var key in data.Keys){
                    dotDB_case newCase = new dotDB_case(tableStructure[key], data[key]);
                    if (!newCase.hasException){
                        newLine.Add(key, newCase);
                    }else{
                        hasException = true;
                    }
                }
                
                if (!hasException){
                    tableData.Add(lineNum, newLine);
                    incrementedID++;
                }
            }else{
                throw new Exception("Current table was not initialized");
            }
        }

        public Dictionary<string, Dictionary<string, dotDB_case>> researchByComparators(string args, string values, comparator[] comparators){
            string[] args_ = args.Split(",");
            string[] values_ = values.Split(",");

            Dictionary<string, Dictionary<string, dotDB_case>> results = new Dictionary<string, Dictionary<string, dotDB_case>>();

            foreach (var id in tableData.Keys){
                bool corresponding = false;
                Dictionary<string, dotDB_case> correspondingValues = new Dictionary<string, dotDB_case>();
                for (int argIndex = 0; argIndex < args_.Length; argIndex++){
                    type currentArgType = tableData[id][args_[argIndex]].getType();
                    switch(comparators[argIndex]){
                        default:
                            corresponding = tableData[id][args_[argIndex]].getData() == values_[argIndex];
                            break;
                        case comparator.Not:
                            corresponding = tableData[id][args_[argIndex]].getData() != values_[argIndex];
                            break;
                        case comparator.Sup_Equal:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args_[argIndex]].getData()) >= Int32.Parse(values_[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args_[argIndex]].getData()) >= float.Parse(values_[argIndex]);
                            }
                            break;
                        case comparator.Inf_Equal:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args_[argIndex]].getData()) <= Int32.Parse(values_[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args_[argIndex]].getData()) <= float.Parse(values_[argIndex]);
                            }
                            break;
                        case comparator.Sup:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args_[argIndex]].getData()) > Int32.Parse(values_[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args_[argIndex]].getData()) > float.Parse(values_[argIndex]);
                            }
                            break;
                        case comparator.Inf:
                            if (currentArgType == type.Int){
                                corresponding = Int32.Parse(tableData[id][args_[argIndex]].getData()) < Int32.Parse(values_[argIndex]);
                            }else if (currentArgType == type.Float){
                                corresponding = float.Parse(tableData[id][args_[argIndex]].getData()) < float.Parse(values_[argIndex]);
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

        public void remove_data(int index){
            tableData.Remove(index.ToString());
        }

        public void showTable(){
            string types_ = "   ";

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