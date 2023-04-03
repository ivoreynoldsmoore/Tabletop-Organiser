using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tabletop_Organiser.CharacterBuilder;
using Utils;
using System.Runtime.CompilerServices;

namespace Tabletop_Organiser.FileHandling
{
    public static class FileHandler
    {
        public static event EventHandler<EventArgs>? loadedRaces;
        public static event EventHandler<EventArgs>? loadedRoles;

        static JsonSerializerOptions options;
        static FileHandler()
        {
            options = new JsonSerializerOptions();
            options.WriteIndented = true;
        }

        public static void Initialise()
        {
            ConstructRaces();
            WriteRaces();
            WriteRoles();
            ReadRaces();
            ReadRoles();
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
                if (file == null)
                {
                    throw new FileNotFoundException("Roles.json not found");
                }
                string json = await file.ReadToEndAsync();
                if (json == null)
                {
                    throw new FileLoadException("Failed to load roles");
                }
                Roles.roles = JsonSerializer.Deserialize<Roles.Role[]>(json);
                loadedRoles?.Invoke(null, new EventArgs());
            }
        }

        private static async void WriteRaces()
        {
            using (StreamWriter file = new("Races.json"))
            {
                await file.WriteAsync(JsonSerializer.Serialize(Races.races, options));
            }
            using (StreamWriter file = new("Subraces.json"))
            {
                await file.WriteAsync(JsonSerializer.Serialize(Races.subraces, options));
            }
        }

        private static async void ReadRaces()
        {
            using (StreamReader file = new("Races.json"))
            {
                if (file == null)
                {
                    throw new FileNotFoundException("Races.json not found");
                }
                string json = await file.ReadToEndAsync();
                if (json == null)
                {
                    throw new FileLoadException("Failed to load races");
                }
                Races.races = JsonSerializer.Deserialize<Races.Race[]>(json, options);
            }
            using (StreamReader file = new("Subraces.json"))
            {
                if (file == null)
                {
                    throw new FileNotFoundException("Subraces.json not found");
                }
                string json = await file.ReadToEndAsync();
                if (json == null)
                {
                    throw new FileLoadException("Failed to load subraces");
                }
                Races.subraces = JsonSerializer.Deserialize<Races.Subrace[]>(json, options);
            }
            loadedRaces?.Invoke(null, new EventArgs());
        }

