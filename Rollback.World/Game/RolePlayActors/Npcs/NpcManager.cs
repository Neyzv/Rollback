using System.Reflection;
using Rollback.Common.DesignPattern.Assemblies;
using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;

namespace Rollback.World.Game.RolePlayActors.Npcs
{
    public sealed class NpcManager : Singleton<NpcManager>
    {
        private readonly Dictionary<short, NpcRecord> _records;
        private readonly Dictionary<string, Func<NpcActionRecord, NpcAction>> _actions;
        private readonly Dictionary<string, Func<NpcReplyRecord, NpcReply>> _replies;

        public NpcManager()
        {
            _records = new();
            _actions = new();
            _replies = new();
        }

        [Initializable(InitializationPriority.LowLevelDatasManager, "Npcs")]
        public void Initialize()
        {
            var npcActionType = typeof(NpcAction);
            var npcReplyType = typeof(NpcReply);

            Logger.Instance.Log("\tLoading actions and replies handlers...");
            foreach (var (type, attributes) in from assembly in AssemblyManager.Instance.Assemblies
                                               from type in assembly.GetTypes()
                                               let attributes = type.GetCustomAttributes<IdentifierAttribute>(false)
                                               where attributes.Any() && type.IsClass && !type.IsAbstract
                                               select (type, attributes))
            {
                if (type.IsSubclassOf(npcActionType))
                {
                    var constructor = type.GetConstructor(new[] { typeof(NpcActionRecord) });

                    if (constructor is not null)
                    {
                        var npcActionFactory = (NpcActionRecord record) => (NpcAction)constructor.Invoke(new[] { record });

                        foreach (var attribute in attributes)
                            if (attribute.Identifier is string identifier)
                                if (!_actions.TryAdd(identifier, npcActionFactory))
                                    Logger.Instance.LogError(msg: $"Found two npc action with alias {identifier}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find a valid constructor for {type.Name}...");
                }
                else if (type.IsSubclassOf(npcReplyType))
                {
                    var constructor = type.GetConstructor(new[] { typeof(NpcReplyRecord) });

                    if (constructor is not null)
                    {
                        var npcReplyFactory = (NpcReplyRecord record) => { return (NpcReply)constructor.Invoke(new[] { record }); };

                        foreach (var attribute in attributes)
                            if (attribute.Identifier is string identifier)
                                if (!_replies.TryAdd(identifier, npcReplyFactory))
                                    Logger.Instance.LogError(msg: $"Found two npc reply with alias {identifier}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find a valid constructor for {type.Name}...");
                }
            }

            Logger.Instance.Log("\tLoading records...");
            foreach (var record in DatabaseAccessor.Instance.Select<NpcRecord>(NpcRelator.GetNpcs))
                _records[record.Id] = record;

            foreach (var actionRecord in DatabaseAccessor.Instance.Select<NpcActionRecord>(NpcActionRelator.GetNpcActions))
            {
                if (_records.ContainsKey(actionRecord.NpcId))
                {
                    foreach (var itemToSell in DatabaseAccessor.Instance.Select<NpcItemRecord>(string.Format(NpcItemRelator.GetItemsByActionId, actionRecord.Id)))
                        actionRecord.Items[itemToSell.ItemId] = itemToSell;

                    _records[actionRecord.NpcId].ActionsRecords.Add(actionRecord);
                }
                else
                    Logger.Instance.LogWarn($"Can not bind action {actionRecord.Id}, npc {actionRecord.NpcId} not found...");
            }

            foreach (var record in DatabaseAccessor.Instance.Select<NpcReplyRecord>(NpcReplyRelator.GetReplies))
            {
                foreach (var npc in _records.Values.Where(x => x.Messages.ContainsKey(record.MessageId) && x.Replies.ContainsKey(record.ReplyId)))
                    npc.RepliesRecords.Add(record);
            }

            Logger.Instance.Log("\tSpawn npcs...");
            foreach (var spawn in DatabaseAccessor.Instance.Select<NpcSpawnRecord>(NpcSpawnRelator.GetSpawns))
            {
                var map = WorldManager.Instance.GetMapById(spawn.MapId);
                if (map is not null)
                {
                    if (spawn.CellId >= 0 && spawn.CellId <= map.Record.Cells?.Length)
                    {
                        if (spawn.Npc is not null)
                            map.AddNpc(spawn);
                        else
                            Logger.Instance.LogError(msg: $"Can not find npc record {spawn.NpcId} for spawn {spawn.Id}...");
                    }
                    else
                        Logger.Instance.LogError(msg: $"Can not find cell {spawn.CellId} for spawn {spawn.Id}...");
                }
                else
                    Logger.Instance.LogError(msg: $"Can not find map {spawn.MapId} for spawn {spawn.Id}...");
            }
        }

        public static Dictionary<short, int> ParseCSVString(string value)
        {
            var result = new Dictionary<short, int>();

            foreach (var values in value.Split(';'))
            {
                var parsedValues = values.Split(',');

                if (parsedValues.Length is 2)
                {
                    try
                    {
                        result.Add(short.Parse(parsedValues[0]), int.Parse(parsedValues[1]));
                    }
                    catch (Exception e)
                    {
                        Logger.Instance.LogError(e, $"Can not parse value {parsedValues[0]} as a short or {parsedValues[1]} as an integer...");
                    }
                }
            }

            return result;
        }

        public List<NpcAction> GetActions(NpcRecord record)
        {
            var result = new List<NpcAction>();

            foreach (var actionRecord in record.ActionsRecords.OrderBy(x => x.Priority))
            {
                if (_actions.ContainsKey(actionRecord.Action))
                {
                    var action = _actions[actionRecord.Action](actionRecord);

                    if (record.Actions.Contains((byte)action.NpcActionType))
                        result.Add(action);
                    else
                        Logger.Instance.LogWarn($"Npc {record.Id} is not allowed to perform action {actionRecord.Action}...");
                }
                else
                    Logger.Instance.LogWarn($"Could not find npc's action handler with alias {actionRecord.Action}, for npc {record.Id}");
            }

            return result;
        }

        public List<NpcReply> GetReplies(Character character, NpcRecord record, short messageId)
        {
            var result = new List<NpcReply>();

            if (record.Messages.ContainsKey(messageId))
            {
                foreach (var replyRecord in record.RepliesRecords.Where(x => x.MessageId == messageId).OrderBy(x => x.Priority))
                {
                    if (record.Replies.ContainsKey(replyRecord.ReplyId))
                    {
                        if (_replies.ContainsKey(replyRecord.Action))
                        {
                            var reply = _replies[replyRecord.Action](replyRecord);

                            if (reply.CanExecute(character))
                                result.Add(reply);
                        }
                        else
                            Logger.Instance.LogWarn($"Could not find npc's reply handler with alias {replyRecord.Action}, for npc {record.Id}...");
                    }
                    else
                        Logger.Instance.LogWarn($"Npc {record.Id} is not allowed to display reply {replyRecord.ReplyId}...");
                }
            }

            return result;
        }

        public NpcRecord? GetNpcRecordById(short id) =>
            _records.ContainsKey(id) ? _records[id] : default;
    }
}
