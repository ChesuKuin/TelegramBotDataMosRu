using System;
using System.IO;
using System.Net;
using System.Text;
using Telegram.Bot;
using Newtonsoft.Json.Linq;

namespace TelegramBotDataMosRu
{
    class Program
    {

        static public WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };

        static public string GetData(string id)
        {
            string data = String.Empty;

            string url = $"https://apidata.mos.ru/v1/datasets/1093/rows?$top=1&$skip={id}&api_key={}";
            try
            {
                string json = wc.DownloadString(url);

                Console.WriteLine(json);

                dynamic d = JArray.Parse(json);

                data = $"{d[0].Cells.ShortName} {d[0].Cells.Year} ";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data = "К сожалению данных не нашёл :'(";
            }
            return data;
        }

        static void Main(string[] args)
        {
            
            TelegramBotClient bot = new TelegramBotClient(File.ReadAllText(@"D:\Work\token.txt"));

            bot.OnMessage += (s, e) =>
            {
                var text = e.Message.Text;
                Console.WriteLine($"{text}") ;
                bot.SendTextMessageAsync(e.Message.Chat.Id, $"{GetData(text)}");
            };


            bot.StartReceiving();

            Console.ReadLine();

        }
    }
}
