using System.Collections.Generic;
using Rollback.Common.ORM;
using Rollback.World.Database.Items;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Types;

namespace ItemEditor.ViewModels
{
    internal sealed class EditItemViewModel : EditEffectsViewModel<ItemRecord, EffectDice>
    {
        protected override string FindRecordById =>
            "SELECT * FROM items_templates WHERE Id = {0}";

        public EditItemViewModel(DatabaseAccessor accessor)
            : base(accessor) { }

        protected override IEnumerable<EffectBase> GetRecordEffects() =>
            _record!.Effects;

        protected override void AssignBinaryDatas(byte[] effectsBin) =>
            _record!.BinaryEffects = effectsBin;
    }
}
