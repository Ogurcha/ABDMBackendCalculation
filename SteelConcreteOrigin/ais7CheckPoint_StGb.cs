using System.ComponentModel;
using System.Reflection;
using System.Text;
using AisMaths;
using AisPcCore.CheckPoint.StGbCrossSection;
using AisPcCore.SfData;

namespace AisPcCore.CheckPoint
{
    public class ais7CheckPoint_StGb
    {
        public ais7StGbCrossSection CS;

        private const Double mb = 0.9;

        #region Значения для расчета
        public WorkSign workSign { get; set; }
        public Double M1 { get; set; }

        public Double M2g { get; set; }

        public Double Mp { get; set; }
        public Double Es { get; set; }
        public Double Est { get { return Es; } }

        public Double Rs1 { get; set; }

        public Double Rs2 { get; set; }

        public Double χ1 { get; set; }

        public Double Eb { get; set; }

        public Double Rb { get; set; }
        private Double Rb2 { get { return Rb * mb; } }

        public Double ε_b_lim { get; set; }

        public Double Ea { get; set; }

        public Double Rr { get; set; }

        public PlateType plateType { get; set; }

        public Double ϕ_kr
        { get; set; }

        public Double L { get; set; }

        public Double Sd { get; set; }

        public Double σ_b_kr { get; set; }

        public Double σ_a_kr { get; set; }


        public Double σ_b_shr { get; set; }

        public Double σ_a_shr { get; set; }



        public Double tmax { get; set; }

        public Double σ_b_t { get; set; }

        public Double σ_a_t { get; set; }

        public enum WorkSign
        {
            [Description("На положительный момент")]
            Plus,
            [Description("На отрицательный момент")]
            Minus
        }

        public enum PlateType
        {
            [Description("Сборная")]
            Combined,
            [Description("Монолитная")]
            Monolithic
        }

        public PropertyInfo[] ExtendedPropList
        {
            get
            {
                return new PropertyInfo[]
                {
                    this.GetType().GetProperty("workSign"),


                    this.GetType().GetProperty("Es"),
                    this.GetType().GetProperty("Ea"),
                    this.GetType().GetProperty("Eb"),
                    this.GetType().GetProperty("ϕ_kr"),
                    this.GetType().GetProperty("ε_b_lim"),
                    this.GetType().GetProperty("Rs1"),
                    this.GetType().GetProperty("Rs2"),
                    this.GetType().GetProperty("Rr"),
                    this.GetType().GetProperty("Rb"),
                    this.GetType().GetProperty("tmax"),

                    this.GetType().GetProperty("plateType"),
                    this.GetType().GetProperty("L"),
                    this.GetType().GetProperty("Sd"),

                    this.GetType().GetProperty("σ_b_kr"),
                    this.GetType().GetProperty("σ_a_kr"),
                    this.GetType().GetProperty("σ_b_shr"),
                    this.GetType().GetProperty("σ_a_shr"),
                    this.GetType().GetProperty("σ_b_t"),
                    this.GetType().GetProperty("σ_a_t"),

                    this.GetType().GetProperty("M1"),
                    this.GetType().GetProperty("M2g"),
                    this.GetType().GetProperty("χ1"),
                    this.GetType().GetProperty("Mp"),
                };
            }
        }

        #endregion



        public ais7CheckPoint_StGb() { }
        public ais7CheckPoint_StGb(CpSubType cpSubType)
        {
            CS = new ais7StGbCrossSection(this);
            Es = 206000;
            Rs1 = 350;
            Rs2 = 350;
            Eb = 32500;
            Rb = 15.50;
            ε_b_lim = 0.0016;
            Ea = 206000;
            Rr = 350;
            this.χ1 = 1;
            L = 0;
        }





        private CnValue GetControlValue(Double inValue, ais7PassTypeEnum restriction, int calculateCase)
        {
            Dictionary<ais7SfUse, Double> values = new Dictionary<ais7SfUse, double>();
            values.Add(ais7SfUse.Single, inValue);
            return GetStGbControlValue(values, restriction, calculateCase);
        }



        public class CnValueStGb : CnValue
        {
            public String name { get; protected set; }
            public Double σ_I { get; internal set; }
            public Double σ_II { get; internal set; }
            public Double σ_tot { get; internal set; }
            public Double σ_pred { get; internal set; }
            public Double σ_lim => (σ_pred - σ_tot) / σ_pred;

