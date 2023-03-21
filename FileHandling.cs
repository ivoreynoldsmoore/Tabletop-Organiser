using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tabletop_Organiser.CharacterBuilder;

namespace Tabletop_Organiser.FileHandling
{
    public static class FileHandler
    {

        static JsonSerializerOptions options;
        static FileHandler()
        {
            options = new JsonSerializerOptions();
            options.WriteIndented = true;
        }

        public static void Initialise()
        {
            WriteRaces();
            //ReadRaces();
            WriteRoles();
            //ReadRoles();
        }
        private static async void WriteRoles()
        {
            using (StreamWriter file = new("Roles.json"))
            {
                await file.WriteAsync(JsonSerializer.Serialize(Roles.roles, options));
            }
        }

        private static async void ReadRoles()
        {
            using (StreamReader file = new("Roles.json"))
            {
                string json = await file.ReadToEndAsync();
                Roles.roles = JsonSerializer.Deserialize<Roles.Role[]>(json);
            }
        }

        private static async void WriteRaces()
        {
            using (StreamWriter file = new("Races.json"))
            {
                await file.WriteAsync(JsonSerializer.Serialize(Races.races, options));
            }
        }

        private static async void ReadRaces()
        {
            using (StreamReader file = new("Races.json"))
            {
                string json = await file.ReadToEndAsync();
                Races.races = JsonSerializer.Deserialize<Races.Race[]>(json);
            }
        }
    }
}
