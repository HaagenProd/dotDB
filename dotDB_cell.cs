using System;
using System.Collections.Generic;

namespace dotDB
{
    /// <summary>
    /// Class representing the data of a cell in a table
    /// </summary>
    public class Cell{

        private List<String> nonAssigned = new List<string>() {"n/a", "N/A"};

        private Table.type dataType;
        private string currentData;
        
        private bool noData = false;
        public bool NoData { get => noData;}

        public Cell(Table.type type, string data){
            dataType = type;

            if (!nonAssigned.Contains(data)){
                if (checkData(data.ToLower())){
                    currentData = data;
                }else{
                    currentData = "N/A";
                }
            }else{
                currentData = data;
            }
        }

        public void editData(string newData){
            currentData = newData;

            checkData(newData.ToLower());
        }

        public string getData(){
            return currentData;
        }

        public Table.type getType(){
            return dataType;
        }

        private bool checkData(string newData){
            bool exception_ = false;
            
            try{
                newData = newData.ToLower();

                if (dataType == Table.type.Int){
                    int value;
                    if (!Int32.TryParse(newData, out value)){
                        throw new ArgumentException("Assigned data type : INT; Current data cannot be used as INT.");
                    }
                }else if (dataType == Table.type.Float){
                    float value;
                    if (!float.TryParse(newData, out value)){
                        throw new ArgumentException("Assigned data type : FLOAT; Current data cannot be used as FLOAT");
                    }
                }else if (dataType == Table.type.Bool){
                    bool value;
                    if (!Boolean.TryParse(newData, out value)){
                        throw new ArgumentException("Assigned data type : BOOL; Current data cannot be used as BOOL");
                    }
                }
            }catch (Exception e){
                exception_ = true;
                Console.WriteLine("An Error has occured");
                Console.WriteLine(e);
            }
                
            if (!exception_){
                return true;
                //Console.WriteLine("Data stored successfully");
            }else{
                return false;
            }       
        }
    }
}