            public Boolean _4Steel { get; set; } = true;

            public CnValueStGb(Double σ_tot, Double σ_pred, String name) : base(σ_tot / σ_pred)
            {
                this.σ_tot = σ_tot;
                this.σ_pred = σ_pred;
                this.name = name;
            }
            #region Контролируемые значения для формирования отчетов (Ращепкин)
            public aisReportValues_StGb ReportValues { get; internal set; } = new aisReportValues_StGb();
            #endregion

        }

        public CnValue GetStGbControlValue(Dictionary<ais7SfUse, Double> values, ais7PassTypeEnum restriction, int CalculateCase = 0)
        {
            var repVal = new aisReportValues_StGb();

            Double M1_2 = M1 * 9.81 / 1000;
            Double M2_2 = M2g * 9.81 / 1000;
            Double value = values[ais7SfUse.Single] * 9.81 / 1000;                
            if (restriction == ais7PassTypeEnum.NoLimit) value += Mp * 9.81 / 1000.0;         
            CS.Eb = Eb;
            CS.Es = Es;
            CS.Ea = Ea;

            ais7StGbCrossSection csConcrete = CS.Clone();
            csConcrete.Items.RemoveAll(item => item.Material == ais7stGbCSItemMaterial.Steel);
            foreach (ais7stGbCSItemRect rect in csConcrete.Items) { rect.Height -= rect.DHeight; rect.DHeight = 0; rect.Ar = 0; }
            csConcrete.Corners.Clear();
            Double Ib = csConcrete.Ib_aa + csConcrete.Ib_co - (csConcrete.Sb_aa / csConcrete.Ab) * csConcrete.Sb_aa;

            Double Ist = (CS.Ir_aa + CS.Is_aa) + CS.Is_co - CS.Zs1_s * (CS.Ss_aa + CS.Sr_aa);

            repVal.Tb02.Применимость_метода_тонкой_плиты = (Eb * Ib < 0.2 * Est * CS.Is);

            if (Eb * Ib > 0.2 * Est * CS.Is) return new CnValue(0.1);              

            Double σ_b1 = M2_2 / (CS.nb * CS.Wb_stb);
            repVal.Tb05.σb1 = σ_b1;  
            repVal.Tb05.Учет_ползучести = false; 
            if (σ_b1 > 0.2 * Rb)        
            {
                repVal.Tb05.Учет_ползучести = true;  
                repVal.Tb05.σb_cr = σ_b_kr;  
                repVal.Tb05.σr_cr = σ_a_kr;  
            }
            repVal.Tb06.Учет_ползучести = repVal.Tb05.Учет_ползучести;

            {
                double cn = aisMathUtils.InterpolAr(new double[11] { 27000, 28500, 30000, 31500, 32500, 34500, 36000, 37500, 39000, 39500, 40000 },
                    new double[11] { 115, 107, 100, 92, 84, 75, 67, 55, 50, 41, 39 }, Eb);
                Double Zb_st = CS.Zbr - CS.Zs1_st;
                Double v = (CS.Ab - CS.Ar) * (1 / CS.Ast + Math.Pow(Zb_st, 2) / Ist);
                double ksi1 = 1.0;
                double ksi2 = 1.0;
                var topBeltWidth = CS.Items.Where(itm => !itm.Vertical && itm.Z > CS.Z).FirstOrDefault()?.Width ?? 0.0;
                double x = (CS.PlateItem.Width + CS.PlateItem.Height2 - topBeltWidth) / (CS.PlateItem.Width * CS.PlateItem.Height2);
                x *= 1000;
                double ksi3 = -4.92770E-10 * Math.Pow(x, 5) + 5.54609E-8 * Math.Pow(x, 4) + 1.95547E-6 * Math.Pow(x, 3) - 5.10810E-4 * x * x + 0.0300060 * x + 0.510542;
                if (x > 80) ksi3 = 1.3;
                double ksi4 = 1.0;
                double clim = cn * ksi1 * ksi2 * ksi3 * ksi4;
                if (Math.Abs(ϕ_kr) < 1e-3)
                    ϕ_kr = 1.1 * Eb * clim * 1e-6;
                Double ϕ_kr_d = ϕ_kr + Eb * Sd / (0.2 * Rb * L);
                Double a = ϕ_kr_d / (0.5 * ϕ_kr_d + v + 1);
                σ_b_kr = a * σ_b1;
                σ_a_kr = a * σ_b1 * v * CS.nb;
                if (CS.Ar < 1e-4) σ_a_kr = 0.0;
                repVal.Tb06.v = v;
                repVal.Tb06.cn = cn;
                repVal.Tb06.ϕ_kr = ϕ_kr;
                repVal.Tb06.ϕ_kr_d = ϕ_kr_d;
                repVal.Tb06.α = a;
                repVal.Tb06.σ_b_kr = σ_b_kr;
                repVal.Tb06.σ_r_kr = σ_a_kr;
                repVal.Tb06.ksi1 = ksi1;
                repVal.Tb06.ksi2 = ksi2;
                repVal.Tb06.ksi3 = ksi3;
                repVal.Tb06.ksi4 = ksi4;
            }

            CS.nb1 = true;        
            Double ε_shr = this.plateType == PlateType.Combined ? 1 * Math.Pow(10, -4) : 2 * Math.Pow(10, -4);
            repVal.Tb02.ε_shr = ε_shr;  
            Double σ_b_shr_ = Math.Abs(σ_b_shr) <= 0.001 ? ε_shr * Eb / 2 * (CS.Ast / CS.Astb + CS.Sshr / CS.Istb_shr * CS.Z) : σ_b_shr;
            Double σ_a_shr_ = Math.Abs(σ_a_shr) <= 0.001 ? ε_shr * Ea * (CS.Ast / CS.Astb + CS.Sshr / CS.Istb_shr * CS.Z - 1) : σ_a_shr;
            CS.nb1 = false;        

            Double At = 0.8 * CS.Awt + 0.3 * CS.As1;
            Double Zb1_stb = CS.tf1 / 1000 + CS.VerticalItem.Height / 1000 - CS.Zstb;
            Double Zs1_stb = CS.Zs1_s;
            Double St = (0.4 * CS.VerticalItem.Height / 1000.0 - 0.8 * CS.Zb_stb) * CS.Awt + 0.3 * CS.As1 * Zs1_stb;
            Double σ_b_t_ = Math.Abs(σ_b_t) <= 0.001 ? 0.00001 * tmax * Eb * (At / CS.Astb + St / CS.Istb * (CS.Zstb - CS.Zbr)) : σ_b_t;
            Double σ_a_t_ = Math.Abs(σ_a_t) <= 0.001 ? 0.00001 * tmax * Ea * (At / CS.Astb + St / CS.Istb * (CS.Zstb - CS.Zbr)) : σ_a_t;

            table2 tt2 = new table2();

            Double M2 = M2_2 + value;
            Double M = M1_2 + M2;
            Boolean e70_b70 = σ_b1 > 0.2 * Rb;        
            Double σb = Math.Min(M2 / CS.nb / CS.Wb_stb - (e70_b70 ? 1 : 0) * σ_b_kr, Rb2),
                σr = CS.Ar > 0.0000001 ? Math.Min(M2 / CS.na / CS.Wb_stb + (e70_b70 ? 1 : 0) * σ_a_kr, Rr) : 0;         

            Double A = CS.As2 / CS.As1;
            Double Nbr = (CS.Abn * σb * CS.nb + CS.Ar * σr);
            Double η1 = tt2.η(workSign, A, Nbr / (Rs1 * CS.As));
            Double η2 = tt2.η(workSign, A, Nbr / (Rs2 * CS.As));

            Double χ3_1 = 1 + η1 * (χ1 - 1), χ3_2 = 1 + η2 * (χ1 - 1);
            Double m1 = Math.Min(Math.Max(1, 1 + (Rb2 - σb) / Rs2 * CS.nb * CS.Abn / CS.As2), 1.2);     
            Double χ4 = Math.Max(1, χ3_2 / m1);

            Double vv1 = (M - CS.Zbs * Nbr) / (χ4 * CS.Ws2_s) - Nbr / CS.As;       
            Double vv2 = (M - CS.Zbs * Nbr) / (χ3_1 * CS.Ws1_s) + Nbr / CS.As;        

            repVal.Tb08.DebugValues.Add(nameof(A), A);
            repVal.Tb08.DebugValues.Add(nameof(η1), η1);
            repVal.Tb08.DebugValues.Add(nameof(η2), η2);
            repVal.Tb08.DebugValues.Add(nameof(χ1), χ1);
            repVal.Tb08.DebugValues.Add(nameof(χ3_1), χ3_1);
            repVal.Tb08.DebugValues.Add(nameof(χ3_2), χ3_2);
            repVal.Tb08.DebugValues.Add(nameof(χ4), χ4);

            repVal.Tb08.DebugValues.Add(nameof(vv1), vv1);
            repVal.Tb08.DebugValues.Add(nameof(vv2), vv2);
            repVal.Tb08.DebugValues.Add(nameof(M), M);
            repVal.Tb08.DebugValues.Add(nameof(Nbr), Nbr);

            repVal.Tb08.DebugValues.Add(nameof(CS.Zbs), CS.Zbs);
            repVal.Tb08.DebugValues.Add(nameof(CS.Is), CS.Is);
            repVal.Tb08.DebugValues.Add(nameof(CS.Zs2_s), CS.Zs2_s);
            repVal.Tb08.DebugValues.Add(nameof(CS.Ws2_s), CS.Ws2_s);
            repVal.Tb08.DebugValues.Add(nameof(CS.Zs1_s), CS.Zs1_s);
            repVal.Tb08.DebugValues.Add(nameof(CS.Ws1_s), CS.Ws1_s);

            repVal.Tb08.DebugValues.Add("x1", Nbr / (Rs1 * CS.As));
            repVal.Tb08.DebugValues.Add("x2", Nbr / (Rs2 * CS.As));

            repVal.Tb08.DebugValues.Add(nameof(CS.As), CS.As);
            repVal.Tb08.DebugValues.Add(nameof(m1), m1);
            repVal.Tb08.DebugValues.Add(nameof(Rb2), Rb2);

            repVal.Tb08.DebugValues.Add(nameof(M2), M2);
            repVal.Tb08.DebugValues.Add(nameof(CS.nb), CS.nb);
            repVal.Tb08.DebugValues.Add(nameof(CS.Wb_stb), CS.Wb_stb);
            repVal.Tb08.DebugValues.Add(nameof(σ_b_kr), σ_b_kr);
            repVal.Tb08.DebugValues.Add(nameof(σ_a_kr), σ_a_kr);
            repVal.Tb08.DebugValues.Add(nameof(CS.Ar), CS.Ar);
            repVal.Tb08.DebugValues.Add(nameof(CS.na), CS.na);
            repVal.Tb08.DebugValues.Add(nameof(Rr), Rr);
            repVal.Tb08.DebugValues.Add(nameof(σr), σr);
            repVal.Tb08.DebugValues.Add(nameof(σb), σb);
            repVal.Tb08.DebugValues.Add(nameof(Rs1), Rs1);
            repVal.Tb08.DebugValues.Add(nameof(Rs2), Rs2);
            repVal.Tb08.DebugValues.Add(nameof(value), value);

            if (Math.Abs(value) > 0.01)
                try { SaveAllDebugInfo(repVal.Tb08); }
                catch { }

            repVal.Tb08.Mv = value;  
            repVal.Tb08.M2 = M2;
            repVal.Tb08.M = M;
            repVal.Tb09.σb = σb;
            repVal.Tb09.mbRb = mb * Rb;
            repVal.Tb09.σr = σr;
            repVal.Tb09.mrRr = 1.0 * Rr;  
            repVal.Tb09.Расчетный_случай = "A";
            repVal.Tb09.Nbr = Nbr;
            repVal.Tb09.σs1_I = M1_2 / CS.Ws1_s;
            repVal.Tb09.σs1_IIg = M2_2 / CS.Istb * (CS.hsb - CS.Zstb);    
            repVal.Tb09.σs1_IIv = value / CS.Istb * (CS.hsb - CS.Zstb);
            repVal.Tb09.σs2_I = M1_2 / CS.Ws2_s;
            repVal.Tb09.σs2_IIg = M2_2 / CS.Istb * CS.Zstb;
            repVal.Tb09.σs2_IIv = value / CS.Istb * CS.Zstb;
            repVal.Tb11.η = η1;   
            repVal.Tb11.æ3 = χ3_1;
            repVal.Tb11.m1 = m1;
            repVal.Tb11.æ4 = χ4;
            repVal.Tb12.σs2 = vv1;
            repVal.Tb12.m1Rs2 = m1 * Rs2;
            repVal.Tb12.σs1 = vv2;
            repVal.Tb12.Rs1 = Rs1;
            repVal.Tb12.value = value;

            List<CnValueStGb> vals = new List<CnValueStGb>();
            vals.Add(new CnValueStGb(m1 * Rs2, vv1, "Первое сочетание, по верхнему поясу")
            {
                σ_I = M1 / CS.Ws2_s,
                σ_II = M2 / CS.Istb * CS.Zstb    
            });
            vals.Add(new CnValueStGb(Rs1, vv2, "Первое сочетание, по нижнему поясу")
            {
                σ_I = M1 / CS.Ws1_s,
                σ_II = M2 / CS.Istb * (CS.hsb - CS.Zstb)    
            });


            if (M2 / CS.nb / CS.Wb_stb - (e70_b70 ? 1 : 0) * σ_b_kr > Rb2)   
            {
                Double K1 = (M - CS.Zbs * Nbr) / CS.Ws2_s, K2 = Rs2 + Nbr / CS.As;   
                Double K;
                if (K1 <= K2) K = 1;
                else
                    if (K1 <= K2 * χ3_2) K = 1 + (K1 - K2) * 0.0009 * Est / Rs2 / (K2 * (χ3_2 - 1));
                else K = 1 + 0.0009 * Est / Rs2;
                vals.Add(new CnValueStGb(this.ε_b_lim, K / Est * ((M2 - CS.Zbs * Nbr) / CS.Wbs - Nbr / CS.As),
                    "Первое сочетание, по бетону плиты")
                { _4Steel = false });

                repVal.Tb09.Расчетный_случай = "Б";  
            }

            CS.nb1 = true;
            M2 = M2_2 + 0.8 * value;
            M = M1_2 + M2;

            σ_b1 = M2_2 / (CS.nb * CS.Wb_stb);
            repVal.Tb14.Z = CS.Zbr - CS.Zstb;
            repVal.Tb14.Zst_stb = CS.Zbs;
            repVal.Tb14.Sshr = CS.Sshr;
            repVal.Tb14.Awt = CS.Awt;
            repVal.Tb14.As1_t = CS.As1;
            repVal.Tb14.Zb1_stb = Zb1_stb;
            repVal.Tb14.St = St;
            repVal.Tb15.Учет_ползучести = false;
            repVal.Tb15.σb1 = σ_b1;

            if (σ_b1 > 0.2 * Rb)        
            {
                Double v = (CS.Ab - CS.Ar) * (1 / CS.Ast + Math.Pow(CS.Zb_st, 2) / Ist);
                Double ϕ_kr_d = ϕ_kr / 2 + Eb * Sd / (0.2 * Rb * L);
                Double a = ϕ_kr_d / (0.5 * ϕ_kr_d + v + 1);
                σ_b_kr = a * σ_b1;
                σ_a_kr = a * σ_b1 * v * CS.nb;
                repVal.Tb15.σb1 = σ_b1;
                repVal.Tb15.Учет_ползучести = true;
                repVal.Tb15.σb_cr = σ_b_kr;
                repVal.Tb15.σr_cr = σ_a_kr;
            }

            σb = Math.Min(M / (CS.nb * CS.Wb_stb) - σ_b_kr - σ_b_shr_ - 0.7 * σ_b_t_, Rb2);
            σr = CS.Ar > 0.0000001 ? Math.Min(M / CS.Wb_stb + σ_a_kr + σ_a_shr_ + 0.7 * σ_a_t_, Rr) : 0;

            Nbr = (CS.Abn * σb * CS.nb + CS.Ar * σr);
            η1 = tt2.η(workSign, A, Nbr / (Rs1 * CS.As));
            η2 = tt2.η(workSign, A, Nbr / (Rs2 * CS.As));

            χ3_1 = 1 + η1 * (χ1 - 1); χ3_2 = 1 + η2 * (χ1 - 1);
            m1 = Math.Min(Math.Max(1, 1 + (Rb2 - σb) / Rs2 * CS.Abn / CS.As2), 1.2);     
            χ4 = Math.Max(1, χ3_2 / m1);

            vv1 = (M - CS.Zbs * Nbr) / (χ4 * CS.Ws2_s) - Nbr / CS.As;       
            vv2 = (M - CS.Zbs * Nbr) / (χ3_1 * CS.Ws1_s) + Nbr / CS.As;        
            vals.Add(new CnValueStGb(m1 * Rs2, vv1, "Второе сочетание, по верхнему поясу")
            {
                σ_I = M1 / CS.Ws2_s,
                σ_II = M2 / CS.Istb * CS.Zstb
            });
            vals.Add(new CnValueStGb(Rs1, vv2, "Второе сочетание, по нижнему поясу")
            {
                σ_I = M1 / CS.Ws1_s,
                σ_II = M2 / CS.Istb * (CS.hsb - CS.Zstb)
            });

            repVal.Tb16.σb_shr = σ_b_shr_;
            double sr_shr = CS.Ar < 1e-4 ? 0.0 : σ_a_shr_;
            repVal.Tb16.σr_shr = sr_shr;
            repVal.Tb17.σb_t = σ_b_t_;
            double sr_t = CS.Ar < 1e-4 ? 0.0 : σ_a_t_;
            repVal.Tb17.σr_t = sr_t;

            repVal.Tb18.Mv = 0.8 * value;  
            repVal.Tb18.M2 = M2;
            repVal.Tb18.M = M;
            repVal.Tb19.σb = σb;
            repVal.Tb19.mbRb = mb * Rb;
            repVal.Tb19.σr = σr;
            repVal.Tb19.mrRr = 1.0 * Rr;
            repVal.Tb19.Расчетный_случай = "A";
            repVal.Tb19.Nbr = Nbr;
            repVal.Tb19.σs1_I = M1_2 / CS.Ws2_s;
            repVal.Tb19.σs1_IIg = M2_2 / CS.Istb * CS.Zstb;
            repVal.Tb19.σs1_IIv = 0.8 * value / CS.Istb * CS.Zstb;
            repVal.Tb19.σs2_I = M1_2 / CS.Ws1_s;
            repVal.Tb19.σs2_IIg = M2_2 / CS.Istb * (CS.hsb - CS.Zstb);
            repVal.Tb19.σs2_IIv = 0.8 * value / CS.Istb * (CS.hsb - CS.Zstb);
            repVal.Tb21.η = η2;   
            repVal.Tb21.æ3 = χ3_1;
            repVal.Tb21.m1 = m1;
            repVal.Tb21.æ4 = χ4;

            repVal.Tb22.σs2 = vv1;
            repVal.Tb22.m1Rs2 = m1 * Rs2;
            repVal.Tb22.σs1 = vv2;
            repVal.Tb22.Rs1 = Rs1;
            repVal.Tb22.value = 0.8 * value;

            CS.nb1 = false;

            if (Math.Truncate(σr - Rr) == 0)
            {
                Double K1 = (M - CS.Zbs * Nbr) / CS.Ws2_s, K2 = Rs2 + Nbr / CS.As;
                Double K;
                if (K1 <= K2) K = 1;
                else
                    if (K1 <= K2 * χ3_2) K = 1 + (K1 - K2) * 0.0009 * Est / Rs2 / (K2 * (χ3_2 - 1));
                else K = 1 + 0.0009 * Est / Rs2;
                vals.Add(new CnValueStGb(this.ε_b_lim, K / Est * ((M2 - CS.Zbs * Nbr) / CS.Wbs - Nbr / CS.As),
                    "Второе сочетание, по бетону плиты")
                { _4Steel = false });
            }
            foreach (var val in vals) val.ReportValues = repVal;

            CnValue result = null;
            var resultLst = new List<CnValueStGb>();
            switch (CalculateCase)
            {
                case 1:   
                    if (vals.Count > 1)
                    {
                        resultLst.Add(vals[0]);
                        resultLst.Add(vals[1]);
                    }
                    break;
                case 2:   
                    if (vals.Count > 3)
                    {
                        resultLst.Add(vals[2]);
                        resultLst.Add(vals[3]);
                    }
                    break;
                default:
                    resultLst = vals;
                    break;
            }
            result = resultLst.OrderBy(i => i.Value).First();
            if (CalculateCase == 2)
                SecondCaseValues = result as CnValueStGb;
            else
                FirstCaseValues = result as CnValueStGb;
            return result;
        }

