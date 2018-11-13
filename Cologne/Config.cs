using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cologne
{
    class Storage
    {
        public string filename { get; set; }
        public string directory { get; set; }
    }

    class Configuration
    {
        private Storage config;
        public List<int> data;
        public Configuration()
        {
            config = new Storage();

            config.filename = "app.conf";
            config.directory = string.Format("C:/Users/{0}/AppData/Local/{1}", Environment.UserName, "Cologne");

            if(!Directory.Exists(config.directory))
            {
                Directory.CreateDirectory(config.directory);
                File.WriteAllText(string.Format("{0}/{1}", config.directory, config.filename), "cpu=0\nmem=0\ngpu=0");
            }
        }

        public bool Update()
        {
            string[] keys = { "cpu", "mem", "gpu" };
            string content = "";

            for(int i = 0; i < 3; i++)
            {
                content += string.Format("{0}={1}\n", keys[i], data[i].ToString());
            }

            File.WriteAllText(string.Format("{0}/{1}", config.directory, config.filename), content);

            return true;
        }

        public string[] Read()
        {
            string[] tmp = new string[3];

            using (FileStream str = File.OpenRead(string.Format("{0}/{1}", config.directory, config.filename)))
            {
                using (StreamReader rd = new StreamReader(str, Encoding.UTF8, true, 128))
                {
                    string line;
                    int i = 0;

                    while((line = rd.ReadLine()) != null)
                    {
                        tmp[i] = line.Split('=')[1];
                        i++;
                    }
                }
            }

            return tmp;
        }
    }
}
