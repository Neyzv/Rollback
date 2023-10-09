using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Characters.Breeds;

namespace Rollback.World.Game.RolePlayActors.Npcs.Replies
{
    [Identifier("Statue")]
    public sealed class StatueReply : NpcReply
    {

        public StatueReply(NpcReplyRecord record) : base(record) { }

        public override bool Execute(Npc npc, Character character)
        {
            if (BreedManager.GetStatueMapCellInfos(character.Breed) is { } tpInfos)
                character.Teleport(tpInfos.Key, tpInfos.Value);
            else
                character.SendServerMessage("Can not find statue informations for breed " + character.Breed);

            return true;
        }
    }
}
