using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionsManager
{
    public class Calculation : IComparable<Calculation>
    {
        public int Multiplier;
        public long TotalSize;
        public double TotalFee;

        public int CompareTo(Calculation calc)
        {
            if (calc != null)
                return calc.TotalFee.CompareTo(TotalFee);

            return 1;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Calculation t = (Calculation)obj;
            return (Multiplier == t.Multiplier);
        }

        public override int GetHashCode()
        {
            return Multiplier;
        }
    }
}
