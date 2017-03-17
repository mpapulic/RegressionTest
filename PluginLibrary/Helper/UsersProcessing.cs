using System.Collections.Generic;
using Newtonsoft.Json;

namespace PluginLibrary.Models
{
    public static class UsersProcessing
    {
        public static string GetUserByMail(string textJSON, string usermail)
        {
            string userID = "";
            List<User> listUser = new List<User>();


            try
            {
                listUser = GetAllUsers(textJSON);
                int index = listUser.FindIndex(x => x.Email.Contains(usermail));
                if (index != -1)
                    userID = listUser[index].User_Guid;
            }
            catch (System.Exception e)
            {
                System.IO.File.WriteAllText(@"C:\Temp\ERROR.txt", "Postoji problem kod preuzimanja JSON:" + e.Message);
            }
            return userID;
        }
        public static List<User> GetAllUsers(string textJSON)
        {
            List<User> list = new List<User>();

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
                i++;
            }
            return list;
        }
    }
}
