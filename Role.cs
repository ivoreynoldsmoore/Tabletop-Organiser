using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Tabletop_Organiser.CharacterBuilder
{
    public static class Roles
    {
        public struct CharacterRole
        {
            public RoleIndex classIndex;

            public int subclassIndex;

            public CharacterRole(RoleIndex classIndex, int subclassIndex)
            {
                this.classIndex = classIndex;
                this.subclassIndex = subclassIndex;
            }
        }

        public enum RoleIndex
        {
            artificer,
            barbarian,
            bard,
            druid,
            fighter,
            monk,
            paladin,
            ranger,
            rogue,
            sorcerer,
            warlock,
            wizard
        }


        public class Role
        {
            public class Subrole
            {
                public string name { get; set; }
                public Feature[] features { get; private set; }
                public Subrole(string name, Feature[] features)
                {
                    this.name = name;
                    this.features = features;
                }
            }
            public string name { get; private set; }
            public RoleIndex index { get; private set; }
            public int hitDice { get; private set; }
            public bool[] saveProficiencies { get; private set; }
            public bool[] armourProficiencies { get; private set; }

            public bool[] weaponProficiencies { get; private set; }

            public bool[] toolProficieincies { get; private set; }
            public Feature[] features { get; private set; }

            public int subclassLevelReq { get; private set; }
            public Subrole[] subclasses { get; private set; }
            public Role(string name, RoleIndex index, int hitDice, Feature[] features, bool[] saveProficiencies, bool[] armourProficiencies, bool[] weaponProficiencies, bool[] toolProficieincies, Subrole[] subclasses, int subclassLevelReq = 3)
            {
                this.name = name;
                this.index = index;
                this.hitDice = hitDice;
                this.features = features;
                this.saveProficiencies = saveProficiencies;
                this.armourProficiencies = armourProficiencies;
                this.weaponProficiencies = weaponProficiencies;
                this.toolProficieincies = toolProficieincies;
                this.subclassLevelReq= subclassLevelReq;
                this.subclasses = subclasses;
            }
        }

        public static Role[] roles;
        static Roles()
        {
            roles = new Role[]
            {
                new Role("Artificer", RoleIndex.artificer, 8, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Barbarian", RoleIndex.barbarian, 12, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Bard", RoleIndex.bard, 10, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Druid", RoleIndex.druid, 8, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Fighter", RoleIndex.fighter, 10, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Monk", RoleIndex.monk, 10, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Paladin", RoleIndex.paladin, 10, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Ranger", RoleIndex.ranger, 10, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Rogue", RoleIndex.rogue, 8, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Sorcerer", RoleIndex.sorcerer, 6, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Warlock", RoleIndex.warlock, 8, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
                new Role("Wizard", RoleIndex.wizard, 6, new Feature[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new bool[] { }, new Role.Subrole[] { }),
            };
        }

        public static Role.Subrole[] GetSubclasses(RoleIndex index)
        {
            return roles.Single(charClass => charClass.index == index).subclasses;
        }

        public static Feature[] GetFeatures(CharacterRole charClassIndices)
        {
            Role charClass = roles.Single(charClass => charClass.index == charClassIndices.classIndex);
            Feature[] features = charClass.features;
            if (charClassIndices.subclassIndex >= 0)
            {
                features = features.Concat(charClass.subclasses[charClassIndices.subclassIndex].features).ToArray();
            }
            return features;
        }

        public static int GetSubclassLevelReq(RoleIndex index)
        {
            return roles.Single(charClass => charClass.index == index).subclassLevelReq;
        }
    }
}
