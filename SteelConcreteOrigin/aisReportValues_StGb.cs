using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisPcCore.CheckPoint
{
    internal class aisReportValues_StGb
    {
        public Table01 Tb01 { get; set; } = new Table01 ();
        public Table02 Tb02 { get; set; } = new Table02 ();
        public Table03 Tb03 { get; set; } = new Table03 ();
        public Table04 Tb04 { get; set; } = new Table04 ();
        public Table05 Tb05 { get; set; } = new Table05 ();
        public Table06 Tb06 { get; set; } = new Table06 ();
        public Table07 Tb07 { get; set; } = new Table07 ();
        public Table08 Tb08 { get; set; } = new Table08 ();
        public Table09 Tb09 { get; set; } = new Table09 ();
        public Table10 Tb10 { get; set; } = new Table10 ();
        public Table11 Tb11 { get; set; } = new Table11 ();
        public Table12 Tb12 { get; set; } = new Table12 ();
        public Table13 Tb13 { get; set; } = new Table13 ();
        public Table14 Tb14 { get; set; } = new Table14 ();
        public Table15 Tb15 { get; set; } = new Table15 ();
        public Table16 Tb16 { get; set; } = new Table16 ();
        public Table17 Tb17 { get; set; } = new Table17 ();
        public Table18 Tb18 { get; set; } = new Table18 ();
        public Table19 Tb19 { get; set; } = new Table19 ();
        public Table20 Tb20 { get; set; } = new Table20 ();
        public Table21 Tb21 { get; set; } = new Table21 ();
        public Table22 Tb22 { get; set; } = new Table22 ();

        public List<TableBase> Tables => new List<TableBase>()
        {
            Tb01,Tb02,Tb03,Tb04,Tb05,Tb06,Tb07,Tb08,Tb09,Tb10,
            Tb11,Tb12,Tb13,Tb14,Tb15,Tb16,Tb17,Tb18,Tb19,Tb20,
            Tb21,Tb22
        };

        internal class TableBase
        {
            public Dictionary<string, double> DebugValues { get; set; } = new Dictionary<string, double>();
        }

        internal class Table01 : TableBase { }
        internal class Table02 : TableBase
        {
            public bool Применимость_метода_тонкой_плиты;
            public double ε_shr;
        }
        internal class Table03 : TableBase { }
        internal class Table04 : TableBase { }
        internal class Table05 : TableBase
        {
            public double σb1;
            public bool Учет_ползучести;
            public double σb_cr;
            public double σr_cr;
        }
        internal class Table06 : TableBase
        {
            public bool Учет_ползучести;
            public double v;
            public double cn;
            public double ϕ_kr;
            public double ϕ_kr_d;
            public double α;
            public double σ_b_kr;
            public double σ_r_kr;
            public double ksi1;
            public double ksi2;
            public double ksi3;
            public double ksi4;

        }
        internal class Table07 : TableBase { }
        internal class Table08 : TableBase
        {
            public double K; 
            public double Mv;
            public double M2;
            public double M;
        }
        internal class Table09 : TableBase
        {
            public double σb;
            public double mbRb;
            public double σr;
            public double mrRr;
            public string Расчетный_случай;
            public double Nbr;
            public double σs2_I;
            public double σs2_IIg;
            public double σs2_IIv;
            public double σs1_I;
            public double σs1_IIg;
            public double σs1_IIv;

        }
        internal class Table10 : TableBase { }
        internal class Table11 : TableBase
        {
            public double η;
            public double æ3;
            public double m1;
            public double æ4;

        }
        internal class Table12 : TableBase
        {
            public double σs2;
            public double m1Rs2;
            public double σs1;
            public double Rs1;
            public double Запас_s2 => (m1Rs2 - σs2) / m1Rs2;
            public double Запас_s1 => (Rs1 - σs1) / Rs1;
            public double value;

        }
        internal class Table13 : Table04 { }
        internal class Table14 : TableBase
        {
            public double Z;
            public double Zst_stb;
            public double Sshr;
            public double Awt;
            public double As1_t;
            public double Zb1_stb;
            public double St;
        }
        internal class Table15 : Table05 { }
        internal class Table16 : TableBase
        {
            public double σb_shr;
            public double σr_shr;
        }
        internal class Table17 : TableBase
        {
            public double σb_t;
            public double σr_t;
        }
        internal class Table18 : Table08 { }
        internal class Table19 : Table09 { }
        internal class Table20 : Table10 { }
        internal class Table21 : Table11 { }
        internal class Table22 : Table12 { }
    }
}
