using System;
using System.Collections.Generic;

namespace dotDB
{
    public class dotDB_case{

        private dotDB_table.type dataType;
        private string currentData;
        
        private bool exception_ = false;
        public bool hasException {get=>exception_;}

        public dotDB_case(dotDB_table.type type, string data){
            dataType = type;
            currentData = data;

            try{
                checkData();
            }catch (Exception e){
                exception_ = true;
                Console.WriteLine("An Error has occured");
                Console.WriteLine(e);
            }finally{
                if (!exception_){
                    //Console.WriteLine("Data stored successfully");
                }else{
                    dataType = dotDB_table.type.Null;
                    currentData = "N/A";
                }
            }
        }

        public string getData(){
            return currentData;
        }

        private void checkData(){
            if (dataType == dotDB_table.type.Int){
                int value;
                if (!Int32.TryParse(currentData, out value)){
                    throw new ArgumentException("Assigned data type : INT; Current data cannot be used as INT");
                }
            }else if (dataType == dotDB_table.type.Float){
                float value;
                if (!float.TryParse(currentData, out value)){
                    throw new ArgumentException("Assigned data type : FLOAT; Current data cannot be used as FLOAT");
                }
            }else if (dataType == dotDB_table.type.Bool){
                bool value;
                currentData = currentData.ToLower();
                if (!Boolean.TryParse(currentData, out value)){
                    throw new ArgumentException("Assigned data type : BOOL; Current data cannot be used as BOOL");
                }
            }
        }
    }
}