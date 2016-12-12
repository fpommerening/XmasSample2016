using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FP.XmasSample2016.Edimax
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                var repo = new EdiPlugRepo(new Uri(@"http://10.30.20.187:10000/smartplug.cgi"));
                //var state = repo.GetPowerState().Result;
                //Console.WriteLine(state);

                // Ausschalten
                repo.SetPowerState(false).Wait();
                // Einschalten
                //repo.SetPowerState(true).Wait();

                //for (int i = 0; i < 100; i++)
                //{
                //    var power = repo.NowPower().Result;
                //    var elementVolt = power.Descendants().First(x => x.Name.LocalName == "Device.System.Power.NowPower");
                //    var elementAmp = power.Descendants().First(x => x.Name.LocalName == "Device.System.Power.NowCurrent");
                //    Console.WriteLine($"{DateTime.Now:T}: {elementVolt.Value} V / {elementAmp.Value} A");
                //    System.Threading.Thread.Sleep(250);
                //}



                //var systemInfo = repo.SystemInfo().Result;
                //Console.WriteLine(systemInfo);


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.ReadLine();




        }
    }
}
