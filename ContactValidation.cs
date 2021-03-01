using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class ContactValidation
{
    public class Contact
    {
        public string FullName { get; set; }
        public string CityName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Error { get; set; }
    }

    public List<Contact> LoadAndValidateContacts() 
    {
        List<Contact> contactList = new List<Contact>();

        // Here I knew I needed to read in a json file and parse it into the Contact object that was created
        // I used a little help from StackOverflow on this one which lead to using a StreamReader in conjunction with
        // Newtonsoft.Json, as it does the deserialization for the developer out of the box
        // https://stackoverflow.com/questions/13297563/read-and-parse-a-json-file-in-c-sharp
        using (StreamReader reader = new StreamReader("./Contacts.json"))
        {
            string jsonFile = reader.ReadToEnd();
            contactList = JsonConvert.DeserializeObject<List<Contact>>(jsonFile);
        }
        
        contactList = ValidateContacts(contactList);
        OutputReport(contactList);

        return contactList;
    }

    public List<Contact> ValidateContacts(List<Contact> contacts)
    {
        foreach (var contact in contacts)
        {
            var phoneValid = true;
            for (int i = 0; i < contact.PhoneNumber.Length; ++i) 
            {
                var currentChar = contact.PhoneNumber[i];
                if (!Char.IsDigit(currentChar)
                    && currentChar != ' '
                    && currentChar != '-')
                {
                    phoneValid = false;
                    break;
                }
            }
            var emailValid = true;
            var atCount = 0;
            for (int i = 0; i < contact.EmailAddress.Length; ++i)
            {
                if (contact.EmailAddress[i] == '@')
                {
                    ++atCount;
                }
            }
            if (atCount != 1) {
                emailValid = false;
            }
            else 
            {
                // keep validating to see if there are characters before and after the @ symbol
                var atIndex = contact.EmailAddress.IndexOf("@");
                emailValid = atIndex != 0 && atIndex != contact.EmailAddress.Length;
            }

            if (!emailValid && !phoneValid)
            {
                contact.Error = "Email and Phone are invalid.";
            }
            else if (!emailValid && phoneValid)
            {
                contact.Error = "Email is invalid.";
            }
            else if (!phoneValid && emailValid)
            {
                contact.Error = "Phone is invalid.";
            }
            else if (phoneValid && emailValid)
            {
                contact.Error = "Valid";
            }
        }

        return contacts;
    }

    public void OutputReport(List<Contact> contactList)
    {
        contactList = contactList.OrderBy(x => x.FullName).ToList();

        Console.WriteLine("INDIVIDUAL REPORT");
        foreach (Contact contact in contactList)
        {
            Console.WriteLine(contact.FullName + ": " + contact.Error);
        }

        Dictionary<string, int> cityCount = new Dictionary<string, int>();
        
        // Get distinct list of city names from contact list
        var cityNames = contactList.Select(x => x.CityName).Distinct();

        foreach (Contact contact in contactList)
        {
            if (contact.Error != "Valid")
            {
                if (!cityCount.ContainsKey(contact.CityName))
                {
                    cityCount.Add(contact.CityName, 1);
                }
                else
                {
                    cityCount[contact.CityName] = cityCount[contact.CityName] + 1;
                }
            }
        }

        // Looked up how to sort a dictionary by value using linq
        // (https://stackoverflow.com/questions/289/how-do-you-sort-a-dictionary-by-value)
        var sortedDict = from city in cityCount orderby city.Value descending select city;

        Console.WriteLine("CITY REPORT");
        foreach (var item in sortedDict)
        {
            Console.WriteLine(item.Key + ": " + item.Value + " errors");
        }
    }
}