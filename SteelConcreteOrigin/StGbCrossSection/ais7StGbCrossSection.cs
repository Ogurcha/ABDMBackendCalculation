using AisPcCore.SfData;

namespace AisPcCore.CheckPoint.StGbCrossSection
{
    internal class ais7StGbCrossSection  
    {
        public List<ais7stGbCSItemRect> Items = new List<ais7stGbCSItemRect>();

        protected ais7CheckPoint_StGb Parent;

        public ais7stGbCSItemRect FocusedObject { get; set; }


        public ais7StGbCrossSection(ais7CheckPoint_StGb parent)
        {
            nb1 = false;    
            Parent = parent;
        }

        public ais7StGbCrossSection Clone()
        {
            ais7StGbCrossSection cs = new ais7StGbCrossSection(Parent) { Es = Es, Ea = Ea, Eb = Eb };
            foreach (ais7stGbCSItemRect rect in Items)
                cs.Add(rect.Clone(cs));
            foreach (KeyValuePair<ais7stGbCSItemCorner.Location, ais7stGbCSItemCorner> item in Corners)
                cs.Corners.Add(item.Key, (ais7stGbCSItemCorner)item.Value.Clone(cs));
            return cs;
        }

        #region Элементы сечения
        public ais7stGbCSItemRect[] UpperItems
        {
            get
            {
                return VerticalItem != null ?
                        Items.Where(item => item.ItemIndex > VerticalItem.ItemIndex && item.Material == ais7stGbCSItemMaterial.Steel).ToArray() :
                        new ais7stGbCSItemRect[0];
            }
        }

        public ais7stGbCSItemRect[] LowerItems
        {
            get
            {
                return VerticalItem != null ?
                        Items.Where(item => item.ItemIndex < VerticalItem.ItemIndex && item.Material == ais7stGbCSItemMaterial.Steel).ToArray() :
                        new ais7stGbCSItemRect[0];
            }
        }

        public void AddUpperItem()
        {
            if (UpperItems.Length == 0)
                Add(new ais7stGbCSItemRect(this) { Width = 300, Height = 20, Material = ais7stGbCSItemMaterial.Steel }, VerticalItem.ItemIndex + 1);
            else Add(new ais7stGbCSItemRect(this) { Width = 300, Height = 20, Material = ais7stGbCSItemMaterial.Steel }, UpperItems.Max(item => item.ItemIndex) + 1);
        }
        public void AddLowerItem()
        {
            if (LowerItems.Length == 0)
                Add(new ais7stGbCSItemRect(this) { Width = 300, Height = 20, Material = ais7stGbCSItemMaterial.Steel }, VerticalItem.ItemIndex);
            else Add(new ais7stGbCSItemRect(this) { Width = 300, Height = 20, Material = ais7stGbCSItemMaterial.Steel }, LowerItems.Min(item => item.ItemIndex));
        }

        public ais7stGbCSItemRect VerticalItem { get { return Items.Where(item => item.Vertical && item.Material == ais7stGbCSItemMaterial.Steel).FirstOrDefault(); } }

        public ais7stGbCSItemRect PlateItem { get { return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).FirstOrDefault(); } }


        #endregion


        public static void Default(ais7StGbCrossSection cs)
        {
            cs.Items.Clear();
            cs.Corners.Clear();
            cs.Items.Add(new ais7stGbCSItemRect(cs) { Material = ais7stGbCSItemMaterial.Steel, Width = 750, Height = 28 });
            cs.Items.Add(DefaultVL(cs));
            cs.Items.Add(new ais7stGbCSItemRect(cs) { Material = ais7stGbCSItemMaterial.Steel, Width = 300, Height = 20 });
            cs.Items.Add(DefaultPlate(cs));
        }

        public static ais7stGbCSItemRect DefaultVL(ais7StGbCrossSection cs)
        {
            return new ais7stGbCSItemRect(cs)
            {
                Material = ais7stGbCSItemMaterial.Steel,
                Width = 12,
                Height = 2400
            };
        }

        public static ais7stGbCSItemRect DefaultPlate(ais7StGbCrossSection cs)
        {
            return new ais7stGbCSItemRect(cs)
            {
                Material = ais7stGbCSItemMaterial.Concrete,
                Width = 5800,
                Height = 350,
                DHeight = 200,
                dYr = 75
            };
        }


        public Dictionary<ais7stGbCSItemCorner.Location, ais7stGbCSItemCorner> Corners = new Dictionary<ais7stGbCSItemCorner.Location, ais7stGbCSItemCorner>();

