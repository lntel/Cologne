using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace Kryptos
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

        public string diag;
        public Appraise(List<string> hardware)
        {
            processor = hardware[0].Replace("(R)", "").Replace("(TM)", "").Replace(" CPU", "");
            gpu = hardware[1].Replace("(R)", "").Replace("(TM)", "").Replace("NVIDIA ", "");
            motherboard = hardware[2].Replace("(R)", "").Replace("(TM)", "");
            memory = hardware[3] + "GB";
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
