using Motion.AdlinkDash;
using Motion.Interfaces;

namespace P072G3A_FuncTest
{
    /// <summary>
    ///     设备 IO 项
    /// </summary>
    public class IoPoints
    {
        private const string Card204c_0Name = "Card204c_0";
        private const string DaskControllerName = "DaskController";

        internal static readonly byte Card7230_0 = 0;
        internal static readonly int card204c_0 = 0;
        public static ApsController Card204c_0 = new ApsController(card204c_0) { Name = Card204c_0Name };
        public static DaskController DaskController = new DaskController(Card7230_0) { Name = DaskControllerName };

        #region Card204C
        /// <summary>
        ///   停止按钮
        /// </summary>
        public static IoPoint TDI0 = new IoPoint(Card204c_0, card204c_0, 8, IoModes.Senser);
        /// <summary>
        ///   暂停按钮
        /// </summary>
        public static IoPoint TDI1 = new IoPoint(Card204c_0, card204c_0, 9, IoModes.Senser);
        /// <summary>
        ///   复位按钮
        /// </summary>
        public static IoPoint TDI2 = new IoPoint(Card204c_0, card204c_0, 10, IoModes.Senser);
        /// <summary>
        ///   急停按钮
        /// </summary>
        public static IoPoint TDI3 = new IoPoint(Card204c_0, card204c_0, 11, IoModes.Senser);
        /// <summary>
        ///   门禁
        /// </summary>
        public static IoPoint TDI4 = new IoPoint(Card204c_0, card204c_0, 12, IoModes.Senser);
        /// <summary>
        ///   左通道启动
        /// </summary>
        public static IoPoint TDI5 = new IoPoint(Card204c_0, card204c_0, 13, IoModes.Senser);
        /// <summary>
        ///   右通道启动
        /// </summary>
        public static IoPoint TDI6 = new IoPoint(Card204c_0, card204c_0, 14, IoModes.Senser);
        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint TDI7 = new IoPoint(Card204c_0, card204c_0, 15, IoModes.Senser);
        /// <summary>
        ///   左通道黑场升降气缸上感应
        /// </summary>
        public static IoPoint TDI8 = new IoPoint(Card204c_0, card204c_0, 16, IoModes.Senser);
        /// <summary>
        ///   左通道黑场升降气缸下感应
        /// </summary>
        public static IoPoint TDI9 = new IoPoint(Card204c_0, card204c_0, 17, IoModes.Senser);
        /// <summary>
        ///   右通道黑场升降气缸上感应
        /// </summary>
        public static IoPoint TDI10 = new IoPoint(Card204c_0, card204c_0, 18, IoModes.Senser);
        /// <summary>
        ///   右通道黑场升降气缸下感应
        /// </summary>
        public static IoPoint TDI11 = new IoPoint(Card204c_0, card204c_0, 19, IoModes.Senser);
        /// <summary>
        ///   白场升降气缸上感应
        /// </summary>
        public static IoPoint TDI12 = new IoPoint(Card204c_0, card204c_0, 20, IoModes.Senser);
        /// <summary>
        ///   白场升降气缸下感应
        /// </summary>
        public static IoPoint TDI13 = new IoPoint(Card204c_0, card204c_0, 21, IoModes.Senser);
        /// <summary>
        ///   左通道平移气缸缩回感应
        /// </summary>
        public static IoPoint TDI14 = new IoPoint(Card204c_0, card204c_0, 22, IoModes.Senser);
        /// <summary>
        ///   左通道平移气缸伸出感应
        /// </summary>
        public static IoPoint TDI15 = new IoPoint(Card204c_0, card204c_0, 23, IoModes.Senser);

        /// <summary>
        ///   三色灯红灯
        /// </summary>
        public static IoPoint TDO0 = new IoPoint(Card204c_0, card204c_0, 8, IoModes.Signal);
        /// <summary>
        ///   三色灯黄灯
        /// </summary>
        public static IoPoint TDO1 = new IoPoint(Card204c_0, card204c_0, 9, IoModes.Signal);
        /// <summary>
        ///   三色灯绿灯
        /// </summary>
        public static IoPoint TDO2 = new IoPoint(Card204c_0, card204c_0, 10, IoModes.Signal);
        /// <summary>
        ///   蜂鸣器
        /// </summary>
        public static IoPoint TDO3 = new IoPoint(Card204c_0, card204c_0, 11, IoModes.Signal);
        /// <summary>
        ///   停止按钮灯
        /// </summary>
        public static IoPoint TDO4 = new IoPoint(Card204c_0, card204c_0, 12, IoModes.Signal);
        /// <summary>
        ///   暂停按钮灯
        /// </summary>
        public static IoPoint TDO5 = new IoPoint(Card204c_0, card204c_0, 13, IoModes.Signal);
        /// <summary>
        ///   复位按钮灯
        /// </summary>
        public static IoPoint TDO6 = new IoPoint(Card204c_0, card204c_0, 14, IoModes.Signal);
        /// <summary>
        ///   Z轴刹车
        /// </summary>
        public static IoPoint TDO7 = new IoPoint(Card204c_0, card204c_0, 15, IoModes.Signal);
        /// <summary>
        ///   左通道黑场升降气缸上
        /// </summary>
        public static IoPoint TDO8 = new IoPoint(Card204c_0, card204c_0, 16, IoModes.Signal);
        /// <summary>
        ///   左通道黑场升降气缸下
        /// </summary>
        public static IoPoint TDO9 = new IoPoint(Card204c_0, card204c_0, 17, IoModes.Signal);
        /// <summary>
        ///   右通道黑场升降气缸下
        /// </summary>
        public static IoPoint TDO10 = new IoPoint(Card204c_0, card204c_0, 18, IoModes.Signal);
        /// <summary>
        ///   右通道黑场升降气缸上
        /// </summary>
        public static IoPoint TDO11 = new IoPoint(Card204c_0, card204c_0, 19, IoModes.Signal);
        /// <summary>
        ///   白场升降气缸下
        /// </summary>
        public static IoPoint TDO12 = new IoPoint(Card204c_0, card204c_0, 20, IoModes.Signal);
        /// <summary>
        ///   白场升降气缸上
        /// </summary>
        public static IoPoint TDO13 = new IoPoint(Card204c_0, card204c_0, 21, IoModes.Signal);
        /// <summary>
        ///   双色灯绿（OK信号灯）
        /// </summary>
        public static IoPoint TDO14 = new IoPoint(Card204c_0, card204c_0, 22, IoModes.Signal);
        /// <summary>
        ///   双色灯红（NG信号灯）
        /// </summary>
        public static IoPoint TDO15 = new IoPoint(Card204c_0, card204c_0, 23, IoModes.Signal);
        #endregion

