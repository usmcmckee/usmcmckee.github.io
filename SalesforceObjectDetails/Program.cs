using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace SalesforceObjectDetails
{
    internal class Program
    {
        private static bool completedObject = false;
        private static string inputDirectory = "C:\\Users\\User\\Documents\\VScode\\sandbox\\force-app\\main\\default\\objects\\";
        private static string outputDircetory;
        private static SFObject sFObject;
        private static List<SFObject> sFObjectsList;

        private static SalesforceClient CreateClient()
        {
            return new SalesforceClient
            {
                Username = ConfigurationManager.AppSettings["username"],
                Password = ConfigurationManager.AppSettings["password"],
                Token = ConfigurationManager.AppSettings["token"],
                ClientId = ConfigurationManager.AppSettings["clientId"],
                ClientSecret = ConfigurationManager.AppSettings["clientSecret"]
            };
        }

        /// <summary>
        /// Creates the field properties CSV.
        /// </summary>
        /// <param name="list"> The list of Salesforce Objects. </param>
        private static void CreateFieldPropertiesCSV(List<SFObject> list)
        {
            //Set up CSV object and csv settings
            using var memoryStream = new MemoryStream();
            Encoding encoding = GetEncoding();
            using StreamWriter writer = new StreamWriter(memoryStream, encoding);
            using CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture);
            csvWriter.Configuration.Delimiter = ",";
            csvWriter.Configuration.HasHeaderRecord = true;

            //Build Headers
            csvWriter.WriteField("Object Name");
            csvWriter.WriteField("Object API Name");
            csvWriter.WriteField("Field Name");
            csvWriter.WriteField("Field API Name");
            csvWriter.WriteField("Description");
            csvWriter.WriteField("Data Type");
            csvWriter.WriteField("Length");
            csvWriter.WriteField("External Id?");
            csvWriter.WriteField("Required?");
            csvWriter.WriteField("Unique?");
            csvWriter.WriteField("Formula");
            csvWriter.NextRecord();
            //Build csv rows for each Salesforce Object and Field Properties
            foreach (SFObject sf in list)
            {
                foreach (Field sfField in sf.FieldPropertyList)
                {
                    csvWriter.WriteField(sf.NAME);
                    csvWriter.WriteField(sf.NAMEAPI);
                    csvWriter.WriteField(sfField.Name);
                    csvWriter.WriteField(sfField.FullNameAPI);
                    csvWriter.WriteField(sfField.Description);
                    csvWriter.WriteField(sfField.Type);
                    csvWriter.WriteField(sfField.Length);
                    csvWriter.WriteField(sfField.ExternalId);
                    csvWriter.WriteField(sfField.Required);
                    csvWriter.WriteField(sfField.Unique);
                    csvWriter.WriteField(sfField.Formula);
                    csvWriter.NextRecord();
                }
            }
            writer.Flush();

            var result = encoding.GetString(memoryStream.ToArray());
            Console.WriteLine(result);
            //Create file
            File.WriteAllText(path: outputDircetory + "Field Properties" + ".csv", contents: result);
        }

        /// <summary>
        /// Creates the lookup CSV.
        /// </summary>
        /// <param name="list"> The list. </param>
        private static void CreateLookupCSV(List<SFObject> list)
        {
            //Set up CSV object and csv settings
            using var memoryStream = new MemoryStream();
            Encoding encoding = GetEncoding();
            using StreamWriter writer = new StreamWriter(memoryStream, encoding);
            using CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture);
            csvWriter.Configuration.Delimiter = ",";
            csvWriter.Configuration.HasHeaderRecord = true;

            //Build Headers
            csvWriter.WriteField("Object Name");
            csvWriter.WriteField("Object API Name");
            csvWriter.WriteField("Lookup");

            csvWriter.NextRecord();
            //Build csv rows for each Salesforce Object and Field Properties
            foreach (SFObject sf in list)
            {
                foreach (string lookUp in sf.LookUps)
                {
                    csvWriter.WriteField(sf.NAME);
                    csvWriter.WriteField(sf.NAMEAPI);
                    csvWriter.WriteField(lookUp);
                    csvWriter.NextRecord();
                }
            }
            writer.Flush();

            var result = encoding.GetString(memoryStream.ToArray());
            Console.WriteLine(result);
            //Create file
            File.WriteAllText(path: outputDircetory + "Lookup" + ".csv", contents: result);
        }

        /// <summary>
        /// Creates the output folder that will contain the csv files.
        /// </summary>
        /// <returns> outputDirectory </returns>
        private static string createOutputFolder()
        {
            string outputFolder = "Salesforce Object Details_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
            string outputDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar;
            if (!System.IO.Directory.Exists(outputDir + outputFolder))
            {
                System.IO.Directory.CreateDirectory(outputDir + outputFolder);
            }
            return outputDir + outputFolder + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Creates the record types CSV.
        /// </summary>
        /// <param name="list"> The list. </param>
        private static void CreateRecordTypesCSV(List<SFObject> list)
        {
            //Set up CSV object and csv settings
            using var memoryStream = new MemoryStream();
            Encoding encoding = GetEncoding();
            using StreamWriter writer = new StreamWriter(memoryStream, encoding);
            using CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture);
            csvWriter.Configuration.Delimiter = ",";
            csvWriter.Configuration.HasHeaderRecord = true;

            //Build Headers
            csvWriter.WriteField("Object Name");
            csvWriter.WriteField("Object API Name");
            csvWriter.WriteField("RecordType Name");
            csvWriter.WriteField("RecordType API Name");
            csvWriter.WriteField("Description");

            csvWriter.NextRecord();
            //Build csv rows for each Salesforce Object and Field Properties
            foreach (SFObject sf in list)
            {
                foreach (RecordType rt in sf.RecordTypes)
                {
                    csvWriter.WriteField(sf.NAME);
                    csvWriter.WriteField(sf.NAMEAPI);
                    csvWriter.WriteField(rt.Name);
                    csvWriter.WriteField(rt.FullNameAPI);
                    csvWriter.WriteField(rt.Description);
                    csvWriter.NextRecord();
                }
            }
            writer.Flush();

            var result = encoding.GetString(memoryStream.ToArray());
            Console.WriteLine(result);
            //Create file
            File.WriteAllText(path: outputDircetory + "RecordType" + ".csv", contents: result);
        }

        /// <summary>
        /// Creates the sf object.
        /// </summary>
        /// <param name="file"> The file. </param>
        /// <returns> SFObject </returns>
        private static SFObject CreateSfObject(FileInfo file)
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(file.DirectoryName + Path.DirectorySeparatorChar + file.Name);
            //Display all the book titles.
            XmlElement descriptionXML = doc["CustomObject"]["description"];
            XmlElement fullNameXML = doc["CustomObject"]["fullName"];
            XmlElement nameXML = doc["CustomObject"]["label"];
            //If the XML document does not have the information need then replace with the file name
            string fullName = (fullNameXML == null) ? file.Name.Substring(0, file.Name.IndexOf('.')) : fullNameXML.InnerText;
            string name = (nameXML == null) ? file.Name.Substring(0, file.Name.IndexOf('.')) : nameXML.InnerText;

            // Create the Salesforce object with the base level of details
            SFObject sFObject = new SFObject(name, fullName);
            //If the description is not empty then assign it to the objects property
            if (descriptionXML != null)
                sFObject.DESCRIPTION = descriptionXML.InnerText;
            //Build a list of all of the lookups
            List<string> lookupList = new List<string>();
            XmlNodeList elemList = doc.GetElementsByTagName("searchLayouts");
            for (int i = 0; i < elemList.Count; i++)
            {
                XmlNodeList list = elemList[i].ChildNodes;
                foreach (XmlNode item in list)
                    if (item.Name.Contains("lookup"))
                        lookupList.Add(item.InnerText);
            }
            sFObject.LookUps = lookupList;
            //Done grabing information from the frist file, return what was collected
            return sFObject;
        }

        //When using Excel on Windows machine you need to defualt to Unicode instead of UTF8
        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <returns> Encodeing Type </returns>
        private static Encoding GetEncoding()
        {
            return (Environment.OSVersion.Platform.ToString().Contains("Win")) ? Encoding.Unicode : Encoding.Default;
        }

        /// <summary>
        /// Gets the sf object field properties.
        /// </summary>
        /// <param name="file">     The file. </param>
        /// <param name="sFObject"> A Salesforce Object. </param>
        /// <returns> SFObject </returns>
        private static SFObject getSfObjectFieldProperties(FileInfo file, SFObject sFObject)
        {
            //Each XML file represents one field
            Field field = new Field();

            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(file.DirectoryName + Path.DirectorySeparatorChar + file.Name);

            //Collect all information needed from the XML file
            XmlElement fullNameXML = doc["CustomField"]["fullName"];
            XmlElement nameXML = doc["CustomField"]["label"];
            XmlElement descriptionXML = doc["CustomField"]["description"];
            XmlElement typeXML = doc["CustomField"]["type"];
            XmlElement inlineHelpTextXML = doc["CustomField"]["inlineHelpText"];
            XmlElement formulaXML = doc["CustomField"]["formula"];
            XmlElement lengthXML = doc["CustomField"]["length"];
            XmlElement externalIdXML = doc["CustomField"]["externalId"];
            XmlElement RequiredXML = doc["CustomField"]["required"];
            XmlElement trackFeedHistoryXML = doc["CustomField"]["trackFeedHistory"];
            XmlElement uniqueXML = doc["CustomField"]["unique"];

            //If the XML document does not have the information need then replace with the file name
            string fullName = (fullNameXML == null) ? file.Name.Substring(0, file.Name.IndexOf('.')) : fullNameXML.InnerText;
            string name = (nameXML == null) ? file.Name.Substring(0, file.Name.IndexOf('.')) : nameXML.InnerText;

            //Map XML Properties to field properties
            field.FullNameAPI = fullName;
            field.Name = name;
            field.Description = (descriptionXML?.InnerText == null) ? null : Regex.Replace(descriptionXML?.InnerText, @"\t|\n|\r", " ");
            field.Type = typeXML?.InnerText?.ToString();
            field.InlineHelpText = (inlineHelpTextXML?.InnerText == null) ? null : Regex.Replace(inlineHelpTextXML?.InnerText, @"\t|\n|\r", " ");
            field.Formula = (formulaXML == null) ? null : Regex.Replace(formulaXML?.InnerText, @"\t|\n|\r", " ");
            field.Length = (lengthXML?.InnerText == null) ? -1 : int.Parse(lengthXML?.InnerText);
            field.ExternalId = (externalIdXML?.InnerText == null) ? false : bool.Parse(externalIdXML?.InnerText);
            field.Required = (RequiredXML?.InnerText == null) ? false : bool.Parse(RequiredXML?.InnerText);
            field.TrackFeedHistory = (trackFeedHistoryXML?.InnerText == null) ? false : bool.Parse(trackFeedHistoryXML?.InnerText);
            field.Unique = (uniqueXML?.InnerText == null) ? false : bool.Parse(uniqueXML?.InnerText);

            //Added property to the current Salesforce object field list
            sFObject.FieldPropertyList.Add(field);
            // return the updated object
            return sFObject;
        }

        /// <summary>
        /// Gets the sf object record types.
        /// </summary>
        /// <param name="file">     The file. </param>
        /// <param name="sFObject"> A Salesforce Object. </param>
        /// <returns> SFObject </returns>
        private static SFObject getSfObjectRecordTypes(FileInfo file, SFObject sFObject)
        {
            //Each XML file represents one recordType
            RecordType recordType = new RecordType();
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(file.DirectoryName + Path.DirectorySeparatorChar + file.Name);

            //If the XML document does not have the information need then replace with the file name
            XmlElement fullNameXML = doc["RecordType"]["fullName"];
            XmlElement nameXML = doc["RecordType"]["label"];
            XmlElement descriptionXML = doc["RecordType"]["description"];

            //If the XML document does not have the information need then replace with the file name
            string fullName = (fullNameXML == null) ? file.Name.Substring(0, file.Name.LastIndexOf('.')) : fullNameXML.InnerText;
            string name = (nameXML == null) ? file.Name.Substring(0, file.Name.LastIndexOf('.')) : nameXML.InnerText;

            //Map XML Properties to recordTypes properties
            recordType.FullNameAPI = fullName;
            recordType.Name = name;
            recordType.Description = descriptionXML?.InnerText?.ToString();

            //Added property to the current Salesforce object RecordType list
            sFObject.RecordTypes.Add(recordType);
            // return the updated object
            return sFObject;
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args"> The arguments. </param>
        private static void Main(string[] args)
        {
            var client = CreateClient();

            if (args.Length > 0)
            {
                client.Login();
                Console.WriteLine(client.Query(args[0]));
            }
            else
            {
                client.Login();
                //Console.WriteLine(client.Describe("Account"));
                string userQuery;
                char response;
                Console.WriteLine("Do you want to run a Query? (Y or N)");
                response = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine("\nUser response was: {0}", response);
                if (response == 'Y')
                {
                    Console.WriteLine("Please write you SQOL: ");
                    userQuery = Console.ReadLine().Trim();
                    Console.WriteLine(client.Query(userQuery));
                }
            }
            Console.ReadLine();
            outputDircetory = createOutputFolder();
            sFObjectsList = new List<SFObject>();
            sFObject = new SFObject("", "");
            //Get File Path to the Object Folder in Salesforce then if the path does does contain the ending slash "\" then added it.
            Console.WriteLine("Please enter a file path the Salesforce object folder\nExample Shown Below\n{0}\nEnter Path: ", inputDirectory);
            inputDirectory = Console.ReadLine();
            inputDirectory = (inputDirectory[inputDirectory.Length - 1] == Path.DirectorySeparatorChar) ? inputDirectory : inputDirectory + Path.DirectorySeparatorChar;
            //Check if the Dir
            if (Directory.Exists(inputDirectory))
            {
                // This path is a directory
                ProcessDirectory(inputDirectory);
                //After gathering all the Salesforce Object create the CSV on the desktop
                CreateFieldPropertiesCSV(sFObjectsList);
                CreateLookupCSV(sFObjectsList);
                CreateRecordTypesCSV(sFObjectsList);
            }
            else
            {
                //Display Error message then wait for response
                Console.WriteLine("{0} is not a valid file or directory.", inputDirectory);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Process all files in the directory passed in, recurse on any directories that are found,
        /// and process the files they contain.
        /// </summary>
        /// <param name="targetDirectory"> The target directory. </param>
        private static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                FileInfo file = new FileInfo(fileName);
                if (file.Name.Contains("object-meta"))
                {
                    Console.WriteLine("Processed object from File:  '{0}'.", fileName);
                    // If all information has been gather for the Salesforce object then add it to
                    // the list
                    if (completedObject == true)
                    {
                        sFObjectsList.Add(sFObject);
                        completedObject = false;
                        Console.WriteLine(sFObject.ToString());
                    }
                    //Read the first XML file to gather the objects base level information
                    sFObject = CreateSfObject(file);
                    break;
                }
                else if (file.Name.Contains("recordType-meta"))
                {
                    //If the Salesforce object has a record type then add it to the current object
                    sFObject = getSfObjectRecordTypes(file, sFObject);
                }
                else if (file.Name.Contains("field-meta"))
                {
                    //If the Salesforce object has fields then add it to the current object
                    sFObject = getSfObjectFieldProperties(file, sFObject);
                    //When the field level is reached then there are no more level to the object, so marked as completed
                    completedObject = true;
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory);
            }
        }
    }
}