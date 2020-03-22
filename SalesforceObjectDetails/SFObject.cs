using System.Collections.Generic;
using System.Text;

namespace SalesforceObjectDetails
{
    public class SFObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SFObject" /> class.
        /// </summary>
        /// <param name="name">    The Salesforce Object name. </param>
        /// <param name="nameAPI"> The Salesforce Objec name API. </param>
        public SFObject(string name, string nameAPI)
        {
            NAME = name;
            NAMEAPI = nameAPI;
            FieldPropertyList = new List<Field>();
            RecordTypes = new List<RecordType>();
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value> The description. </value>
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// Gets or sets the Salesforce field property list.
        /// </summary>
        /// <value> The field property list. </value>
        public List<Field> FieldPropertyList { get; set; }

        /// <summary>
        /// Gets or sets the lookups.
        /// </summary>
        /// <value> The lookups properties for a related Object. </value>
        public List<string> LookUps { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value> The Salesforce Object name. </value>
        public string NAME { get; }

        /// <summary>
        /// Gets the nameapi.
        /// </summary>
        /// <value> The nameapi. </value>
        public string NAMEAPI { get; }

        public List<RecordType> RecordTypes { get; set; }

        /// <summary>
        /// Adds the field to list.
        /// </summary>
        /// <param name="field"> The Salesforce field. </param>
        public void AddFieldToList(Field field) => FieldPropertyList.Add(field);

        /// <summary>
        /// Removes the field from list.
        /// </summary>
        /// <param name="field"> The Salesforce field. </param>
        public void RemoveFieldFromList(Field field) => FieldPropertyList.Remove(field);

        //Print a clean verion of a Salesforce Object
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns> A Pretty version of the Salesforce Object as a String </returns>
        public override string ToString()
        {
            //Need to add structures both Field and Record Type
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("\nName: " + this.NAME + "\n");
            stringBuilder.Append("nameAPI: " + this.NAMEAPI + "\n");
            stringBuilder.Append("Description: " + this.DESCRIPTION + "\n");
            stringBuilder.Append("\nlookups: ");
            if (LookUps.Count > 0)
            {
                foreach (var item in LookUps)
                    stringBuilder.Append(item + "\n\t ");
                stringBuilder.Append("\n");
            }

            foreach (var item in FieldPropertyList)
            {
                stringBuilder.Append("Field:  \n\t");
                stringBuilder.Append("Name:\t\t  " + item.Name + "\n\t");
                stringBuilder.Append("nameAPI:\t  " + item.FullNameAPI + "\n\t");
                stringBuilder.Append("Description:\t " + item.Description + "\n\t");
                stringBuilder.Append("Type:\t\t\t " + item.Type + "\n\t");
                stringBuilder.Append("Length:\t\t  " + item.Length + "\n\t");
                stringBuilder.Append("Formula:\t\t " + item.Formula + "\n\t");
                stringBuilder.Append("External ID:\t  " + item.ExternalId + "\n\t");
                stringBuilder.Append("Required:\t  " + item.Required + "\n\t");
                stringBuilder.Append("TrackFeedHistory: " + item.TrackFeedHistory + "\n\t");
                stringBuilder.Append("Unique:\t\t  " + item.Unique + "\n\t\n");
            }
            return stringBuilder.ToString();
        }
    }
}