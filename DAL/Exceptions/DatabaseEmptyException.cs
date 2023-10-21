using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    [Serializable]
    public class DatabaseEmptyException:Exception
    {
        private readonly string message = "database is empty";

        public DatabaseEmptyException()
        {
            
        }
        public DatabaseEmptyException(string message):base(message)
        {
            
        }
    }
}
