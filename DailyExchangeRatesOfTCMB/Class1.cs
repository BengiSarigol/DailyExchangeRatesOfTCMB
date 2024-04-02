using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DailyExchangeRatesOfTCMB
{
    public class DailyExchangeRatesOfTCMB : CodeActivity
    {
        [RequiredArgument]

        [Category("Input")]
        public InArgument<DateTime> ExchangeRateDate { get; set; }


        [Category("Output")]
        public OutArgument<DataTable> DT_DailyExchangeRates { get; set; }

        private DataTable dtObject;
        private string URL;
        private XmlTextReader xmltxtReader;

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                
                DateTime CurrentDate = ExchangeRateDate.Get(context);
                URL = "https://www.tcmb.gov.tr/kurlar/" + CurrentDate.ToString("yyyyMM") + "/" + CurrentDate.ToString("ddMMyyyy") + ".xml";
                Console.WriteLine("URL created: " + URL);

                DataSet ds = new DataSet();
                xmltxtReader = new XmlTextReader(URL);
                ds.ReadXml(xmltxtReader);

                // "Currency" adlı tabloyu alırken, DataTable'ı ilgili değişkene atama işlemi
                dtObject = ds.Tables["Currency"];
                if (dtObject == null)
                {
                    Console.WriteLine("Error: 'Currency' table not found in the XML data.");
                }
                else
                {
                    Console.WriteLine("Table 'Currency' retrieved successfully.");
                    DT_DailyExchangeRates.Set(context, dtObject);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
    }
}
