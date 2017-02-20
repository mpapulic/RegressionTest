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


            System.IO.File.WriteAllText(@"C:\Temp\TestGetUserByMail.txt", "Usao u GetUserByMail:" + textJSON + " a trazimo usera sa mail adresom :" + usermail);
            try
            {
                listUser = GetAllUsers(textJSON);
                int index = listUser.FindIndex(x => x.Email.Contains(usermail));
                if (index != -1)
                    userID = listUser[index].User_Guid;
                System.IO.File.WriteAllText(@"C:\Temp\TestGetUserByMail.txt", "Izlaza iz GetUserByMail:" + " trazeni GUID  :" + userID);
            }
            catch (System.Exception e)
            {
                System.IO.File.WriteAllText(@"C:\Temp\ERROR.txt", "Postoji problem kod preuzimanja JSON:" + e.Message );
            }
            return userID;
        }
        public static List<User> GetAllUsers(string textJSON)
        {
            List<User> list = new List<User>();

            dynamic newJSON = JsonConvert.DeserializeObject<object>(textJSON);
            int i = 1;
            System.IO.File.WriteAllText(@"C:\Temp\GetAllUsers.txt", "usao u GetAllUsers newJSON i:" + newJSON.ToString());
            foreach (var item in newJSON.data)
            {
                list.Add(new User
                {
                    FirstName = item[0],
                    LastName = item[1],
                    Email = item[2],
                    User_Guid = item[9]
                });
                using (StreamWriter sw = File.AppendText(@"C:\Temp\GetAllUsers.txt"))
                {
                    sw.WriteLine($"/r User name:{item[1].ToString()} {item[2].ToString()}  user ID : {item[9].ToString()} .");
                    sw.WriteLine($"/r IZ LISTE User name:{list[0].FirstName} {list[0].LastName}  user ID : {list[0].User_Guid} .");
                }
                i++;
            }
            //System.IO.File.WriteAllText(@"C:\Temp\GetAllUsersIZLAZ.txt", "usao u GetAllUsers newJSON i:" + list.ToString());
            return list;
        }
    }
}
