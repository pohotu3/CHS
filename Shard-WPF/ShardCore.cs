using System;
using ConnectionData;
using System.IO;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard_WPF
{
    class ShardCore
    {

        private Client client;
        private static ShardCore core;
        private MainWindow mw;
        private Log log;
        private Config cfg;
        public Guid guid;

        public static string commandKey; // the command key that the speach uses to tell if you're talking to it. example: Ok Crystal

        public static string baseDir = "/CrystalHomeSys/", logBaseDir = "Logs/", configDir = "shard_config.cfg";

        public ShardCore(MainWindow mw)
        {
            this.mw = mw;

            baseDir = System.Environment.GetEnvironmentVariable("HOME") + baseDir;
            logBaseDir = baseDir + logBaseDir;
            configDir = baseDir + configDir;

            log = new Log(logBaseDir);

            core = this;
            Write("##################Crystal Shard Dev has been Started##################"); // ##################
            Log("##################Crystal Shard Dev has been Started##################");

            cfg = new Config(configDir);
            LoadConfig();

            Write("Setting up client...");
            Log("Setting up client...");
            client = new Client("127.0.0.1", 6976, guid);
        }

        public void Write(string s)
        {
            mw.Write(s);
        }

        public void Log(string s)
        {
            log.write(s);
        }

        private void LoadConfig()
        {
            try
            {
                guid = Guid.Parse(cfg.get("guid"));
                Write("Configuration file found. Loading settings.");//#######################
                Log("Configuration file found. Loading settings.");
            }
            catch (Exception e)
            {
                CreateCFG();
            }
        }

        private void CreateCFG()
        {
            Write("Configuration file does not exist. Creating file."); // #############################
            Log("Configuration file does not exist. Creating file.");
            try
            {
                File.Delete(cfg.FileName());
            }
            catch (Exception e) { }

            cfg.reload();
            cfg.set("guid", Guid.NewGuid().ToString());
            cfg.Save();

            LoadConfig();
        }

        public Config GetConfig()
        {
            return cfg;
        }

        public void SendPacket(Packet p)
        {
            client.Data_OUT(p);
        }

        public void Speak(string s)
        {
            Write(s);
        }

        public static ShardCore GetCore()
        {
            return core;
        }

        public Client GetClient()
        {
            return client;
        }

        public void Shutdown()
        {
            client.Close();
        }
    }
}
