using Motion.AdlinkAps;
namespace Motion.Enginee
{
    /// <summary>
    ///     凌华 Adlink 伺服马达驱动轴
    /// </summary>
    public class ServoAxis : ApsAxis
    {
        public ServoAxis(ApsController apsController) : base(apsController)
        {
        }
        /// <summary>
        ///     当量脉冲(如：p/mm，p/度)
        /// </summary>
        /// <remarks>一圈的脉冲数/导程</remarks>
        public double EquivalentPulse { get; set; }
        public override int CurrentPos
        {
            get
            {
                return ApsController.GetCurrentCommandPosition(NoId);
            }
        }
        /// <summary>
        ///     是否原点
        /// </summary>
        public bool IsOrigin
        {
            get { return ApsController.IsOrg(NoId); }
        }

        /// <summary>
        ///     是否到达正限位。
        /// </summary>
        public bool IsPositiveLimit
        {
            get { return ApsController.IsMel(NoId); }
        }

        /// <summary>
        ///     是否到达负限位。
        /// </summary>
        public bool IsNegativeLimit
        {
            get { return ApsController.IsPel(NoId); }
        }
        /// <summary>
        ///     是否报警
        /// </summary>
        public bool IsAlarmed
        {
            get { return ApsController.IsAlm(NoId); }
        }

        /// <summary>
        ///     是否急停
        /// </summary>
        public bool IsEmg
        {
            get { return ApsController.IsEmg(NoId); }
        }
        /// <summary>
        ///     是否到位。
        /// </summary>
        public bool IsInPosition(int pos)
        {
            return ApsController.IsDown(NoId)
                & (ApsController.GetCurrentCommandPosition(NoId) == pos);
        }
    }
}