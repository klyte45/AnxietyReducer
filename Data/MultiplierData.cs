using Klyte.Commons.Interfaces;
using System.Xml.Serialization;

namespace Klyte.AnxietyReducer.Data
{

    public class AnxietyData : DataExtensorBase<AnxietyData>
    {
        private readonly byte[] lookup = new byte[] { 0x0, 0x8, 0x4, 0xc, 0x2, 0xa, 0x6, 0xe, 0x1, 0x9, 0x5, 0xd, 0x3, 0xb, 0x7, 0xf, };


        [XmlAttribute("multiplier")]
        public byte AnxietyFactor { get; set; } = 0;

        public bool WillDo(long seed)
        {
            long meanPart = seed & 0xFF;
            return ((lookup[meanPart & 0b1111] << 4) | lookup[meanPart >> 4]) > AnxietyFactor;
        }

        public override string SaveId => "K45_AR_MultiplierData";

    }

}
