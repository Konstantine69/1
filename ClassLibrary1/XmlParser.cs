using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace XMLValidation
{
    public class XmlParser
    {
        public static AlpinistDiary ParseXml(string filePath)
        {
            AlpinistDiary diary = new AlpinistDiary();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Создание пространства имен
            XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("ns", "http://tempuri.org/AlpinistDiarySchema.xsd");

            XmlNodeList climbNodes = xmlDoc.SelectNodes("//ns:climb", nsManager);
            foreach (XmlNode climbNode in climbNodes)
            {
                Climb climb = new Climb();
                climb.PeakName = climbNode.SelectSingleNode("ns:peak_name", nsManager).InnerText;
                climb.Height = int.Parse(climbNode.SelectSingleNode("ns:height", nsManager).InnerText);
                climb.Country = climbNode.SelectSingleNode("ns:country", nsManager).InnerText;
                climb.VisitDate = DateTime.ParseExact(
                    climbNode.SelectSingleNode("ns:visit_date", nsManager).InnerText,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture
                );
                climb.ClimbTime = climbNode.SelectSingleNode("ns:climb_time", nsManager).InnerText;
                climb.DifficultyCategory = climbNode.SelectSingleNode("ns:difficulty_category", nsManager).InnerText;

                diary.Climbs.Add(climb);
            }

            return diary;
        }

    }
}
