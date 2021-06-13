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
    static class exceptionThrower
    {
        public static ArgumentException exception_NoKeyInStructure(string table, string faultyKey)
        {
            return new ArgumentException(table + " does not have '" + faultyKey + "' in it's structure.");
        }

        public static ArgumentException exception_NoCorrespondingTableInDB(string db, string faultyTable)
        {
            return new ArgumentException(db + " does not have '" + faultyTable + "' table.");
        }

        public static ArgumentException exception_NoIDInTable(string table, string faultyID)
        {
            return new ArgumentException(table + " does not have '" + faultyID + "' ID in table.");
        }

        public static ArgumentException exception_valTypeNotCorresponding(string type, string faultyValue)
        {

            return new ArgumentException("'" + faultyValue + "' cannot be used as '" + type + "'.");
        }

        public static Exception exception_TableNotInitilized(string faultyTable)
        {
            return new Exception("'" + faultyTable + "' is not Initialized.");
        }
    }
}
