using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Criterion;
using Rollback.World.Game.Looks;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Npcs.Actions;
using Rollback.World.Game.RolePlayActors.Npcs.Replies;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.RolePlayActors.Npcs
{
    public sealed class Npc : ContextualActor
    {
        private readonly List<NpcAction> _actions;

        public NpcRecord Record { get; }

        public Npc(int contextualId, MapInstance mapInstance, Cell cell, DirectionsEnum direction, NpcRecord record, CriterionExpression? visibilityCriterion) :
            base(contextualId, mapInstance, cell, direction, ActorLook.Parse(record.EntityLookString), visibilityCriterion)
        {
            Record = record;
            _actions = NpcManager.Instance.GetActions(record);
        }

        private NpcAction? GetBestAction(Character character, sbyte actionId) =>
            _actions.FirstOrDefault(x => (sbyte)x.NpcActionType == actionId && x.CanExecute(character));

        public bool CanGiveQuest(Character character)
        {
            if (GetBestAction(character, (sbyte)NpcActionType.Talk) is TalkAction talkAction && talkAction.MessageId.HasValue)
            {
                var testedIds = new HashSet<short>();
                var replies = new Queue<NpcReply>(NpcManager.Instance.GetReplies(character, Record, talkAction.MessageId.Value));
                while (replies.TryDequeue(out var reply))
                {
                    if (reply is StartQuestReply)
                        return true;
                    else if (reply is DialogReply dialogReply && dialogReply.NewMessageId.HasValue && !testedIds.Contains(dialogReply.NewMessageId.Value))
                    {
                        testedIds.Add(dialogReply.NewMessageId.Value);

                        foreach (var newReply in NpcManager.Instance.GetReplies(character, Record, dialogReply.NewMessageId.Value))
                            replies.Enqueue(newReply);
                    }
                }
            }

            return false;
        }

        public void ExecuteBestAction(Character character, sbyte actionId) =>
                GetBestAction(character, actionId)?.Execute(this, character);

        public override GameRolePlayActorInformations GameRolePlayActorInformations(Character character) =>
            new GameRolePlayNpcInformations(Id,
                Look.GetEntityLook(),
                EntityDispositionInformations,
                Record.Id,
                Record.Gender,
                0,
                CanGiveQuest(character));
    }
}
