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
        private string API = "http://localhost/v1/";

        private string processor;
        private string motherboard;
        private string gpu;
        private string memory;

        public decimal actual;
        private enum Component
        {
            processor,
            memory,
            graphics,
            hardrive
        }

        public string diag;

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

                prices = new decimal[3];

                configs.Add(config);
            }

            int x = 0;

            for(index = 0; index < 3; index++)
            {
                if(configs[index].Data.data[x].Price != 0)
                {
                    prices[index] = configs[index].Data.data[x].Price;
                } else
                {
                    while(configs[index].Data.data[x].Price == 0)
                    {
                        x++;
                    }

                    prices[index] = configs[index].Data.data[x].Price;
                }
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

            return prices.Sum();
        }

        public decimal Estimate()
        {
            string proc_content = null;
            string gpu_content = null;
            string mem_content = null;

            int mem_index = 0;

            string content = null;
            string[] enums = { "processor", "graphics", "memory" };
            foreach(string x in enums)
            {
                string json = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API + x + "/search.php");
                request.ContentType = "application/json";
                request.Method = "POST";

                switch (x)
                {
                    case "processor":
                        json = "{ \"key\": \"" + processor + "\" }";
                        break;
                    case "graphics":
                        json = "{ \"key\": \"" + gpu + "\" }";
                        break;
                    case "memory":
                        json = "{ \"key\": \"" + memory + "\" }";
                        break;
                }

                diag = json;

                ASCIIEncoding encoding = new ASCIIEncoding();

                Byte[] bytes = encoding.GetBytes(json);

                Stream str = request.GetRequestStream();
                str.Write(bytes, 0, bytes.Length);
                str.Close();

                WebResponse response = request.GetResponse();

                str = response.GetResponseStream();
                StreamReader sr = new StreamReader(str);
                content = sr.ReadToEnd();

                switch (x)
                {
                    case "processor":
                        proc_content = content;
                        break;
                    case "graphics":
                        gpu_content = content;
                        break;
                    case "memory":
                        mem_content = content;
                        break;
                }
            }

            Item cpu = JsonConvert.DeserializeObject<Item>(proc_content);
            Item gpu_json = JsonConvert.DeserializeObject<Item>(gpu_content);
            Item mem = JsonConvert.DeserializeObject<Item>(mem_content);

            if(mem.data[0].Price == 0)
            {
                mem_index++;
            }

            decimal total = decimal.Add(decimal.Add(cpu.data[0].Price, gpu_json.data[0].Price), mem.data[mem_index].Price);

            return total;
        }
    }
}
