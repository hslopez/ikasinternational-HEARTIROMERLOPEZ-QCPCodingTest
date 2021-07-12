using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCPCodeChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter path of the order book (ex.d:\\orderbook.txt):");
            var orderfile = Console.ReadLine();
            Console.WriteLine("Order Book: ");
            Console.WriteLine();

            double[,] orderTable = GetOrderTable(orderfile);
            
            var openingPrize = GetOpeningPrize(orderTable);

            Console.ReadLine();

        }

        //GetOrderTable
        private static double[,] GetOrderTable(string fileOrder)
        {
            List<double[]> orders = new List<double[]>();
            //@"D:\My Files\Personal\Code Challenge\QCPCodeChallenge\QCPCodeChallenge\bin\Debug\Orderbook.txt"
            int x = 0;
            var fileStream = new FileStream(fileOrder, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    var order = line.Split(',').Select(y => double.Parse(y));
                    orders.Add(order.ToArray());
                    x++;
                }
            }

            double[,] orderTable2 = new double[x, 4];

            Console.WriteLine("Buy                              Sell");
            Console.WriteLine("_______________________________________________________");
            Console.WriteLine("Volume           Price          Price           Volume");
            Console.WriteLine("_______________________________________________________");

            int u = 0;
            foreach (var item in orders)
            {
                int t = 0;
                foreach (var i in item)
                {
                    orderTable2[u, t] = i;
                    Console.Write(string.Format("{0}{1}", i, "              "));
                    t++;
                }
                Console.WriteLine();
                Console.WriteLine("_______________________________________________________");
                u++;
            }

            return orderTable2;
        }

        //Calculate the opening prize
        private static double[] GetOpeningPrize(double[,] orderTable)
        {
            if (orderTable == null || orderTable.Length < 1) return new double[] { };

            int row = orderTable.GetLength(0);            
            double[] buyPrize = new double[row];
            double[] sellPrize = new double[row];

            int length = orderTable.Length;
            for (int i = 0; i < row; i++)
            {
                buyPrize[i] = orderTable[i, 1];
                sellPrize[i] = orderTable[i, 2];                
            }
            
            var sellPrices = sellPrize.Where(x => Math.Ceiling(x) < buyPrize.Max());

            double cnt = 0;
            for (int i = 0; i < row; i++)
            {
                foreach (var item in sellPrices)
                {
                    if(orderTable[i,2]==item)
                    {
                        cnt = cnt + orderTable[i, 3];
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Opening Price: ");
            Console.WriteLine();
            Console.WriteLine("Buy                              Sell");
            Console.WriteLine("_______________________________________________________");
            Console.WriteLine("Volume           Price          Price           Volume");
            Console.WriteLine("_______________________________________________________");
            Console.WriteLine(string.Format("{0}              {1}               {0}             {1}", cnt, sellPrices.Max(), cnt, sellPrices.Max()));
            Console.WriteLine("_______________________________________________________");

            return new double[] { cnt, sellPrices.Max() };
        }
    }
}
