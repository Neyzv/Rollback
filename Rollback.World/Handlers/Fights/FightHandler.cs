using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Triggers;
using Rollback.World.Game.Fights.Triggers.Types;
using Rollback.World.Game.Interactions.Requests.Fights;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Fights
{
    public static class FightHandler
    {
        [WorldHandler(GameRolePlayPlayerFightRequestMessage.Id)]
        public static void HandleGameRolePlayPlayerFightRequestMessage(WorldClient client, GameRolePlayPlayerFightRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
            {
                var target = WorldServer.Instance.GetClient(x => x.Account?.Character is not null && x.Account.Character.Id == message.targetId)?.Account!.Character;
                if (target is not null)
                {
                    var result = client.Account!.Character!.CanRequestFight(target, message.friendly);
                    if (result is FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                    {
                        if (message.friendly)
                            new FightRequest(client.Account.Character, target).Open();
                    }
                    else
                        SendChallengeFightJoinRefusedMessage(client, result);
                }
            }
        }

        [WorldHandler(GameRolePlayPlayerFightFriendlyAnswerMessage.Id)]
        public static void HandleGameRolePlayPlayerFightFriendlyAnswerMessage(WorldClient client, GameRolePlayPlayerFightFriendlyAnswerMessage message)
        {
            if (client.Account!.Character!.Interaction is FightRequest request)
            {
                if (message.accept)
                    request.Accept();
                else
                    request.Close();
            }
        }

        [WorldHandler(GameFightPlacementPositionRequestMessage.Id)]
        public static void SendGameFightPlacementPositionRequestMessage(WorldClient client, GameFightPlacementPositionRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is not null && client.Account.Character.Cell.Id != message.cellId)
                client.Account.Character.Fighter.ChangePlacementCell(message.cellId);
        }

        [WorldHandler(GameFightJoinRequestMessage.Id)]
        public static void HandleGameFightJoinRequestMessage(WorldClient client, GameFightJoinRequestMessage message)
        {
            if (client.Account!.Character!.Fighter is null)
            {
                var fight = client.Account.Character.MapInstance.GetFight(message.fightId);

                if (fight is not null)
                {
                    if (message.fighterId is 0) // TO DO spectate
                    {

                    }
                    else
                    {
                        var team = default(Team?);

                        if (message.fighterId == fight.Challengers.Leader!.Id)
                            team = fight.Challengers;
                        else if (message.fighterId == fight.Defenders.Leader!.Id)
                            team = fight.Defenders;

                        if (team is not null)
                        {
                            var reason = team.CanJoin(client.Account.Character);

                            if (reason is FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                                client.Account.Character.JoinTeam(team);
                            else
                                SendChallengeFightJoinRefusedMessage(client, reason);
                        }
                    }
                }
            }
        }

        [WorldHandler(GameContextKickMessage.Id)]
        public static void HandleGameContextKickMessage(WorldClient client, GameContextKickMessage message) =>
            client.Account!.Character!.Fighter?.Team!.KickFighter(client.Account.Character.Fighter, message.targetId);

        [WorldHandler(GameFightOptionToggleMessage.Id)]
        public static void HandleGameFightOptionToggleMessage(WorldClient client, GameFightOptionToggleMessage message)
        {
            if (client.Account!.Character!.Fighter is not null && client.Account.Character.Fighter.IsLeader &&
                Enum.IsDefined(typeof(FightOptionsEnum), (int)message.option))
                client.Account.Character.Fighter.Team!.ToggleOption((FightOptionsEnum)message.option);
        }

        [WorldHandler(GameFightReadyMessage.Id)]
        public static void HandleGameFightReadyMessage(WorldClient client, GameFightReadyMessage message) =>
            client.Account!.Character!.Fighter?.SetReady(message.isReady);

        [WorldHandler(GameActionAcknowledgementMessage.Id)]
        public static void HandleGameActionAcknowledgementMessage(WorldClient client, GameActionAcknowledgementMessage message) =>
                client.Account!.Character!.Fighter?.Team!.Fight.AcknowledgeSequence(client.Account.Character.Fighter, message.actionId);

        [WorldHandler(GameFightTurnReadyMessage.Id)]
        public static void HandleGameFightTurnReadyMessage(WorldClient client, GameFightTurnReadyMessage message)
        {
            if (message.isReady && client.Account!.Character!.Fighter is not null)
                client.Account.Character.Fighter.SetReadyForNextTurn();
        }

        [WorldHandler(GameFightTurnFinishMessage.Id)]
        public static void HandleGameFightTurnFinishMessage(WorldClient client, GameFightTurnFinishMessage message) =>
            client.Account!.Character!.Fighter?.EndTurn();

        [WorldHandler(GameActionFightCastRequestMessage.Id)]
        public static void HandleGameActionFightCastRequestMessage(WorldClient client, GameActionFightCastRequestMessage message)
        {
            if (client!.Account!.Character!.Fighter is not null)
                client.Account.Character.Fighter.CastSpell(message.spellId, message.cellId);
        }

        [WorldHandler(ShowCellRequestMessage.Id)]
        public static void HandleShowCellRequestMessage(WorldClient client, ShowCellRequestMessage message) =>
            client.Account!.Character!.Fighter?.Team?.Send(SendShowCellMessage, new object[] { client.Account.Character.Fighter, message.cellId });

        [WorldHandler(GameContextQuitMessage.Id)]
        public static void HandleGameContextQuitMessage(WorldClient client, GameContextQuitMessage message)
        {
            if (client.Account!.Character!.Fighter is not null)
            {
                client.Account.Character.Fighter.Kill(client.Account.Character.Fighter);
                client.Account.Character.QuitFight();
            }
        }

        public static void SendChallengeFightJoinRefusedMessage(WorldClient client, FighterRefusedReasonEnum reason) =>
            client.Send(new ChallengeFightJoinRefusedMessage(client.Account!.Character!.Id, (sbyte)reason));

        public static void SendGameRolePlayPlayerFightFriendlyRequestedMessage(WorldClient client, Character sender, Character receiver) =>
            client.Send(new GameRolePlayPlayerFightFriendlyRequestedMessage(sender.Id == client.Account!.Character!.Id ? receiver.Id : sender.Id, sender.Id, receiver.Id));

        public static void SendGameRolePlayPlayerFightFriendlyAnsweredMessage(WorldClient client, Character sender, Character receiver, bool accept) =>
            client.Send(new GameRolePlayPlayerFightFriendlyAnsweredMessage(sender.Id == client.Account!.Character!.Id ? receiver.Id : sender.Id, sender.Id, receiver.Id, accept));

        public static void SendGameFightUpdateTeamMessage(WorldClient client, Team team) =>
            client.Send(new GameFightUpdateTeamMessage(team.Fight.Id, team.FightTeamInformations));

        public static void SendGameFightShowFighterMessage(WorldClient client, FightActor fighter) =>
            client.Send(new GameFightShowFighterMessage(fighter.GameFightFighterInformations(client.Account!.Character!.Fighter!)));

        public static void SendGameFightStartingMessage(WorldClient client, FightTypeEnum fightType) =>
            client.Send(new GameFightStartingMessage((sbyte)fightType));

        public static void SendGameFightJoinMessage(WorldClient client, IFight fight) => // TO DO spectate
            client.Send(new GameFightJoinMessage(fight.CanCancelFight, !fight.CanCancelFight, false, fight.Started, fight.GetPlacementTimeLeft(), (sbyte)fight.Type));

        public static void SendGameFightPlacementPossiblePositionsMessage(WorldClient client, Team team) =>
            client.Send(new GameFightPlacementPossiblePositionsMessage(team.Fight.Challengers.PlacementCells.Select(x => x.Id).ToArray(), team.Fight.Defenders.PlacementCells.Select(x => x.Id).ToArray(), (sbyte)team.Side));

        public static void SendGameEntitiesDispositionMessage(WorldClient client, IEnumerable<FightActor> actors) =>
            client.Send(new GameEntitiesDispositionMessage(actors.Where(x => x.GetVisibilityFor(client.Account!.Character!.Fighter!) is not GameActionFightInvisibilityStateEnum.INVISIBLE).Select(x => x.IdentifiedEntityDispositionInformations).ToArray()));

        public static void SendGameFightTurnListMessage(WorldClient client, FightActor[] fighters)
        {
            var ids = new List<int>();
            var deadIds = new List<int>();

            foreach (var fighter in fighters)
            {
                if (!fighter.Alive)
                    deadIds.Add(fighter.Id);

                ids.Add(fighter.Id);
            }

            client.Send(new GameFightTurnListMessage(ids.ToArray(), deadIds.ToArray()));
        }

        public static void SendGameRolePlayShowChallengeMessage(WorldClient client, IFight fight) =>
            client.Send(new GameRolePlayShowChallengeMessage(fight.FightCommonInformations));

        public static void SendGameFightRemoveTeamMemberMessage(WorldClient client, FightActor fighter) =>
            client.Send(new GameFightRemoveTeamMemberMessage(fighter.Team!.Fight.Id, (sbyte)fighter.Team!.Side, fighter.Id));

        public static void SendGameFightStartMessage(WorldClient client) =>
            client.Send(new GameFightStartMessage());

        public static void SendGameFightEndMessage(WorldClient client, IFight fight, FightResultListEntry[] results) =>
            client.Send(new GameFightEndMessage(fight.Duration, fight.AgeBonus, results));

        public static void SendGameFightOptionStateUpdateMessage(WorldClient client, Team team, FightOptionsEnum option) =>
            client.Send(new GameFightOptionStateUpdateMessage(team.Fight.Id, (sbyte)team.Side, (sbyte)option, team.GetOptionState(option)));

        public static void SendGameRolePlayRemoveChallengeMessage(WorldClient client, short fightId) =>
            client.Send(new GameRolePlayRemoveChallengeMessage(fightId));

        public static void SendGameFightSynchronizeMessage(WorldClient client, IEnumerable<FightActor> fighters) =>
            client.Send(new GameFightSynchronizeMessage(fighters.Select(x => x.GameFightFighterInformations(client.Account!.Character!.Fighter!)).ToArray()));

        public static void SendGameFightHumanReadyStateMessage(WorldClient client, CharacterFighter characterFighter) =>
            client.Send(new GameFightHumanReadyStateMessage(characterFighter.Id, characterFighter.Ready));

        public static void SendSequenceStartMessage(WorldClient client, FightSequence sequence) =>
            client.Send(new SequenceStartMessage(sequence.Sender.Id, (sbyte)sequence.Sequence));

        public static void SendSequenceEndMessage(WorldClient client, FightSequence sequence) =>
            client.Send(new SequenceEndMessage((short)sequence.Id, sequence.Sender.Id, (sbyte)sequence.Sequence));

        public static void SendGameFightTurnStartMessage(WorldClient client, IFight fight) =>
            client.Send(new GameFightTurnStartMessage(fight.FighterPlaying!.Id, FightConfig.Instance.TurnTime));

        public static void SendGameFightTurnEndMessage(WorldClient client, IFight fight) =>
            client.Send(new GameFightTurnEndMessage(fight.FighterPlaying!.Id));

        public static void SendGameFightTurnReadyRequestMessage(WorldClient client, IFight fight) =>
            client.Send(new GameFightTurnReadyRequestMessage(fight.FighterPlaying!.Id));

        public static void SendGameActionFightTackledMessage(WorldClient client, FightActor fighter) =>
            client.Send(new GameActionFightTackledMessage((short)Actions.ActionCharacterActionTackled, fighter.Id));

        public static void SendGameActionFightPointsVariationMessage(WorldClient client, Actions action, FightActor source, FightActor target, short delta) =>
            client.Send(new GameActionFightPointsVariationMessage((short)action, source.Id, target.Id, delta));

        public static void SendGameActionFightSpellCastMessage(WorldClient client, SpellCast spellCast)
        {
            var isSilent = spellCast.IsSilentCast(client.Account!.Character!.Fighter!);
            client.Send(new GameActionFightSpellCastMessage((short)Actions.ActionFightCastSpell, spellCast.Caster.Id, isSilent ? (short)-1 : spellCast.TargetedCell.Id,
                (sbyte)spellCast.Critical, isSilent, spellCast.Spell.Id, spellCast.Spell.Level));
        }

        public static void SendGameActionFightCloseCombatMessage(WorldClient client, SpellCast cast, int weaponId)
        {
            var isSilent = cast.IsSilentCast(client.Account!.Character!.Fighter!);
            client.Send(new GameActionFightCloseCombatMessage((short)(cast.Critical switch
            {
                FightSpellCastCriticalEnum.CRITICAL_HIT => Actions.ActionFightCloseCombatCriticalHit,
                FightSpellCastCriticalEnum.CRITICAL_FAIL => Actions.ActionFightCloseCombatCriticalMiss,
                _ => Actions.ActionFightCloseCombat
            }), cast.Caster.Id, isSilent ? (short)-1 : cast.TargetedCell.Id, (sbyte)cast.Critical, isSilent, weaponId));
        }

        public static void SendGameActionFightLifePointsVariationMessage(WorldClient client, FightActor caster, FightActor target, short delta) =>
            client.Send(new GameActionFightLifePointsVariationMessage((short)Actions.ActionCharacterLifePointsLost, caster.Id, target.Id, delta));

        public static void SendGameActionFightDeathMessage(WorldClient client, FightActor killer, FightActor deadActor) =>
            client.Send(new GameActionFightDeathMessage((short)Actions.ActionCharacterDeath, killer.Id, deadActor.Id));

        public static void SendGameActionFightDispellableEffectMessage(WorldClient client, Buff buff) =>
            client.Send(new GameActionFightDispellableEffectMessage(buff.ActionId, buff.Handler.Cast.Caster.Id, buff.AbstractFightDispellableEffect));

        public static void SendGameActionFightDodgePointLossMessage(WorldClient client, FightActor source, FightActor target, short amount, bool ap) =>
            client.Send(new GameActionFightDodgePointLossMessage((short)(ap ? Actions.ActionFightSpellDodgedPa : Actions.ActionFightSpellDodgedPm),
                source.Id, target.Id, amount));

        public static void SendGameActionFightTeleportOnSameMapMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightTeleportOnSameMapMessage((short)Actions.ActionCharacterTeleportOnSameMap, source.Id, target.Id, target.Cell.Id));

        public static void SendGameActionFightSlideMessage(WorldClient client, FightActor source, FightActor target, short startCellId) =>
            client.Send(new GameActionFightSlideMessage((short)Actions.ActionCharacterPush, source.Id, target.Id, startCellId, target.Cell.Id));

        public static void SendGameActionFightExchangePositionsMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightExchangePositionsMessage((short)Actions.ActionCharacterExchangePlaces, source.Id, target.Id, source.Cell.Id, target.Cell.Id));

        public static void SendGameActionFightMarkCellsMessage(WorldClient client, TriggerMark trigger) =>
            client.Send(new GameActionFightMarkCellsMessage((short)(trigger.Type switch
            {
                GameActionMarkTypeEnum.GLYPH =>
                trigger.TriggerType is TriggerType.TurnEnd ? Actions.ActionFightAddGlyphCastingSpellEndturn : Actions.ActionFightAddGlyphCastingSpell,
                _ => Actions.ActionFightAddTrapCastingSpell,
            }), trigger.Caster.Id, trigger.Id, (sbyte)trigger.Type, trigger.GameActionMarkedCell));

        public static void SendGameActionFightTriggerGlyphTrapMessage(WorldClient client, SpellTriggerMark mark, FightActor target) =>
            client.Send(new GameActionFightTriggerGlyphTrapMessage((short)(mark is Trap ? Actions.ActionFightTriggerTrap : Actions.ActionFightTriggerGlyph),
                mark.Caster.Id, mark.Id, target.Id, mark.Spell.Id));

        public static void SendGameActionFightUnmarkCellsMessage(WorldClient client, TriggerMark trigger) =>
             client.Send(new GameActionFightUnmarkCellsMessage((short)Actions.ActionRemoveMarkTrigger, trigger.Caster.Id, trigger.Id));

        public static void SendGameActionFightReduceDamagesMessage(WorldClient client, FightActor source, FightActor target, short amount) =>
            client.Send(new GameActionFightReduceDamagesMessage((short)Actions.ActionCharacterLifeLostModerator, source.Id, target.Id, amount));

        public static void SendGameActionFightReflectSpellMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightReflectSpellMessage((short)Actions.ActionCharacterSpellReflector, source.Id, target.Id));

        public static void SendGameActionFightSummonMessage(WorldClient client, SummonedFighter fighter) =>
            client.Send(new GameActionFightSummonMessage((short)Actions.ActionSummonCreature, fighter.Summoner.Id, fighter.GameFightFighterInformations(client.Account!.Character!.Fighter!)));

        public static void SendGameActionFightInvisibilityMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightInvisibilityMessage((short)Actions.ActionCharacterMakeInvisible, source.Id, target.Id, (sbyte)target.GetVisibilityFor(client.Account!.Character!.Fighter!)));

        public static void SendShowCellMessage(WorldClient client, FightActor source, short cellId) =>
            client.Send(new ShowCellMessage(source.Id, cellId));

        public static void SendGameActionFightDispellEffectMessage(WorldClient client, FightActor source, FightActor target, Buff buff) =>
            client.Send(new GameActionFightDispellEffectMessage((short)Actions.ActionCharacterBoostDispelled, source.Id, target.Id, buff.Id));

        public static void SendGameActionFightChangeLookMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightChangeLookMessage((short)Actions.ActionCharacterChangeLook, source.Id, target.Id, target.Look.GetEntityLook()));

        public static void SendGameActionFightReflectDamagesMessage(WorldClient client, FightActor source, FightActor target, short amount) =>
            client.Send(new GameActionFightReflectDamagesMessage((short)Actions.ActionCharacterLifeLostReflector, source.Id, target.Id, amount));

        public static void SendGameActionFightCarryCharacterMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightCarryCharacterMessage((short)Actions.ActionCarryCharacter, source.Id, target.Id));

        public static void SendGameActionFightDropCharacterMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightDropCharacterMessage((short)Actions.ActionNoMoreCarried, source.Id, target.Id, target.Cell.Id));

        public static void SendGameActionFightThrowCharacterMessage(WorldClient client, FightActor source, FightActor target) =>
            client.Send(new GameActionFightThrowCharacterMessage((short)Actions.ActionNoMoreCarried, source.Id, target.Id, target.Cell.Id));
    }
}
