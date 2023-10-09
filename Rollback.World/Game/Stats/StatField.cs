using Rollback.Protocol.Types;

namespace Rollback.World.Game.Stats
{
    public class StatField
    {
        protected readonly StatsData _stats;

        private short _base;
        public short Base
        {
            get => _base;
            set
            {
                var old = _base;
                _base = value;
                Update((short)(_base - old));
            }
        }

        private short _additional;
        public short Additional
        {
            get => _additional;
            set
            {
                var old = _additional;
                _additional = value;
                Update((short)(_additional - old));
            }
        }

        private short _equipments;
        public short Equipments
        {
            get => _equipments;
            set
            {
                var old = _equipments;
                _equipments = value;
                Update((short)(_equipments - old));
            }
        }

        private short _alignmentBonus;
        public short AlignmentBonus
        {
            get => _alignmentBonus;
            set
            {
                var old = _alignmentBonus;
                _alignmentBonus = value;
                Update((short)(_alignmentBonus - old));
            }
        }

        private short _context;
        public short Context
        {
            get => _context;
            set
            {
                var old = _context;
                _context = value;
                Update((short)(_context - old));
            }
        }

        public virtual short TotalWithOutContext =>
            (short)(Base + Additional + Equipments + AlignmentBonus);

        public virtual short Total =>
            (short)(TotalWithOutContext + Context);

        public CharacterBaseCharacteristic CharacterBaseCharacteristic =>
            new((short)(Base + Additional), Equipments, AlignmentBonus, Context);

        public StatField(StatsData stats) =>
            _stats = stats;

        protected virtual void Update(short delta) { }
    }
}
