using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseloadManager.Entities
{
    public class DataContext
    {
        public CaseloadManager.Entities.Context GetContext()
        {
            return new Entities.Context();
        }

    }
}
