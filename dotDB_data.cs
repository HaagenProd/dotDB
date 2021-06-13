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
using System.Text;
using System.Threading.Tasks;

namespace dotDB
{
    /// <summary>
    /// Class containing the result of a search in the database.
    /// </summary>
    public class Data
    {
        private string ID;
        private Dictionary<string, Cell> lineData;

        public Data(string id, Dictionary<string, Cell> data)
        {
            ID = id;
            lineData = data;
        }

        public string getID()
        {
            return ID;
        }
        
        public int getInt(string key)
        {
            if (lineData[key].getType() == Table.type.Int)
            {
                return Int32.Parse(lineData[key].getData());
            }
            else
            {
                throw exceptionThrower.exception_valTypeNotCorresponding("INT", lineData[key].getData());
            }
        }

        public float getFloat(string key)
        {
            if (lineData[key].getType() == Table.type.Float)
            {
                return float.Parse(lineData[key].getData());
            }
            else
            {
                throw exceptionThrower.exception_valTypeNotCorresponding("FLOAT", lineData[key].getData());
            }
        }

        public string getString(string key)
        {
            return lineData[key].getData();
        }

        public bool getBool(string key)
        {
            if (lineData[key].getType() == Table.type.Bool)
            {
                return Boolean.Parse(lineData[key].getData());
            }
            else
            {
                throw exceptionThrower.exception_valTypeNotCorresponding("BOOL", lineData[key].getData());
            }
        }
    }
}
