using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace TransactionsManager
{
    public class Program
    {
        const int maxSize = 1000000; // Max available size of transactions
        const double reward = 12.5; // Reward 

        public static void Main(string[] args)
        {
            List<Calculation> calculations = new List<Calculation>();
            Transaction[] transactions = Transaction.GetTransactions();
            int numberOfTransactions = transactions.Length; 
            int maxTransactionsThatCanFitIn = 0;
            long tempSize = 0; 
            int max = 0;
            int min = 0;
            double maxFee = 0;
            Calculation finalCalculation = null;

            /* Optimization
             * Calculate the max number of transactions that can fit in
             * This is done by fitting in the transactions with least space in sequence
             * The number of calculations to perform will reduce by some factor
             */
            foreach (Transaction t in transactions)
            {
                if (tempSize + t.Size <= maxSize)
                {
                    tempSize += t.Size;
                    maxTransactionsThatCanFitIn += 1;
                }
            }

            max = (int)Math.Pow(2, numberOfTransactions) - 1;
            min = (int)Math.Pow(2, maxTransactionsThatCanFitIn);

            /* Multiply and calculate the total fee for different combinations of transactions */
            for (int i = max; i >= min; i--)
            {
                BitArray bitArray = new BitArray(BitConverter.GetBytes(i));
                bitArray.Length = 12;
                int[] bits = bitArray.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();
                Calculation calc = new Calculation();
                calc.Multiplier = i;

                for (int j = 0; j < numberOfTransactions; j++)
                {
                    calc.TotalSize = calc.TotalSize + bits[j] * transactions[j].Size;
                    calc.TotalFee = calc.TotalFee + bits[j] * transactions[j].Fee;
                }
                
                calculations.Add(calc);
            }

            // Sort the calculations by fee
            calculations.Sort();

            // Find the calculation with max fee and size less than the max size
            foreach (Calculation calc in calculations)
            {
                if (calc.TotalSize <= maxSize)
                {
                    maxFee = reward + calc.TotalFee;
                    finalCalculation = calc;
                    break;
                }
            }

            Console.WriteLine(string.Format("The max fee that can be gained is: {0} BTC \n", maxFee));
            Console.WriteLine("The transactions to include are: ");

            BitArray multiplierArray = new BitArray(BitConverter.GetBytes(finalCalculation.Multiplier));
            multiplierArray.Length = 12;

            for(int count = 0; count < transactions.Length; count++)
            {
                if(multiplierArray[count])
                {
                    Console.WriteLine(string.Format("{0} {1} {2}", transactions[count].Id, transactions[count].Size, transactions[count].Fee));
                }
            }

            Console.ReadKey();
        }
    }
}