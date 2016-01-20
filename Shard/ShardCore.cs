using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ConnectionData;

namespace Shard
{
    class ShardCore
    {
        private Client client;
        private static ShardCore core;
        private Log log;
        private Config cfg;
        private Guid guid;

        public static string commandKey, systemName = "Crystal Shard Dev", baseDir = "/CrystalHomeSys/", logBaseDir = "Logs/", configDir = "shard_config.cfg";

        public ShardCore()
        {
            baseDir = System.Environment.GetEnvironmentVariable("HOME") + baseDir;
            logBaseDir = baseDir + logBaseDir;
            configDir = baseDir + configDir;

            log = new Log(logBaseDir);

            core = this;
            Log("####################" + systemName + " has been Started#####################");

            cfg = new Config(configDir);
            LoadConfig();

            Write("Setting up client...");
            Log("Setting up client...");
            client = new Client("127.0.0.1", 6976, guid);
        }

        public void Write(string s)
        {
            Console.WriteLine(s);
        }

        public void Log(string s)
        {
            Write(s);
            log.write(s);
            try
            {
                Packet logPacket = new Packet(Packet.PacketType.ShardLog, guid.ToString());
                logPacket.packetString = s;
                SendPacket(logPacket);
            }
            catch (Exception e) { }
        }

        private void LoadConfig()
        {
            try
            {
                guid = Guid.Parse(cfg.get("guid"));
                Log("Configuration file found. Loading settings.");
            }
            catch (Exception e)
            {
                CreateCFG();
            }
        }

        private void CreateCFG()
        {
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
            if (client.connected)
                client.Data_OUT(p);
        }

        public void Speak(string s)
        {

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

        static void Main(string[] args)
        {
            new ShardCore();
        }
    }
}
