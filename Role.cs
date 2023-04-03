using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using Tabletop_Organiser.CharacterBuilder.Proficiencies;

namespace Tabletop_Organiser.CharacterBuilder
{
    public static class Roles
    {
        public struct CharacterRole
        {
            public RoleIndex roleIndex;

            public int subroleIndex;

            public CharacterRole(RoleIndex classIndex, int subclassIndex)
            {
                this.roleIndex = classIndex;
                this.subroleIndex = subclassIndex;
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

            public HashSet<Abilities> saveProficiencies { get; set; } = new();
            public HashSet<Skills> skillProficiencies { get; set; } = new();
            public HashSet<ArmourCatagory> armourProficiencies { get; set; } = new();
            public HashSet<Weapons> weaponProficiencies { get; set; } = new();
            public HashSet<Tools> toolProficiencies { get; set; } = new();

            public Feature[] features { get; private set; }

            public int subroleLevelReq { get; private set; }
            public Subrole[] subroles { get; private set; }

            [JsonConstructor]
            public Role(string name, RoleIndex index, int hitDice, Feature[] features, HashSet<Abilities> saveProficiencies, HashSet<ArmourCatagory> armourProficiencies, HashSet<Weapons> weaponProficiencies, HashSet<Tools> toolProficiencies, Subrole[] subroles, int subroleLevelReq = 3)
            {
                this.name = name;
                this.index = index;
                this.hitDice = hitDice;
                this.features = features;
                this.saveProficiencies = saveProficiencies;
                this.armourProficiencies = armourProficiencies;
                this.weaponProficiencies = weaponProficiencies;
                this.toolProficiencies = toolProficiencies;
                this.subroleLevelReq= subroleLevelReq;
                this.subroles = subroles;
            }
        }

        public static Role[] roles;
        static Roles()
        {
            roles = new Role[]
            {
                new Role("Artificer", RoleIndex.artificer, 8, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Barbarian", RoleIndex.barbarian, 12, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Bard", RoleIndex.bard, 10, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Druid", RoleIndex.druid, 8, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Fighter", RoleIndex.fighter, 10, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Monk", RoleIndex.monk, 10, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Paladin", RoleIndex.paladin, 10, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Ranger", RoleIndex.ranger, 10, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Rogue", RoleIndex.rogue, 8, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Sorcerer", RoleIndex.sorcerer, 6, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Warlock", RoleIndex.warlock, 8, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
                new Role("Wizard", RoleIndex.wizard, 6, new Feature[] { }, new(), new(), new(), new(), new Role.Subrole[] { }),
            };
        }

        public static Role.Subrole[] GetSubclasses(RoleIndex index)
        {
            return roles.Single(charRole => charRole.index == index).subroles;
        }

        public static Role GetRole(RoleIndex index)
        {
            return roles.Single(charRole => charRole.index == index);
        }

        public static Role.Subrole? GetSubrole(CharacterRole role)
        {
            Role charRole = roles.Single(charRole => charRole.index == role.roleIndex);
            Role.Subrole? subrole = null;
            if (role.subroleIndex >= 0)
            {
                subrole = charRole.subroles[role.subroleIndex];
            }
            return subrole;
        }

        public static Feature[] GetFeatures(CharacterRole charRoleIndices)
        {
            Role charRole = roles.Single(charRole => charRole.index == charRoleIndices.roleIndex);
            Feature[] features = charRole.features;
            if (charRoleIndices.subroleIndex >= 0)
            {
                features = features.Concat(charRole.subroles[charRoleIndices.subroleIndex].features).ToArray();
            }
            return features;
        }

        public static int GetSubclassLevelReq(RoleIndex index)
        {
            return roles.Single(charRole => charRole.index == index).subroleLevelReq;
        }
    }
}