        internal Double cAwt
        {
            get
            {
                Double ww = 0;
                ww += Corners.ContainsKey(ais7stGbCSItemCorner.Location.Up) ?
                    2 * (Corners[ais7stGbCSItemCorner.Location.Up].Height - Corners[ais7stGbCSItemCorner.Location.Up].H2) * 
                        Corners[ais7stGbCSItemCorner.Location.Up].H2 / Math.Pow(10, 6) :
                    0;
                ww += Corners.ContainsKey(ais7stGbCSItemCorner.Location.Down) ?
                    2 * (Corners[ais7stGbCSItemCorner.Location.Down].Height - Corners[ais7stGbCSItemCorner.Location.Down].H2) *
                        Corners[ais7stGbCSItemCorner.Location.Down].H2 / Math.Pow(10, 6) :
                    0;
                return ww;
            }
        }


        public double MaxX { get { return Items.Count > 0 ? Items.Max(item => item.Width) / 2 : 0; } }
        public double MinX { get { return Items.Count > 0 ? -Items.Max(item => item.Width) / 2 : 0; } }
        public double MinY { get { return 0; } }
        public double MaxY { get { return Items.Count > 0 ? Items.Sum(item => item.Height) : 0; } }
        public double MinZ { get { return 0; } }
        public double MaxZ { get { return 0; } }




        #region Характеристики сечения

        public Double Eb { get; set; }
        public Double Es { get; set; }
        public Double Ea { get; set; }
        public Double nb { get { return Eb > 0 ? (Convert.ToInt16(nb1) + 1) * Es / Eb : 1; } }
        public Double na { get { return Ea > 0 ? (Convert.ToInt16(nb1) + 1) * Es / Ea : 1; } }

        public Boolean nb1 { get; set; }

