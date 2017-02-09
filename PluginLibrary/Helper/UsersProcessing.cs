using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PluginLibrary.Models
{
    public static class UsersProcessing
    {
        public static string GetUserByMail(string textJSON, string usermail)
        {


            string userID = "";
            List<User> listUser = new List<User>();


            System.IO.File.WriteAllText(@"C:\Temp\textJSONpreneseni.txt", "Pred ulazak u  u GetAllUsers");
            listUser = GetAllUsers(textJSON);

            int index = listUser.FindIndex(x => x.Email.Contains(usermail));
            if (index != -1)
                userID = listUser[index].User_Guid;
            return userID;


        }
        public static List<User> GetAllUsers(string  textJSON)
        {
            System.IO.File.WriteAllText(@"C:\Temp\textJSONpreneseni.txt", "usli u GetAllUsers");
            List<User> list = new List<User>();

            string pathFileUpis = @"C:\Temp\Test\TestVrtimoGUID.txt";
            using (StreamWriter sw = File.CreateText(pathFileUpis))
            {
                sw.WriteLine($"KRECE UPIS PODATAKA IZ JSON FILE");
                sw.WriteLine("");
            }

            dynamic newJSON = JsonConvert.DeserializeObject<object>(textJSON);
            int i = 1;
            foreach (var item in newJSON.data)
            {
                list.Add(new User
                {
                    FirstName = item[0],
                    LastName = item[1],
                    Email = item[2],
                    User_Guid = item[9]
                });
                using (StreamWriter sw = File.AppendText(pathFileUpis))
                {
                    sw.WriteLine($"User name: {item[2].ToString()}  user ID : {item[9].ToString()} .");
                }

            }
            return list;
        }
    }
}
