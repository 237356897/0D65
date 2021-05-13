
namespace Motion.Interfaces
{
    public abstract class Cylinder :Automatic
    {
        public abstract bool HaveAlarmReset { get; }

        public abstract bool OutOriginStatus { get; }

        public abstract bool OutMoveStatus { get; }

        public abstract void InitExecute();

        public abstract void InternalReset();

        public abstract void ManualExecute();

        public abstract void Set();

        public abstract void Reset();
    }
}
