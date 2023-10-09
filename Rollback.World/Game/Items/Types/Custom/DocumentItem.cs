using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Documents;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemType((short)ItemType.Document)]
    public sealed class DocumentItem : PlayerItem
    {
        private static readonly IReadOnlyDictionary<short, short> _documentsRelations = new Dictionary<short, short>()
        {
            [7939] = 129,
            [7940] = 130,
            [7179] = 91,
            [1567] = 23,
            [2337] = 25,
            [2091] = 45,
            [2092] = 46,
            [2093] = 47,
            [2111] = 48,
            [2112] = 49,
            [8011] = 110,
            [8528] = 143,
            [1625] = 24,
            [1626] = 39,
            [1635] = 114,
            [7529] = 115,
            [7530] = 116,
            [7531] = 117,
            [8316] = 137,
            [2173] = 55,
            [7805] = 118,
            [8317] = 138,
            [8318] = 136,
            [8319] = 135,
            [7313] = 92,
            [410] = 111,
            [8107] = 134,
            [1715] = 20,
            [1716] = 21,
            [1717] = 22,
            [10678] = 146,
            [7359] = 90,
            [7360] = 102,
            [7361] = 103,
            [7362] = 113,
            [7363] = 106,
            [7364] = 107,
            [7365] = 108,
            [966] = 109,
            [7367] = 110,
            [7879] = 119,
            [7371] = 104,
            [7372] = 105,
            [7387] = 112,
            [7899] = 120,
            [7900] = 121,
            [1512] = 17,
            [7413] = 133,
            [7930] = 124,
            [7931] = 125,
            [7932] = 126,
            [7934] = 128,
            [8509] = 142
        };

        public DocumentItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory) { }

        public override bool Use(Cell? targetedCell)
        {
            if (_documentsRelations.TryGetValue(Id, out var documentId))
                DocumentHandler.SendDocumentReadingBeginMessage(_storage.Owner.Client, documentId);

            return false;
        }
    }
}