        #region Card7230
        /// <summary>
        ///   右通道平移气缸缩回感应
        /// </summary>
        public static IoPoint IDI0 = new IoPoint(DaskController, Card7230_0, 0, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI1 = new IoPoint(DaskController, Card7230_0, 1, IoModes.Senser);

        /// <summary>
        ///   右通道平移气缸伸出感应
        /// </summary>
        public static IoPoint IDI2 = new IoPoint(DaskController, Card7230_0, 2, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI3 = new IoPoint(DaskController, Card7230_0, 3, IoModes.Senser);

        /// <summary>
        ///   气压不足报警
        /// </summary>
        public static IoPoint IDI4 = new IoPoint(DaskController, Card7230_0, 4, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI5 = new IoPoint(DaskController, Card7230_0, 5, IoModes.Senser);

        /// <summary>
        ///   左盖检测
        /// </summary>
        public static IoPoint IDI6 = new IoPoint(DaskController, Card7230_0, 6, IoModes.Senser);

        /// <summary>
        ///   NG料筒
        /// </summary>
        public static IoPoint IDI7 = new IoPoint(DaskController, Card7230_0, 7, IoModes.Senser);

        /// <summary>
        ///  右盖检测
        /// </summary>
        public static IoPoint IDI8 = new IoPoint(DaskController, Card7230_0, 8, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI9 = new IoPoint(DaskController, Card7230_0, 9, IoModes.Senser);

        /// <summary>
        ///   光栅检测
        /// </summary>
        public static IoPoint IDI10 = new IoPoint(DaskController, Card7230_0, 10, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI11 = new IoPoint(DaskController, Card7230_0, 11, IoModes.Senser);

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint IDI12 = new IoPoint(DaskController, Card7230_0, 12, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI13 = new IoPoint(DaskController, Card7230_0, 13, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI14 = new IoPoint(DaskController, Card7230_0, 14, IoModes.Senser);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI15 = new IoPoint(DaskController, Card7230_0, 15, IoModes.Senser);

        /// <summary>
        ///   左通道平移气缸缩回
        /// </summary>
        public static IoPoint IDO0 = new IoPoint(DaskController, Card7230_0, 0, IoModes.Signal);

        /// <summary>
        ///   左通道平移气缸伸出
        /// </summary>
        public static IoPoint IDO1 = new IoPoint(DaskController, Card7230_0, 1, IoModes.Signal);

        /// <summary>
        ///  右通道平移气缸伸出
        /// </summary>
        public static IoPoint IDO2 = new IoPoint(DaskController, Card7230_0, 2, IoModes.Signal);

        /// <summary>
        ///   右通道平移气缸缩回
        /// </summary>
        public static IoPoint IDO3 = new IoPoint(DaskController, Card7230_0, 3, IoModes.Signal);

        /// <summary>
        ///   左边测试OK信号灯
        /// </summary>
        public static IoPoint IDO4 = new IoPoint(DaskController, Card7230_0, 4, IoModes.Signal);

        /// <summary>
        ///   左边测试NG信号灯
        /// </summary>
        public static IoPoint IDO5 = new IoPoint(DaskController, Card7230_0, 5, IoModes.Signal);

        /// <summary>
        ///   右边测试OK信号灯
        /// </summary>
        public static IoPoint IDO6 = new IoPoint(DaskController, Card7230_0, 6, IoModes.Signal);

        /// <summary>
        ///   右边测试NG信号灯
        /// </summary>
        public static IoPoint IDO7 = new IoPoint(DaskController, Card7230_0, 7, IoModes.Signal);

        /// <summary>
        ///   左边新增IR上电
        /// </summary>
        public static IoPoint IDO8 = new IoPoint(DaskController, Card7230_0, 8, IoModes.Signal);

        /// <summary>
        ///   右边新增IR上电
        /// </summary>
        public static IoPoint IDO9 = new IoPoint(DaskController, Card7230_0, 9, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDO10 = new IoPoint(DaskController, Card7230_0, 10, IoModes.Signal);

        /// <summary>
        ///  备用
        /// </summary>
        public static IoPoint IDO11 = new IoPoint(DaskController, Card7230_0, 11, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDO12 = new IoPoint(DaskController, Card7230_0, 12, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDO13 = new IoPoint(DaskController, Card7230_0, 13, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDO14 = new IoPoint(DaskController, Card7230_0, 14, IoModes.Signal);

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDO15 = new IoPoint(DaskController, Card7230_0, 15, IoModes.Signal);
        #endregion

    }
}