using System;
using System.Configuration;

namespace ConfigAppNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config =  Properties.Settings.Default.MyArrayParametr;
                Console.WriteLine($"configVal: {config}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            Console.ReadKey();
        }
    }
}
