using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTiger
{
    public class Recurrence
    {
        #region Constructors

        public Recurrence()
        {
            
        }

        #endregion Constructors

        #region Properties

        public DateTime CalculationBaseDate { get; set; }

        public DateTime CalculationMaxDate { get; set; }

        public Int32 CalculationMaxRemaining { get; set; }

        public DateSpan DateSpan { get; set; }

        #endregion Properties

        #region Methods

        #region Overrides

        public override string ToString()
        {
            return String.Format("{0}-{1} {2}", CalculationBaseDate.ToShortDateString(), CalculationMaxDate.ToShortDateString(), DateSpan);
        }

        #endregion Overrides

        public DateTime[] CalculateRecurrences(DateTime maxDate)
        {
            List<DateTime> recurrences = new List<DateTime>();

            DateTime nextDate = CalculationBaseDate + DateSpan;
            while (nextDate <= maxDate)
            {
                recurrences.Add(nextDate);
                nextDate += DateSpan;
            }

            CalculationBaseDate = nextDate - DateSpan;

            return recurrences.ToArray();
        }
        
        #endregion Methods
    }
}