        private static void ConstructRaces()
        {
            Races.races = new Races.Race[] {
                new Races.Race(Races.RaceIndex.human, "Human", new AbilityScores(1, 1, 1, 1, 1, 1), new Feature[]{}, 30),
                new Races.Race(Races.RaceIndex.elf, "Elf", new AbilityScores(dexterity:2), new Feature[]{
                    new Feature("darkvision", "Darkvision", "Accustomed to twilit forests and the night sky, you have superior vision in dark and dim conditions. You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light. You can't discern color in darkness, only shades of gray."),
                    new Feature("fey_Ancest", "Fey Ancestry", "You have advantage on saving throws against being charmed, and magic can't put you to sleep."),
                    new Feature("trance", "Trance","Elves do not sleep. Instead they meditate deeply, remaining semi-conscious, for 4 hours a day. The Common word for this meditation is \u0022trance.\u0022 While meditating, you dream after a fashion; such dreams are actually mental exercises that have become reflexive after years of practice. After resting in this way, you gain the same benefit a human would from 8 hours of sleep.")
                }, 30),
                new Races.Race(Races.RaceIndex.halfElf, "Half Elf", new AbilityScores(charisma:2), new Feature[]{
                    new Feature("darkvision", "Darkvision", "Accustomed to twilit forests and the night sky, you have superior vision in dark and dim conditions. You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light. You can't discern color in darkness, only shades of gray."),
                    new Feature("fey_Ancest", "Fey Ancestry", "You have advantage on saving throws against being charmed, and magic can't put you to sleep."),
                }, 30),
                new Races.Race(Races.RaceIndex.dwarf, "Dwarf", new AbilityScores(constitution:2), new Feature[] {
                new Feature("darkvision","Darkvision", "Accustomed to twilit forests and the night sky, you have superior vision in dark and dim conditions. You can see in dim light within 60 feet of you as if it were bright light, and in darkness as if it were dim light. You can't discern color in darkness, only shades of gray."),
                new Feature("dwarf_Res", "Dwarven Resiliece", "You have advantage on saving throws against poison, and you have resistance against poison damage."),
                new Feature("stoneCunning","Stonecunning","Whenever you make an Intelligence (History) check related to the origin of stonework, you are considered proficient in the History skill and add double your proficiency bonus to the check, instead of your normal proficiency bonus.")
                }, 25),
                new Races.Race(Races.RaceIndex.halfOrc, "Half Orc", new AbilityScores(strength:2, constitution:1), new Feature[]{}, 30),
                new Races.Race(Races.RaceIndex.dragonborn, "Dragonborn", new AbilityScores(wisdom:2), new Feature[] { }, 30),
                new Races.Race(Races.RaceIndex.halfling, "Halfling", new AbilityScores(dexterity:2), new Feature[] { }, 25),
                new Races.Race(Races.RaceIndex.gnome, "Gnome", new AbilityScores(intelligence: 2), new Feature[] { }, 25),
                new Races.Race(Races.RaceIndex.tiefling, "Tiefling", new AbilityScores(charisma:2, intelligence:1), new Feature[] { }, 30)
            };
            Races.subraces = new Races.Subrace[]
            {
                new Races.Subrace(Races.SubraceIndex.drow, "Drow", Races.RaceIndex.elf, new AbilityScores(charisma:1), new Feature[]{
                        new Feature("sup_Darkvision", "Superior Darkvision", "Your darkvision has a range of 120 feet, instead of 60."),
                        new Feature("sun_Sens", "Sunlight Sensitivity", "You have disadvantage on attack rolls and Wisdom (Perception) checks that rely on sight when you, the target of the attack, or whatever you are trying to perceive is in direct sunlight."),
                        new Feature("drow_Magic", "Drow Magic", "You know the Dancing Lights cantrip. When you reach 3rd level, you can cast the Faerie Fire spell once with this trait and regain the ability to do so when you finish a long rest. When you reach 5th level, you can cast the Darkness spell onceand regain the ability to do so when you finish a long rest. Charisma is your spellcasting ability for these spells.")
                }),
                new Races.Subrace(Races.SubraceIndex.highElf, "High", Races.RaceIndex.elf, new AbilityScores(intelligence:1), Array.Empty<Feature>()),
                new Races.Subrace(Races.SubraceIndex.woodElf, "Wood", Races.RaceIndex.elf, new AbilityScores(wisdom: 1), new Feature[]{
                        new Feature("wild_Mask", "Mask of the Wild.", "You can attempt to hide even when you are only lightly obscured by foliage, heavy rain, falling snow, mist, and other natural phenomena.")
                }),
                new Races.Subrace(Races.SubraceIndex.hillDwarf, "Hill", Races.RaceIndex.dwarf, new AbilityScores(wisdom: 1), Array.Empty<Feature>()),
                new Races.Subrace(Races.SubraceIndex.mountainDwarf, "Mountain", Races.RaceIndex.dwarf, new AbilityScores(strength: 2), Array.Empty<Feature>()),
                new Races.Subrace(Races.SubraceIndex.lightfootHalfling, "Lightfoot", Races.RaceIndex.halfling, new AbilityScores(charisma: 1), Array.Empty<Feature>()),
                new Races.Subrace(Races.SubraceIndex.stoutHalfling, "Stout", Races.RaceIndex.halfling, new AbilityScores(constitution: 1), Array.Empty<Feature>()),
                new Races.Subrace(Races.SubraceIndex.forestGnome, "Forest", Races.RaceIndex.gnome, new AbilityScores(dexterity:1), Array.Empty<Feature>()),
                new Races.Subrace (Races.SubraceIndex.rockGnome, "Rock", Races.RaceIndex.gnome, new AbilityScores(constitution:1), Array.Empty<Feature>()),
                new Races.Subrace (Races.SubraceIndex.svirfneblin, "Dark", Races.RaceIndex.gnome, new AbilityScores(dexterity:1), Array.Empty<Feature>())
            };
        }
    }
}