        public CnValueStGb FirstCaseValues;
        public CnValueStGb SecondCaseValues;

        private void SaveAllDebugInfo(aisReportValues_StGb.Table08 tb)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StGbDebugAll.txt");
            if (!File.Exists(path)) return;

            TextWriter csvf = new StreamWriter(path, true, Encoding.Unicode);
            string line;
            line = $"{tb.GetType().Name}:";
            csvf.WriteLine(line);
            foreach (var item in tb.DebugValues)
            {
                line = $"  {item.Key} = {item.Value}";
                csvf.WriteLine(line);
            }
            csvf.WriteLine("");
            csvf.Flush();
            csvf.Close();
            csvf = null;
        }

        private Double getAvailable(Double cValue, Double newValue) { return Math.Min(cValue, Math.Abs(newValue)); }
        private Double getAvailable(Double newValue) { return Math.Abs(newValue); }



        #region Таблица 2 и все, что с ней связано
        private class table2 : List<ais7CheckPoint_StGb.table2.table2Record>
        {
            public class t2item
            {
                public Double v1 { get; set; }
                public Double v2 { get; set; }
                public t2item(Double v1, Double v2) { this.v1 = v1; this.v2 = v2; }
                public t2item(Double v) { this.v1 = v; this.v2 = v; }

