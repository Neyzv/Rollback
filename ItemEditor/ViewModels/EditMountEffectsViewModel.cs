using System.Collections.Generic;
using Rollback.Common.ORM;
using Rollback.World.Database.Mounts;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Types;

namespace ItemEditor.ViewModels
{
    internal sealed class EditMountEffectsViewModel : EditEffectsViewModel<MountRecord, EffectInteger>
    {
        protected override string FindRecordById =>
            "SELECT * FROM mounts WHERE Id = {0}";

        public EditMountEffectsViewModel(DatabaseAccessor accessor)
            : base(accessor) { }

        protected override IEnumerable<EffectBase> GetRecordEffects() =>
            _record!.Effects;

        protected override void AssignBinaryDatas(byte[] effectsBin) =>
            _record!.EffectsBin = effectsBin;
    }
}
