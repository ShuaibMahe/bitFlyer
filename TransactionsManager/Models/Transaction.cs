using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TransactionsManager
{
    public class Transaction : IComparable<Transaction>
    {
        public int Id;
        public long Size;
        public double Fee;

        public Transaction(int id, long size, double fee)
        {
            Id = id;
            Size = size;
            Fee = fee;
        }

        public int CompareTo(Transaction tran)
        {
            if (tran != null)
                return tran.Size.CompareTo(Size);
            return 1;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Transaction t = (Transaction)obj;
            return (Id == t.Id);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static Transaction[] GetTransactions()
        {
            List<Transaction> availableTransactions = new List<Transaction>();

            try
            {
                string[] data = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\input.txt");

                for(int count = 1; count < data.Length; count++)
                {
                    string[] entry = data[count].Split(new char[] { '\t' });
                    availableTransactions.Add(new Transaction(int.Parse(entry[0]), long.Parse(entry[1]), double.Parse(entry[2])));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            availableTransactions.Sort();

            return availableTransactions.ToArray();
        }
    }
}
