using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataBase
{
    public interface IData
    {
        List<Int32> GetMentionDates(DateTime date);
    }
}
