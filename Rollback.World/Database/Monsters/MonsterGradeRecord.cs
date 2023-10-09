using Rollback.Common.ORM;
using Rollback.World.Game.Spells;

namespace Rollback.World.Database.Monsters
{
    public static class MonsterGradeRelator
    {
        public const string GetMonsterGradesByMonsterId = "SELECT * FROM monsters_grades WHERE MonsterId = {0} ORDER BY Grade";
    }

    [Table("monsters_grades")]
    public sealed record MonsterGradeRecord
    {
        public MonsterGradeRecord() =>
            Spells = new();

        [Key]
        public short MonsterId { get; set; }

        [Key]
        public sbyte Grade { get; set; }

        public short Level { get; set; }

        public int Health { get; set; }

        public short AP { get; set; }

        public short MP { get; set; }

        public short APDodge { get; set; }

        public short MPDodge { get; set; }

        public short EarthResistance { get; set; }

        public short AirResistance { get; set; }

        public short FireResistance { get; set; }

        public short WaterResistance { get; set; }

        public short NeutralResistance { get; set; }

        public short Wisdom { get; set; }

        public short Strength { get; set; }

        public short Intelligence { get; set; }

        public short Chance { get; set; }

        public short Agility { get; set; }

        public long XP { get; set; }

        public int MinKamas { get; set; }

        public int MaxKamas { get; set; }

        [Ignore]
        public Dictionary<short, Spell> Spells { get; set; }
    }
}
