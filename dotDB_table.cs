using System;
using System.Collections.Generic;

namespace dotDB
{
    public class dotDB_table{
        
        readonly string tableName;
        private Dictionary<string, types> tableStructure = new Dictionary<string, types>();
        private Dictionary<string, Dictionary<string, dotDB_case>> tableData = new Dictionary<string, Dictionary<string, dotDB_case>>();

        private bool initialized = false;

        private bool useCustomID;
        private int incrementedID = 0;

        public enum types
        {
            Int, Float, String, Bool, Null
        }

        public dotDB_table(string Name, Dictionary<string, types> structure, bool useCustomID_ = false){
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