                public Double η(WorkSign sign) { return sign == WorkSign.Plus ? v2 : v1; }
            }

            public class table2Record
            {


                public t2item[] Items { get; set; }

                public Double As { get; set; }

                public Double η(WorkSign sign, Double x)
                {
                    if (x <= 0) return Items[0].η(sign);
                    if (x >= 0.7) return Items[Items.Length - 1].η(sign);
                    Int16 i1 = (Int16)Math.Truncate(x / 0.05);
                    Double x1 = i1 * 0.05, x2 = (i1 + 1) * 0.05, y1 = Items[i1].η(sign), y2 = Items[i1 + 1].η(sign);
                    return (x - x1) / (x2 - x1) * (y2 - y1) + y1;
                }
            }


            public table2()
            {
                this.Add(new table2Record()
                {
                    As = 0,
                    Items = new t2item[] {
                        new t2item(1), new t2item(1, 0.98), new t2item(1, 0.94),
                        new t2item(1, 0.9), new t2item(1, 0.87), new t2item(1, 0.81),
                        new t2item(1, 0.75), new t2item(0.98, 0.67), new t2item(0.96, 0.58),
                        new t2item(0.95, 0.45), new t2item(0.92, 0.28), new t2item(0.88, 0.52),
                        new t2item(0.83, 0.68), new t2item(0.75, 0.76), new t2item(0.63, 0.82) }
                });
                this.Add(new table2Record()
                {
                    As = 0.2,
                    Items = new t2item[] {
                        new t2item(1), new t2item(1, 0.97), new t2item(1, 0.92),
                        new t2item(1.02, 0.87), new t2item(1.03, 0.8), new t2item(1.04, 0.7),
                        new t2item(1.05, 0.57), new t2item(1.06, 0.38), new t2item(1.07, 0.49),
                        new t2item(1.06, 0.61), new t2item(1.05, 0.72), new t2item(1.02, 0.82),
                        new t2item(0.99, 0.91), new t2item(0.9, 0.99), new t2item(0.75, 1.05) }
                });
                this.Add(new table2Record()
                {
                    As = 0.4,
                    Items = new t2item[] {
                        new t2item(1), new t2item(1.04, 0.9), new t2item(1.08, 0.8),
                        new t2item(1.12, 0.67), new t2item(1.14, 0.52), new t2item(1.16, 0.34),
                        new t2item(1.19, 0.53), new t2item(1.2, 0.68), new t2item(1.21, 0.84),
                        new t2item(1.2, 0.98), new t2item(1.18, 1.12), new t2item(1.16, 1.22),
                        new t2item(1.13, 1.3), new t2item(1.09, 1.38), new t2item(1.04, 1.42) }
                });
                this.Add(new table2Record()
                {
                    As = 0.6,
                    Items = new t2item[] {
                        new t2item(1), new t2item(1.1, 0.84), new t2item(1.19, 0.64),
                        new t2item(1.28, 0.4), new t2item(1.35, 0.56), new t2item(1.4, 0.75),
                        new t2item(1.44, 0.95), new t2item(1.46, 1.13), new t2item(1.47, 1.3),
                        new t2item(1.46, 1.45), new t2item(1.45, 1.58), new t2item(1.42, 1.69),
                        new t2item(1.39, 1.76), new t2item(1.35, 1.84), new t2item(1.3, 1.9) }
                });
                this.Add(new table2Record()
                {
                    As = 0.8,
                    Items = new t2item[] {
                        new t2item(1), new t2item(1.2, 0.61), new t2item(1.39, 0.51),
                        new t2item(1.55, 0.84), new t2item(1.7, 1.12), new t2item(1.83, 1.36),
                        new t2item(1.93, 1.6), new t2item(1.98, 1.86), new t2item(2, 2.08),
                        new t2item(2.02, 2.29), new t2item(2.01, 2.47), new t2item(1.99, 2.52),
                        new t2item(1.97, 2.5), new t2item(1.91, 2.46), new t2item(1.84, 2.38) }
                });
                this.Add(new table2Record()
                {
                    As = 1,
                    Items = new t2item[] {
                        new t2item(1), new t2item(1.29), new t2item(1.63),
                        new t2item(2.04), new t2item(2.47), new t2item(2.86),
                        new t2item(3.2), new t2item(3.38), new t2item(3.49),
                        new t2item(3.56), new t2item(3.57), new t2item(3.53),
                        new t2item(3.43), new t2item(3.29), new t2item(3.05) }
                });
            }

