using Rollback.Common.Commands.Types;
using Rollback.Protocol.Enums;
using Rollback.World.Game;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.World;

namespace Rollback.World.Commands.Commands
{
    public sealed class MonsterCommand : CommandContainer
    {
        public override string[] Aliases =>
            new[] { "monster", "monsters" };

        public override string Description =>
            "Command to manage monsters";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;
    }

    public sealed class MonsterMoveCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(MonsterCommand);

        public override string[] Aliases =>
            new[] { "move", "m" };

        public override string Description =>
            "Command to force all monster groups on the map to move.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        protected override void Execute(Character sender)
        {
            foreach (var monsterGroup in sender.MapInstance.GetActors<MonsterGroup>())
                monsterGroup.AutoMove();
        }
    }

    public sealed class MonsterStarsCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(MonsterCommand);

        public override string[] Aliases =>
            new[] { "stars", "s" };

        public override string Description =>
            "Command to add stars to monster groups.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public MonsterStarsCommand()
        {
            AddParameter($"Amount between 0 - {WorldObject.StarBonusLimit}.");
            AddParameter("Action zone : -map / -subArea / -area / -all.", "-map");
        }

        protected override void Execute(Character sender)
        {
            var amount = GetParameterValue<byte>(0);
            if (amount > WorldObject.StarBonusLimit)
                throw new Exception($"Incorrect amount of stars, must be between 0 - {WorldObject.StarBonusLimit}");

            foreach (var group in GetParameterValue<string>(1) switch
            {
                "-map" => sender.MapInstance.GetActors<MonsterGroup>(),
                "-subArea" => WorldManager.Instance.GetMaps(x => x.SubArea?.Id == sender.MapInstance.Map.SubArea?.Id).SelectMany(map => map.GetInstances().SelectMany(x => x.GetActors<MonsterGroup>())).ToArray(),
                "-area" => WorldManager.Instance.GetMaps(x => x.SubArea?.Area.Id == sender.MapInstance.Map.SubArea?.Area.Id).SelectMany(map => map.GetInstances().SelectMany(x => x.GetActors<MonsterGroup>())).ToArray(),
                "-all" => WorldManager.Instance.GetMaps().SelectMany(map => map.GetInstances().SelectMany(x => x.GetActors<MonsterGroup>())).ToArray(),
                _ => throw new Exception("Incorrect action zone value...")
            })
            {
                group.AgeBonus = amount;
                group.Refresh();
            }
        }
    }
}
