using AdoQRCode.Models;
using AdoQRCode.Repositories;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static QRCoder.PayloadGenerator;

namespace AdoQRCode
{
    class Program
    {
        public enum QrCodeType
        {
            TextEncodedQrCode,
            LocationEncodedQrCode
        }
        public class QrCodeEntity
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public byte[] Content { get; set; }
            public QrCodeType QrCodeType { get; set; }
        }

        public struct HabrNews
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
            public DateTime PubDate { get; set; }

            public override string ToString()
            {
                string str = string.Format("{0}\n--> {1}\n--> {2: dd.MM.yyyy}\n", Title, Link, PubDate);
                return str;
            }
        }

        static List<HabrNews> GetHabrNews()
        {
            List<HabrNews> habrNews = new List<HabrNews>();

            foreach (XmlNode item in GetDoc("https://habr.com/ru/rss/interesting/").SelectNodes("//rss/channel/item"))            
            {
                HabrNews habr = new HabrNews();
                habr.Title = item.SelectSingleNode("title").InnerText;
                habr.Link = item.SelectSingleNode("link").InnerText;
                habr.Description = item.SelectSingleNode("description").InnerText;
                habr.PubDate = Convert.ToDateTime(item.SelectSingleNode("pubDate").InnerText);
                habrNews.Add(habr);
            }

            return habrNews;
        }

        static XmlDocument GetDoc(string link)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(link);
            return doc;
        }
        

        static string GetCurr()
        {
            string des = "";

            foreach (XmlNode item in GetDoc("https://nationalbank.kz/rss/rates_all.xml").SelectNodes("//rss/channel/item"))
            {
                if (item.SelectSingleNode("title").InnerText == "USD")
                    des = item.SelectSingleNode("description").InnerText;
            }

            return des;
        }

        static void Main(string[] args)
        {
            ProductRepository rep = new ProductRepository();
            PurchaseRepository pur = new PurchaseRepository();
            QrCodeGeneratorService qr = new QrCodeGeneratorService();

            //Purchase purchase = new Purchase { PurchaseGuid = Guid.NewGuid(), PurchaseDate = DateTime.Now, ProductId = 2, CustomerName = "John Jones" };
            //purchase.PriceKZT = rep.Read(purchase.ProductId).PriceUsd * purchase.ExchangeRate;
            //purchase.PurchaseQr = qr.GetQrCodePurchaseInfo(purchase);
            //purchase.ShippingQr = qr.GetQrCodeShippingInfo(purchase);

            //string message = "";
            //pur.Add(purchase, out message);
            //Console.WriteLine(message);

            IEnumerable<Purchase> purchases = pur.ReadAll();
            if (purchases != null)
            {
                foreach (Purchase item in purchases)
                {
                    Console.WriteLine(item.PriceKZT);
                }
            }








        }
    }
}
