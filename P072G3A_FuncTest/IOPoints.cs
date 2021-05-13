using Motion.AdlinkDash;
using Motion.Interfaces;
using Motion.AdlinkAps;
namespace G4A
{
    /// <summary>
    ///     设备 IO 项
    /// </summary>
    public class IoPoints
    {
        private const string ApsControllerName = "ApsController";
        private const string DaskControllerName = "DaskController";

        internal static readonly byte Card230_0 = 0;
        internal static readonly int Card204c_0 = 0;
        public static ApsController ApsController = new ApsController(Card204c_0) { Name = ApsControllerName };
        public static DaskController DaskController = new DaskController(Card230_0) { Name = DaskControllerName };

        #region Static instances

        /// <summary>
        ///   双启动按钮1#
        /// </summary>
        public static IoPoint DI0 = new IoPoint(DaskController, Card230_0, 0, IoModes.Senser);

        /// <summary>
        ///   双启动按钮2#
        /// </summary>
        public static IoPoint DI1 = new IoPoint(DaskController, Card230_0, 1, IoModes.Senser);

        /// <summary>
        ///   停止按钮
        /// </summary>
        public static IoPoint DI2 = new IoPoint(DaskController, Card230_0, 2, IoModes.Senser);

        /// <summary>
        ///   复位按钮
        /// </summary>
        public static IoPoint DI3 = new IoPoint(DaskController, Card230_0, 3, IoModes.Senser);

        /// <summary>
        ///   急停按钮
        /// </summary>
        public static IoPoint DI4 = new IoPoint(DaskController, Card230_0, 4, IoModes.Senser);

        /// <summary>
        ///   安全光幕
        /// </summary>
        public static IoPoint DI5 = new IoPoint(DaskController, Card230_0, 5, IoModes.Senser);

        /// <summary>
        ///   门禁开关
        /// </summary>
        public static IoPoint DI6 = new IoPoint(DaskController, Card230_0, 6, IoModes.Senser);

        /// <summary>
        ///   温控信号
        /// </summary>
        public static IoPoint DI7 = new IoPoint(DaskController, Card230_0, 7, IoModes.Senser);

        /// <summary>
        ///  轴1回原点完成
        /// </summary>
        public static IoPoint DI8 = new IoPoint(DaskController, Card230_0, 8, IoModes.Senser);

        /// <summary>
        ///   轴2回原点完成
        /// </summary>
        public static IoPoint DI9 = new IoPoint(DaskController, Card230_0, 9, IoModes.Senser);

        /// <summary>
        ///   轴3回原点完成
        /// </summary>
        public static IoPoint DI10 = new IoPoint(DaskController, Card230_0, 10, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint DI11 = new IoPoint(DaskController, Card230_0, 11, IoModes.Senser);

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint DI12 = new IoPoint(DaskController, Card230_0, 12, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint DI13 = new IoPoint(DaskController, Card230_0, 13, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint DI14 = new IoPoint(DaskController, Card230_0, 14, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint DI15 = new IoPoint(DaskController, Card230_0, 15, IoModes.Senser);

        /// <summary>
        ///   红灯
        /// </summary>
        public static IoPoint DO0 = new IoPoint(DaskController, Card230_0, 0, IoModes.Signal);

        /// <summary>
        ///   黄灯
        /// </summary>
        public static IoPoint DO1 = new IoPoint(DaskController, Card230_0, 1, IoModes.Signal);

        /// <summary>
        ///  绿灯
        /// </summary>
        public static IoPoint DO2 = new IoPoint(DaskController, Card230_0, 2, IoModes.Signal);

        /// <summary>
        ///   蜂鸣器
        /// </summary>
        public static IoPoint DO3 = new IoPoint(DaskController, Card230_0, 3, IoModes.Signal);

        /// <summary>
        ///   双启动按钮1#灯
        /// </summary>
        public static IoPoint DO4 = new IoPoint(DaskController, Card230_0, 4, IoModes.Signal);

        /// <summary>
        ///   双启动按钮2#灯
        /// </summary>
        public static IoPoint DO5 = new IoPoint(DaskController, Card230_0, 5, IoModes.Signal);

        /// <summary>
        ///   停止按钮灯
        /// </summary>
        public static IoPoint DO6 = new IoPoint(DaskController, Card230_0, 6, IoModes.Signal);

        /// <summary>
        ///   复位按钮灯
        /// </summary>
        public static IoPoint DO7 = new IoPoint(DaskController, Card230_0, 7, IoModes.Signal);

        /// <summary>
        ///   轴1#回原点
        /// </summary>
        public static IoPoint DO8 = new IoPoint(DaskController, Card230_0, 8, IoModes.Signal);

        /// <summary>
        ///   轴2#回原点
        /// </summary>
        public static IoPoint DO9 = new IoPoint(DaskController, Card230_0, 9, IoModes.Signal);

        /// <summary>
        ///   轴3#回原点
        /// </summary>
        public static IoPoint DO10 = new IoPoint(DaskController, Card230_0, 10, IoModes.Signal);

        /// <summary>
        ///  plassma清洗
        /// </summary>
        public static IoPoint DO11 = new IoPoint(DaskController, Card230_0, 11, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint DO12 = new IoPoint(DaskController, Card230_0, 12, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint DO13 = new IoPoint(DaskController, Card230_0, 13, IoModes.Signal);

        /// <summary>
        ///   等离子设备启动
        /// </summary>
        public static IoPoint DO14 = new IoPoint(DaskController, Card230_0, 14, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint DO15 = new IoPoint(DaskController, Card230_0, 15, IoModes.Signal);
        #endregion

        /// <summary>
        ///   X轴报警清除
        /// </summary>
        public static IoPoint DO100 = new IoPoint(ApsController, Card204c_0, 0, IoModes.Signal);
        /// <summary>
        ///   Y轴报警清除
        /// </summary>
        public static IoPoint DO101 = new IoPoint(ApsController, Card204c_0, 1, IoModes.Signal);
        /// <summary>
        ///   Z轴报警清除
        /// </summary>
        public static IoPoint DO102 = new IoPoint(ApsController, Card204c_0, 2, IoModes.Signal);
    }
}