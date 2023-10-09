using System.Collections.Generic;
using Rollback.Common.ORM;
using Rollback.World.Database.Pets;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Types;

namespace ItemEditor.ViewModels
{
    internal sealed class EditPetItemViewModel : EditEffectsViewModel<PetFoodRecord, EffectInteger>
    {
        protected override string FindRecordById =>
            "SELECT * FROM pets_foods WHERE Id = {0}";

        public EditPetItemViewModel(DatabaseAccessor accessor)
            : base(accessor) { }

        protected override IEnumerable<EffectBase> GetRecordEffects()
        {
            yield return _record!.Effect;
        }

        protected override void AssignBinaryDatas(byte[] effectsBin) =>
            _record!.EffectBin = effectsBin;
    }
}
