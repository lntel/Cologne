using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

namespace Cologne
{
    public class Device
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Item
    {
        public List<Device> data { get; set;  }
    }
    class Appraise
    {
        private string API = "https://fivestack.000webhostapp.com/v1/";

        private string processor;
        private string motherboard;
        private string gpu;
        private string memory;

        public List<Config> diag = new List<Config>();

        public decimal actual;
        private enum Component
        {
            processor,
            memory,
            graphics,
            hardrive
        }

        public Appraise(List<string> hardware)
        {
            processor = hardware[0].Replace("(R)", "").Replace("(TM)", "").Replace(" CPU", "");
            gpu = hardware[1].Replace("(R)", "").Replace("(TM)", "").Replace("NVIDIA ", "");
            motherboard = hardware[2].Replace("(R)", "").Replace("(TM)", "");
            memory = hardware[3] + "GB";
        }

        public class Config
        {
            public string Json { get; set; }
            public string Url { get; set; }
            public string Content { get; set; }
            public Item Data { get; set; }
        }

        public decimal[] Calculate()
        {
            string[] content = new string[3];
            decimal[] prices = { };
            int index = 0;

            List<Config> configs = new List<Config>();

            for(int i = 0; i < 3; i++)
            {
                string json;
                Config config = new Config();

                switch(i)
                {
                    case 0:
                        config.Json = "{ \"key\": \"" + processor + "\" }";

                        config.Url = string.Format("{0}{1}/{2}", API, "processor", "search.php");
                        break;
                    case 1:
                        config.Json = "{ \"key\": \"" + memory + "\" }";

                        config.Url = string.Format("{0}{1}/{2}", API, "memory", "search.php" );
                        break;
                    case 2:
                        config.Json = "{ \"key\": \"" + gpu + "\" }";

                        config.Url = string.Format("{0}{1}/{2}", API, "graphics", "search.php");
                        break;
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(config.Url);
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240";
                request.Method = "POST";
                ASCIIEncoding encoding = new ASCIIEncoding();

                Byte[] bytes = encoding.GetBytes(config.Json);

                Stream str = request.GetRequestStream();
                str.Write(bytes, 0, bytes.Length);
                str.Close();

                WebResponse response = request.GetResponse();

                str = response.GetResponseStream();

                StreamReader sr = new StreamReader(str);

                config.Content = sr.ReadToEnd();
                config.Data = JsonConvert.DeserializeObject<Item>(config.Content);

                prices = new decimal[10];

                configs.Add(config);

                //diag.Add(config);
            }

            int x = 0;

            for(index = 0; index < configs.Count - 1; index++)
            {
                //if(configs.Count < index)
                //{
                    if (configs[index].Data.data[x].Price != 0)
                    {
                        prices[index] = configs[index].Data.data[x].Price;
                    }
                    else
                    {
                        while (configs[index].Data.data[x].Price == 0)
                        {
                            x++;
                        }

                        prices[index] = configs[index].Data.data[x].Price;
                    }
                //}
            }

            actual = prices.Sum();

            return prices;
        }

        public decimal Depreciate()
        {
            int lifespan = 0;
            decimal salvage = 0;

            string[] tmp;

            Configuration config = new Configuration();

            tmp = config.Read();

            decimal[] prices = Calculate();

            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        lifespan = 40;
                        salvage = 90;
                        break;
                    case 1:
                        lifespan = 5;
                        salvage = 60;
                        break;
                    case 2:
                        lifespan = 7;
                        salvage = 30;
                        break;
                }

                decimal yearly = ((prices[i] - salvage) / lifespan);

                prices[i] = (prices[i] - (Convert.ToInt16(tmp[i]) * yearly));
            }

            if(prices.Sum() < 0)
            {
                return 0;
            } else
            {
                return prices.Sum();
            }
        }
    }
}
