using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.Model
{
    [Serializable]
    public class WarningAnalysisModel
    {
        public string SelectTypeId;
        public int BuildId;
        public DateTime StartDate;
        public DateTime EndDate;

        public int PageSize;
        public int PageCurrent;
    }
}
