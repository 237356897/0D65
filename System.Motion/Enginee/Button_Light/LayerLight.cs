using Motion.Interfaces;

namespace Motion.Enginee
{
    /// <summary>
    ///     多层指示灯
    /// </summary>
    public class LayerLight : Automatic
    {
        public LayerLight(IoPoint greenIo, IoPoint yellowIo, IoPoint redIo, IoPoint speakerIo)
        {
            GreenLamp = greenIo;
            YellowLamp = yellowIo;
            RedLamp = redIo;
            Speeker = speakerIo;
        }
        public IoPoint GreenLamp { get; set; }
        public IoPoint YellowLamp { get; set; }
        public IoPoint RedLamp { get; set; }
        public IoPoint Speeker { get; set; }
    }
}