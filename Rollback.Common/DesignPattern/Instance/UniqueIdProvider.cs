using System.Collections.Concurrent;

namespace Rollback.Common.DesignPattern.Instance
{
    public class UniqueIdProvider
    {
        private readonly ConcurrentQueue<int> _freeIds;
        private int _highestId;

        public UniqueIdProvider(int highestId = default) =>
            (_freeIds, _highestId) = (new(), highestId);

        public UniqueIdProvider(HashSet<int> usedIds)
        {
            _freeIds = new ConcurrentQueue<int>();

            if (usedIds.Count is not 0)
                _highestId = usedIds.Last();

            for (var i = 1; i < _highestId; i++)
            {
                if (!usedIds.Contains(i))
                    Free(i);
            }
        }

        public void SetHighestId(int highestId) =>
            _highestId = highestId;

        public int Next() =>
            Interlocked.Increment(ref _highestId);

        public int Generate() =>
            _freeIds.IsEmpty || !_freeIds.TryDequeue(out int result) ? Next() : result;

        public void Free(int id) =>
            _freeIds.Enqueue(id);
    }
}