            public Double η(WorkSign sign, Double a, Double x)
            {
                a = !Double.IsNaN(a) ? Math.Max(0, Math.Min(1, a)) : 0;
                Int32 i1 = (Int32)Math.Truncate(a / 0.2);
                Double y1 = this[Math.Min(this.Count - 1, Math.Max(i1, 0))].η(sign, x), y2 = this[Math.Min(i1 + 1, Count - 1)].η(sign, x);
                Double a1 = this[Math.Min(this.Count - 1, Math.Max(i1, 0))].As, a2 = this[Math.Min(i1 + 1, Count - 1)].As;

                return y1 == y2 ? y1 : (a - a1) / (a2 - a1) * (y2 - y1) + y1;
            }
        }

        #endregion

        public class CnValue
        {
            public Double Value { get; protected set; }

            public CnValue(Double value) { Value = value; }
        }


        internal CnValue ControlValue(Double value, ais7PassTypeEnum restriction)
        {
            Dictionary<ais7SfUse, Double> values = new Dictionary<ais7SfUse, double>();
            values.Add(ais7SfUse.Single, value);
            return ControlValue(values, restriction);
        }

        internal virtual CnValue ControlValue(Dictionary<ais7SfUse, Double> values, ais7PassTypeEnum restriction)
        {
            Double str = Strength;
            switch (restriction)
            {
                case ais7PassTypeEnum.WoPedestian:
                case ais7PassTypeEnum.Speed10:
                case ais7PassTypeEnum.SingleOnlyAndPlace:
                case ais7PassTypeEnum.SingleOnly:
                    str += PedestianWeight;
                    break;
            }

            Double rv = Math.Abs(values[ais7SfUse.Single]) < 0.00001 ? Double.NaN : str / values[ais7SfUse.Single];
            return new CnValue(rv);
        }

        public CnValue Test(Dictionary<ais7SfUse, Double> values) { return ControlValue(values, new ais7PassTypeEnum()); }

        public Double Strength { get { return 0; } set { } }

        public virtual Double PedestianWeight { get { return 0; } }


    }


    public enum ais7PassTypeEnum
    {

        [Description("Нет сведений")]
        Unknown,
        [Description("Пропуск возможен без ограничений")]
        NoLimit,
        [Description("Пропуск возможен при отсутствии пешеходов на тротуарах")]
        WoPedestian,
        [Description("Пропуск возможен с ограничением скорости до 10км/ч")]
        Speed10,
        [Description("Пропуск возможен в одиночном порядке с ограничением скорости до 10км/ч")]
        SingleOnly,
        [Description("Пропуск возможен в одиночном порядке, с ограничением скорости до 10км/ч и положения")]
        SingleOnlyAndPlace,
        [Description("Пропуск невозможен")]
        Denied
    }
}
