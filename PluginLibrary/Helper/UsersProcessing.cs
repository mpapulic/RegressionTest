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


            System.IO.File.WriteAllText(@"C:\Temp\STEP1.txt", "Pred deserializaciju:" + textJSON + " a trazimo usera sa mail adresom :" + usermail);
            try
            {
                dynamic newJSON = JsonConvert.DeserializeObject<object>(textJSON);
                System.IO.File.WriteAllText(@"C:\Temp\STEP2.txt", "Posle deserializacije:" + newJSON.ToString());
                listUser = GetAllUsers(newJSON.ToString());
                int index = listUser.FindIndex(x => x.Email.Contains(usermail));
                if (index != -1)
                    userID = listUser[index].User_Guid;
            }
            catch (System.Exception e)
            {
                System.IO.File.WriteAllText(@"C:\Temp\ERROR.txt", "Postoji problem kod preuzimanja JSON:" + e.Message );
            }
            return userID;
        }
        public static List<User> GetAllUsers(dynamic newJSON)
        {
            System.IO.File.WriteAllText(@"C:\Temp\STEP3.txt", "usao u GetAllUsers:");
            List<User> list = new List<User>();

            //string pathFileUpis = @"C:\Temp\Test\TestVrtimoGUID.txt";
            //using (StreamWriter sw = File.CreateText(pathFileUpis))
            //{
            //    sw.WriteLine($"KRECE UPIS PODATAKA IZ JSON FILE");
            //    sw.WriteLine("");
            //}
            //System.IO.File.WriteAllText(@"C:\Temp\textJSONpreneseni.txt", "STEP 2");

            //dynamic newJSON = JsonConvert.DeserializeObject<object>(textJSON);
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
                //System.IO.File.WriteAllText(@"C:\Temp\textJSONpreneseni.txt", "STEP:" + i.ToString());
                //using (StreamWriter sw = File.AppendText(pathFileUpis))
                //{
                //    sw.WriteLine($"User name: {item[2].ToString()}  user ID : {item[9].ToString()} .");
                //}

            }
            return list;
        }
    }
}
