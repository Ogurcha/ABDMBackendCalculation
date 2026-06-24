using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AisMaths;
using AisSysUtils;

namespace AisPcCore.CheckPoint.StGbCrossSection
{

    public enum ais7stGbCSItemMaterial
    { 
        [Description("Сталь")]
        Steel = 0,
        [Description("Бетон")]
        Concrete = 1
    }


    public class ais7stGbCSItemRect
    {

        public ais7StGbCrossSection CrossSection { get; protected set; }

        public Int32 ItemIndex { get { return CrossSection != null ? CrossSection.Items.IndexOf(this) : 0; } }

        public virtual ais7stGbCSItemRect Clone(ais7StGbCrossSection cs)
        {
            return new ais7stGbCSItemRect(cs) { Width = Width, DHeight = DHeight, Height = Height, Ar = Ar, dYr = dYr, Material = Material };
        }

        public virtual Double Z
        {
            get 
            {
                Double z = DHeight + Height2 / 2;
                foreach (ais7stGbCSItemRect rect in CrossSection.Items)
                {
                    if (rect == this) break;
                    z += rect.Height;
                }
                return z/1000.0;
            }
        }

        public ais7stGbCSItemRect(ais7StGbCrossSection cSection)
        {
            CrossSection = cSection;
            DHeight = 0;
        }

        public Double Width { get; set; }

        public Double Height { get; set; }

        public Double Height2 { get { return Height - DHeight; } set { Height = value + DHeight; } }

        public Double DHeight { get; set; }
        public Double DHeight2 { get { return Height - Height2; } set { Height -= DHeight - value; DHeight = value; } }

        public virtual Double A
        {
            get
            {
                Double w = Width * Height2 / Math.Pow(10, 6);
                return w /
                    (Material == ais7stGbCSItemMaterial.Concrete ? CrossSection.nb : 1);
            }
        }

        public virtual Double Saa { get { return A * Z; } }
        public virtual Double Iaa { get { return Saa * Z; } }

        public virtual Double Ico
        {
            get
            {
                return Width / 1000.0 * Math.Pow(Height2 / 1000.0, 3) / 12 /
                    (Material == ais7stGbCSItemMaterial.Concrete ? CrossSection.nb : 1);
            }
        }

        public Double Ar { get; set; }

        public Double ar { get { return Ar * 10000; } set { Ar = value / 10000; } }

        public Double Sr_aa { get { return material == ais7stGbCSItemMaterial.Concrete ? Ar * (Z - Height2 / 2000.0 + dYr / 1000.0) : 0; } }
        public Double Ir_aa { get { return Sr_aa * (Z - Height2 / 2000.0 + dYr / 1000.0); } }

        public Double dYr { get; set; }

        public Boolean Vertical { get { return Height > Width; } }

        public virtual ais7stGbCSItemMaterial Material 
        { 
            get { return material; }
            set { material = value; } 
        }

        private ais7stGbCSItemMaterial material;


        public virtual Boolean CanDelete { get { return !(CrossSection.VerticalItem == this || CrossSection.PlateItem == this); } }

        public virtual Boolean CanMove 
        { 
            get 
            { 
                return CanDelete && !(
                    (CrossSection.UpperItems.Length == 1 && CrossSection.UpperItems[0] == this) ||
                    (CrossSection.LowerItems.Length == 1 && CrossSection.LowerItems[0] == this)); 
            } 
        }
        public override string ToString()
        {
            return String.Format("{3}, размер {0:f0}x{1:f0}, {2}", 
                Vertical ? Height : Width, Vertical ? Width : Height - DHeight, 
                aisEnum.GetEnumDescription(Material).ToLower(),
                Material == ais7stGbCSItemMaterial.Steel ? (Vertical ? "ВЛ" : (CrossSection.UpperItems.Length>0 && CrossSection.UpperItems.Contains(this) ? "ВГЛ" : "НГЛ")) : "Плита");
        }
    }

    public class ais7stGbCSItemCorner : ais7stGbCSItemRect
    {

        public override bool CanDelete { get { return true; } }
        public override bool CanMove { get { return false; } }

        public ais7stGbCSItemCorner(ais7StGbCrossSection cSection)
            : base(cSection)
        {
        }

        public override double A
        {
            get
            {
                return 2 * (Width * H2 + (Height - H2) * H2) / Math.Pow(10, 6);
            }
        }

        public override double Saa
        {
            get
            {
                ais7stGbCSItemRect vi = CrossSection.VerticalItem;
                Location loc = CrossSection.Corners.Where(item => item.Value == this).FirstOrDefault().Key;   
                Int32 sign = loc == Location.Down ? 1 : -1;
                if(vi!=null)
                {
                    Double sh = (Width*H2) / Math.Pow(10, 6), sv = ((Height - H2)*H2) / Math.Pow(10, 6);
                    Double zh = vi.Z - sign * (vi.Height / 2000 - H2 / 2000),
                        zv = vi.Z - sign * (vi.Height / 2000 - (H2 + (Height - H2) / 2) / 1000);
                    return 2 * sh * zh + 2 * sv * zv;
                }
                return 0;
            }
        }

        public override double Iaa
        {
            get
            {
                ais7stGbCSItemRect vi = CrossSection.VerticalItem;
                Location loc = CrossSection.Corners.Where(item => item.Value == this).FirstOrDefault().Key;   
                Int32 sign = loc == Location.Down ? 1 : -1;
                if (vi != null)
                {
                    Double sh = (Width * H2) / Math.Pow(10, 6), sv = ((Height - H2) * H2) / Math.Pow(10, 6);
                    Double zh = vi.Z - sign * (vi.Height / 2000 - H2 / 2000),
                        zv = vi.Z - sign * (vi.Height / 2000 - (H2 + (Height - H2) / 2) / 1000);
                    return 2 * sh * Math.Pow(zh, 2) + 2 * sv * Math.Pow(zv, 2);
                }
                return 0;
            }
        }

        public override ais7stGbCSItemRect Clone(ais7StGbCrossSection cs)
        {
            return new ais7stGbCSItemCorner(cs) { Width = Width, Height = Height, H2 = H2 };
        }


        public enum Location
        {
            [Description("верхние")]
            Up = 0,
            [Description("нижние")]
            Down = 1
        }

        public override ais7stGbCSItemMaterial Material { get { return ais7stGbCSItemMaterial.Steel; } }

        public Double H2 { get; set; }

        public Double Awt { get { return 2 * (Height - H2) * H2 / Math.Pow(10, 6); } }
        public Double As { get { return 2 * Width * H2 / Math.Pow(10, 6); } }

        public override double Ico { get { return (Width / 1000 * Math.Pow(H2 / 1000, 3) + (Height - H2) / 1000 * Math.Pow(H2 / 1000, 3)) / 6; } }

    }

}
