using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;

namespace Rollback.World.Game.Looks
{
    public class SubActorLook
    {
        public sbyte Index { get; }

        public SubEntityBindingPointCategoryEnum Category { get; }

        public ActorLook Look { get; }

        public SubActorLook(sbyte index, SubEntityBindingPointCategoryEnum category, ActorLook look)
        {
            Index = index;
            Category = category;
            Look = look;
        }

        public SubEntity SubEntity =>
            new((sbyte)Category, Index, Look.GetEntityLook());

        public override string ToString() =>
            $"{(sbyte)Category}@{Index}={Look}";
    }
}