        public Double Ab
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Sum(item => item.A) + Ar : 0;
            }
        }
        public Double As
        {
            get
            {
                Double a = Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Sum(item => item.A) : 0;
                a += Corners.Sum(item => item.Value.A);
                return a;
            }
        }

        public Double Ar
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Sum(item => item.Ar) : 0;
            }
        }

        public Double Sb_aa
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Sum(item => item.Saa) + Sr_aa : 0;
            }
        }
        public Double Ss_aa
        {
            get
            {
                Double s = Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Sum(item => item.Saa) : 0;
                return Corners.Sum(item => item.Value.Saa) + s;
            }
        }

        public Double Sr_aa
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Sum(item => item.Sr_aa) : 0;
            }
        }

        public Double Sstb_aa { get { return Sb_aa + Ss_aa; } }



        public Double Ib_aa
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Sum(item => item.Iaa + item.Ir_aa) : 0;
            }
        }
        public Double Ir_aa
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Sum(item => item.Ir_aa) : 0;
            }
        }
        public Double Is_aa
        {
            get
            {
                Double i = Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Sum(item => item.Iaa) : 0;
                return i + Corners.Sum(item => item.Value.Iaa);
            }
        }
        public Double Istb_aa { get { return Ib_aa + Is_aa; } }




        public Double Ib_co
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Concrete).Sum(item => item.Ico) : 0;
            }
        }
        public Double Is_co
        {
            get
            {
                Double i = Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Sum(item => item.Ico) : 0;
                return i + cIs_co;
            }
        }

        internal Double cIs_co
        {
            get 
            {
                Double s = 0;
                if (Corners.ContainsKey(ais7stGbCSItemCorner.Location.Down)) s += Corners[ais7stGbCSItemCorner.Location.Down].Ico;
                if (Corners.ContainsKey(ais7stGbCSItemCorner.Location.Up)) s += Corners[ais7stGbCSItemCorner.Location.Up].Ico;

                return s;
            }
        }

        public Double Istb_co { get { return Ib_co + Is_co; } }



        public Double Ast { get { return As + Ar; } }
        public Double Astb { get { return Ab + As; } }


        public Double hsb
        {
            get
            {
                return Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel).Sum(item => item.Height) / 1000.0 : 0;
            }
        }


        public double Awt
        {
            get
            {
                Double a2 = Corners != null && Corners.Count == 2 ? Corners.Sum(item => item.Value.Awt) : 0.0;    
                Double a = Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Vertical).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Vertical).Sum(item => item.A) + a2 : 0;
                return a + cAwt;
            }
        }

        public double tf1
        {
            get
            {
                return VerticalItem != null && Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Z < VerticalItem.Z).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Z < VerticalItem.Z).Sum(item => item.Height) :
                    0;
            }
        }

        internal const Double minT = 0.001;
        internal const Double minA = minT * minT;

        public double As1
        {
            get
            {
                Double a = VerticalItem != null && Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Z < VerticalItem.Z).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Z < VerticalItem.Z).Sum(item => item.A) :
                    minA;
                return a + (Corners.ContainsKey(ais7stGbCSItemCorner.Location.Down) ? Corners[ais7stGbCSItemCorner.Location.Down].A : 0);
            }
        }
        public double As2
        {
            get
            {
                Double a = VerticalItem != null && Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Z > VerticalItem.Z).Count() > 0 ?
                    Items.Where(item => item.Material == ais7stGbCSItemMaterial.Steel && item.Z > VerticalItem.Z).Sum(item => item.A) :
                    minA;
                return a + (Corners.ContainsKey(ais7stGbCSItemCorner.Location.Up) ? Corners[ais7stGbCSItemCorner.Location.Up].A : 0);
            }
        }


        #endregion


        #region
        public Double Zs1_s { get { return Ss_aa / As; } }
        public Double Zs2_s { get { return hsb - Zs1_s; } }
        public Double Abn { get { return Ab - Ar; } }
        public Double Zbr { get { return Sb_aa / Ab; } }   
        public Double Zstb { get { return Sstb_aa / Astb; } }

        public Double Zs1_st { get { return (Ss_aa + Sr_aa) / (As + Ar); } }
        public Double Zb_st { get { return Zbr - Zs1_st; } }

        public Double Zb_stb { get { return Zbr - Zstb; } }
        public Double Zbs { get { return Zbr - Zs1_s; } }
        public Double Is { get { return Is_aa + Is_co - Zs1_s * Ss_aa; } }
        public Double Istb { get { return Istb_aa + Istb_co - Zstb * Sstb_aa; } }
        public Double Wb_stb { get { return Istb / Zb_stb; } }
        public Double Ws1_s { get { return Is / Zs1_s; } }
        public Double Ws2_s { get { return Is / Zs2_s; } }
        public Double Wbs { get { return Is / Zbs; } }
        public Double Zstb_shr { get { return Sstb_aa / Astb; } }
        public Double Z { get { return -Zbr + Zstb_shr; } }
        public Double Zst_stb { get { return Zstb_shr - Zs1_st; } }
        public Double Istb_shr { get { return Istb_aa + Istb_co - Zstb_shr * Sstb_aa; } }
        public Double Sshr { get { return Ast * Zst_stb; } }


        #endregion


        #region Реализация Iais7OpenGlObject


        public bool IsVisible() { return true; }
        public string ObjectName() { return "Сталежелезобетонное сечение"; }


        #endregion


        public List<String> Messages
        {
            get
            {
                List<String> retV = new List<string>();
                if (Items.Count == 0) retV.Add("Не задан конструктив сталежелезобетонного сечения");
                else
                {
                    if (VerticalItem == null) retV.Add("Отсутствует вертикальный лист");
                    if (PlateItem == null) retV.Add("Отсутствует плита");
                    if (UpperItems.Length == 0 && Corners.Count == 0) retV.Add("Отсутствуют верхние горизонтальные листы при отсутствующих уголках");
                    if (LowerItems.Length == 0 && Corners.Count == 0) retV.Add("Отсутствуют нижние горизонтальные листы при отсутствующих уголках");
                }
                return retV;
            }
        }


        public void Add(ais7stGbCSItemRect rect) { Add(rect, -1); }
        public void Add(ais7stGbCSItemRect rect, Int32 index) { if (index == -1) Items.Add(rect); else Items.Insert(index, rect); }
        internal void AddCorner(ais7stGbCSItemCorner corner, ais7stGbCSItemCorner.Location lc)
        {
            if (Corners.ContainsKey(lc)) Corners.Remove(lc);
            Corners.Add(lc, corner);
        }

        
        public void SaveToCSV(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Create);
            StreamWriter s = new StreamWriter(file);
            foreach (ais7stGbCSItemRect rect in Items)
                s.WriteLine(String.Format("i;{0:f4};{1:f4};{2:f4};{3}", rect.Width, rect.Height, rect.DHeight, (Int16)rect.Material));

            foreach (ais7stGbCSItemCorner.Location lc in Enum.GetValues(typeof(ais7stGbCSItemCorner.Location)))
                if (Corners.ContainsKey(lc))
                    s.WriteLine(String.Format("c;{0:f4};{1:f4};{2:f4};{3}", Corners[lc].Width, Corners[lc].Height, Corners[lc].H2, (Int16)lc));

            s.Close();
            file.Close();
        }
    }
}
