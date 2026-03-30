using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Submission
    {
        // Replace these with your real GitHub Pages/raw file URLs
        public static string xmlURL = "https://kanishk7436.github.io/assignment4xml/NationalParks.xml"; // Q1.2
        public static string xmlErrorURL = "https://kanishk7436.github.io/assignment4xml/NationalParksErrors.xml"; // Q1.3
        public static string xsdURL = "https://kanishk7436.github.io/assignment4xml/NationalParks.xsd"; // Q1.1

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            StringBuilder errors = new StringBuilder();

            try
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                using (XmlReader xsdReader = XmlReader.Create(xsdUrl))
                {
                    schemas.Add(null, xsdReader);
                }

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemas;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += delegate (object sender, ValidationEventArgs e)
                {
                    errors.AppendLine(e.Message);
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read())
                    {
                        // Just reading through the entire XML triggers validation
                    }
                }

                if (errors.Length == 0)
                {
                    return "No errors are found";
                }

                return errors.ToString().Trim();
            }
            catch (XmlException ex)
            {
                return ex.Message;
            }
            catch (XmlSchemaException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);

                string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                return jsonText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
