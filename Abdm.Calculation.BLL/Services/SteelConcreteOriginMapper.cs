using System.Globalization;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.StrainAnalysis.SteelConcrete;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Models;
using Abdm.Calculation.SteelConcrete.SteelConcrete;
using Abdm.Calculation.WebApi.ResponseModels.StrainAnalysis.SteelConcrete;
using AisPcCore.CheckPoint;
using AisPcCore.CheckPoint.StGbCrossSection;
using AisPcCore.SfData;
using IssoSteelConcreteParameters = Abdm.Calculation.SteelConcrete.Models.IssoSteelConcreteParameters;

namespace Abdm.Calculation.BLL.Services
{
    internal static class SteelConcreteOriginMapper
    {
        private const double Gravity = 9.81;

        public static ais7CheckPoint_StGb CreateCheckPoint(
            SteelConcreteData steelConcreteData,
            IssoSteelConcreteParameters parameters)
        {
            var checkPoint = new ais7CheckPoint_StGb();
            MapCrossSection(checkPoint, steelConcreteData.CrossSection);
            MapParameters(checkPoint, parameters);
            return checkPoint;
        }

        public static AnalysisSteelConcrete MapToAnalysisSteelConcrete(
            ais7CheckPoint_StGb checkPoint,
            aisReportValues_StGb repVal,
            IssoSteelConcreteParameters parameters)
        {
            var cs = checkPoint.CS;
            var concreteSlabInertia = GetConcreteSlabInertia(cs);
            var thinPlateApplicable = IsThinPlateMethodApplicable(checkPoint, concreteSlabInertia);

            return new AnalysisSteelConcrete
            {
                InputParameters = MapInputParameters(checkPoint, parameters),
                Materials = MapMaterials(checkPoint, repVal, concreteSlabInertia, thinPlateApplicable),
                LoadsAtCheckPoint = MapLoads(parameters),
                SectionGeometric = MapSectionGeometric(cs, nb1: false),
                CreepAccounting = MapCreepAccounting(repVal.Tb05, checkPoint.Rb),
                LoadClasses = MapLoadClasses(repVal.Tb08),
                StressesAtSlab = MapStressesAtSlab(repVal.Tb09),
                Coefficients = MapCoefficients(repVal.Tb11),
                SteelBeamBelts = MapSteelBeamBelts(repVal.Tb12),
                SectionGeometric13 = MapSectionGeometric(cs, nb1: true),
                AdditionalCharacteristics = MapAdditionalCharacteristics(repVal.Tb14),
                CreepAccounting15 = MapCreepAccounting(repVal.Tb15, checkPoint.Rb),
                ShrinkageStress = MapConcreteStress(repVal.Tb16.σb_shr, repVal.Tb16.σr_shr),
                TemperatureStress = MapConcreteStress(repVal.Tb17.σb_t, repVal.Tb17.σr_t),
                LoadClasses18 = MapLoadClasses(repVal.Tb18),
                StressesAtSlab19 = MapStressesAtSlab(repVal.Tb19),
                Coefficients21 = MapCoefficients(repVal.Tb21),
                SteelBeamBelts22 = MapSteelBeamBelts(repVal.Tb22),
            };
        }

        private static void MapCrossSection(ais7CheckPoint_StGb checkPoint, CrossSection crossSection)
        {
            checkPoint.CS.Items.Clear();
            checkPoint.CS.Corners.Clear();

            foreach (var rectangle in crossSection.Rectangles)
            {
                checkPoint.CS.Add(new ais7stGbCSItemRect(checkPoint.CS)
                {
                    Width = rectangle.Width,
                    Height = rectangle.Height,
                    DHeight = rectangle.DHeight,
                    Ar = rectangle.Ar,
                    dYr = rectangle.dYr,
                    Material = (ais7stGbCSItemMaterial)(int)rectangle.Material,
                });
            }

            foreach (var corner in crossSection.Corners)
            {
                var location = corner.Location == CornerLocationEnum.Upper
                    ? ais7stGbCSItemCorner.Location.Up
                    : ais7stGbCSItemCorner.Location.Down;

                checkPoint.CS.AddCorner(new ais7stGbCSItemCorner(checkPoint.CS)
                {
                    Width = corner.Width,
                    Height = corner.Height,
                    H2 = corner.H2,
                }, location);
            }
        }

        private static void MapParameters(ais7CheckPoint_StGb checkPoint, IssoSteelConcreteParameters parameters)
        {
            checkPoint.workSign = parameters.IsNegative
                ? ais7CheckPoint_StGb.WorkSign.Minus
                : ais7CheckPoint_StGb.WorkSign.Plus;
            checkPoint.M1 = parameters.M1;
            checkPoint.M2g = parameters.M2g;
            checkPoint.Mp = parameters.Mp;
            checkPoint.Es = parameters.Es;
            checkPoint.Ea = parameters.Ea;
            checkPoint.Eb = parameters.Eb;
            checkPoint.Rs1 = parameters.Rs1;
            checkPoint.Rs2 = parameters.Rs2;
            checkPoint.Rb = parameters.Rb;
            checkPoint.ε_b_lim = parameters.EpsilonBetaLim;
            checkPoint.Rr = parameters.Rr;
            checkPoint.χ1 = parameters.X1Coefficient;
            checkPoint.L = parameters.L;
            checkPoint.Sd = parameters.Sd;
            checkPoint.plateType = parameters.PlateType == PlateTypeEnum.Combined
                ? ais7CheckPoint_StGb.PlateType.Combined
                : ais7CheckPoint_StGb.PlateType.Monolithic;
            checkPoint.tmax = parameters.TMax;

            if (parameters.TetaKrParam.HasValue)
            {
                checkPoint.ϕ_kr = parameters.TetaKrParam.Value;
            }

            if (parameters.SigmaBetaKrParam > 1e-3)
            {
                checkPoint.σ_b_kr = parameters.SigmaBetaKrParam;
            }

            if (parameters.SigmaAlfaKrParam > 1e-3)
            {
                checkPoint.σ_a_kr = parameters.SigmaAlfaKrParam;
            }

            if (parameters.SigmaBetaShrParam is > 1e-3)
            {
                checkPoint.σ_b_shr = parameters.SigmaBetaShrParam.Value;
            }

            if (parameters.SigmaAlfaShrParam is > 1e-3)
            {
                checkPoint.σ_a_shr = parameters.SigmaAlfaShrParam.Value;
            }

            if (parameters.SigmaBetaTParam is > 1e-3)
            {
                checkPoint.σ_b_t = parameters.SigmaBetaTParam.Value;
            }

            if (parameters.SigmaAlfaTParam is > 1e-3)
            {
                checkPoint.σ_a_t = parameters.SigmaAlfaTParam.Value;
            }
        }

        private static SteelConcreteInputParameters MapInputParameters(
            ais7CheckPoint_StGb checkPoint,
            IssoSteelConcreteParameters parameters)
        {
            var cs = checkPoint.CS;
            cs.nb1 = false;

            var plate = cs.PlateItem;
            var upper = cs.UpperItems.FirstOrDefault();
            var lower = cs.LowerItems.FirstOrDefault();
            var web = cs.VerticalItem;

            return new SteelConcreteInputParameters
            {
                DesignSlabWidth = MmToM(plate?.Width ?? 0),
                DesignSlabThickness = MmToM(plate?.Height2 ?? 0),
                GapTopFlangeToSlabBottom = MmToM(plate?.dYr ?? 0),
                TopFlangePlateThickness = MmToM(upper?.Height2 ?? 0),
                TopFlangePlateWidth = MmToM(upper?.Width ?? 0),
                WebPlateThickness = MmToM(web?.Width ?? 0),
                WebPlateWidth = MmToM(web?.Height2 ?? 0),
                BottomFlangePlateThickness = MmToM(lower?.Height2 ?? 0),
                BottomFlangePlateWidth = MmToM(lower?.Width ?? 0),
                SteelBeamHeight = D(cs.hsb),
                CompositeSectionHeight = MmToM(cs.Items.Sum(item => item.Height)),
                LongitudinalReinforcementArea = D(cs.Ar),
                LimitedPlasticDeformationFactor = D(checkPoint.χ1),
                SteelSectionArea = D(cs.As),
                StaticMomentSteelSection = D(cs.Ss_aa),
                SteelSectionAreaWithReinforcement = D(cs.Ast),
                StaticMomentSteelSectionWithReinforcement = D(cs.Ss_aa + cs.Sr_aa),
                MomentOfInertiaSteelSection = D(cs.Is),
                MomentOfInertiaSteelSectionAboutCentroid = D(cs.Is_co),
                SteelSectionCentroidPosition = D(cs.Zs1_s),
                DistanceFromSteelCentroidToTopFiber = D(cs.hsb - cs.Zs2_s),
                MomentOfInertiaSteelPart = D(cs.Is_aa + cs.Is_co - cs.Zs1_s * cs.Ss_aa),
                MomentOfInertiaSteelSectionWithReinforcement = D(cs.Is_aa + cs.Is_co + cs.Ir_aa - cs.Zs1_st * (cs.Ss_aa + cs.Sr_aa)),
                SteelSectionCentroidWithReinforcementPosition = D(cs.Zs1_st),
                SectionModulusTopFlangeSteelPart = D(cs.Ws2_s),
                SectionModulusBottomFlangeSteelPart = D(cs.Ws1_s),
            };
        }

        private static SteelConcreteMaterials MapMaterials(
            ais7CheckPoint_StGb checkPoint,
            aisReportValues_StGb repVal,
            double concreteSlabInertia,
            bool thinPlateApplicable)
        {
            var cs = checkPoint.CS;
            cs.nb1 = false;
            var nb1 = cs.nb;
            cs.nb1 = true;
            var nb2 = cs.nb;

            return new SteelConcreteMaterials
            {
                SteelElasticModulus = D(checkPoint.Es),
                ReinforcementElasticModulus = D(checkPoint.Ea),
                ConcreteElasticModulus = D(checkPoint.Eb),
                ConversionCoefficientFirst = D(nb1),
                ConversionCoefficientSecond = D(nb2),
                ConcreteUltimateCompressiveStrain = D(checkPoint.ε_b_lim),
                ConcreteElasticModulusForShrinkage = D(checkPoint.Eb),
                NbConversionCoefficientFirst = D(nb1),
                NbConversionCoefficientSecond = D(nb2),
                ConcreteUltimateStrainForShrinkage = D(repVal.Tb02.ε_shr),
                UpperSteelDesignStrength = D(checkPoint.Rs2),
                LowerSteelDesignStrength = D(checkPoint.Rs1),
                SlabReinforcementDesignStrength = D(checkPoint.Rr),
                ConcreteDesignStrength = D(checkPoint.Rb),
                MaximumTemperatureDifference = D(checkPoint.tmax),
                ConcreteSlabMomentOfInertia = D(concreteSlabInertia),
                ThinPlateMethodApplicable = thinPlateApplicable,
            };
        }

        private static SteelConcreteLoads MapLoads(IssoSteelConcreteParameters parameters) => new()
        {
            PermanentLoadsFirstStageMoments = TonMeterToMNm(parameters.M1),
            PermanentLoadsSecondStageMoments = TonMeterToMNm(parameters.M2g),
            PedestrianLoadMoments = TonMeterToMNm(parameters.Mp),
        };

        private static SectionGeometricCharacteristics MapSectionGeometric(ais7StGbCrossSection cs, bool nb1)
        {
            cs.nb1 = nb1;

            var plate = cs.PlateItem;
            var distanceFromSlabCentroidToTopFiber = plate != null
                ? plate.Height2 / 2000.0 + plate.dYr / 1000.0
                : 0.0;

            return new SectionGeometricCharacteristics
            {
                SlabAreaReducedToSteelExcludingReinforcement = D(cs.Abn),
                SlabAreaReducedToSteelWithReinforcement = D(cs.Ab),
                SlabStaticMomentReducedToSteelWithReinforcement = D(cs.Sb_aa),
                SlabMomentOfInertiaReducedToSteelWithReinforcement = D(cs.Ib_aa + cs.Ib_co - cs.Zbr * cs.Sb_aa),
                SlabMomentOfInertiaReducedToSteelAboutCentroidExcludingRebar = D(cs.Ib_co),
                CompositeBeamSectionArea = D(cs.Astb),
                CompositeBeamStaticMoment = D(cs.Sstb_aa),
                CompositeBeamMomentOfInertia = D(cs.Istb_aa + cs.Istb_co - cs.Zstb * cs.Sstb_aa),
                CompositeBeamMomentOfInertiaAboutCentroid = D(cs.Istb_co),
                ConcreteSlabCentroidPosition = D(cs.Zbr),
                CompositeSectionCentroidPosition = D(cs.Zstb),
                DistanceBetweenCompositeAndSlabCentroids = D(cs.Zstb - cs.Zbr),
                DistanceBetweenSteelCentroidAndSlabCentroid = D(cs.Zs1_s - cs.Zbr),
                DistanceFromCompositeCentroidToTopFiberOfConcreteSlab = D(cs.Zb_stb),
                DistanceFromConcreteSlabCentroidToTopFiber = D(distanceFromSlabCentroidToTopFiber),
                CombinedSectionMomentOfInertia = D(cs.Istb),
                SectionModulusTop = D(cs.Wb_stb),
                SectionModulusBottom = D(cs.Istb / Math.Max(cs.hsb - cs.Zstb, 1e-9)),
            };
        }

        private static CreepAccounting MapCreepAccounting(aisReportValues_StGb.Table05 table, double rb) => new()
        {
            StressesAtSlab = D(table.σb1),
            ControlValue = D(0.2 * rb),
            CreepAccountingNotRequired = !table.Учет_ползучести,
            StressesFromConcreteCreepInSlab = D(table.σb_cr),
            StressesFromConcreteCreepInReinforcement = D(table.σr_cr),
        };

        private static PermissibleLoadClasses MapLoadClasses(aisReportValues_StGb.Table08 table) => new()
        {
            PermissibleReferenceTemporaryLoadClasses = string.Empty,
            TemporaryLoadsMoments = KNmToMNm(table.Mv),
            FullBendingMomentSecondStage = KNmToMNm(table.M2),
            FullBendingMoment = KNmToMNm(table.M),
        };

        private static StressesAtSlabCentroid MapStressesAtSlab(aisReportValues_StGb.Table09 table) => new()
        {
            ConcreteStresses = D(table.σb),
            ConcreteControlValue = D(table.mbRb),
            ReinforcementStresses = D(table.σr),
            ReinforcementControlValue = D(table.mrRr),
            CalculationCase = table.Расчетный_случай,
            UnloadingForce = D(table.Nbr),
            UpperFlangeStresses = D(table.σs2_I),
            UpperFlangeStresses2 = D(table.σs2_IIg),
            LowerFlangeStresses = D(table.σs1_I),
            LowerFlangeStresses2 = D(table.σs1_IIg),
        };

        private static CorrectionCoefficients MapCoefficients(aisReportValues_StGb.Table11 table) => new()
        {
            Theta = Format(table.η),
            Ash3 = Format(table.æ3),
            M1 = Format(table.m1),
            Ash4 = Format(table.æ4),
        };

        private static SteelBeamBelts MapSteelBeamBelts(aisReportValues_StGb.Table12 table) => new()
        {
            UpperBelt = MapSteelBeamBelt(table.σs2, table.m1Rs2, table.Запас_s2, table.σs1, table.Rs1, table.Запас_s1, table.value),
            LowerBelt = MapSteelBeamBelt(table.σs1, table.Rs1, table.Запас_s1, table.σs2, table.m1Rs2, table.Запас_s2, table.value),
        };

        private static SteelBeamBelt MapSteelBeamBelt(
            double ak,
            double limitsAk,
            double reserveAk,
            double nk,
            double limitsNk,
            double reserveNk,
            double n3) => new()
        {
            AK = D(ak),
            StrainAK = D(ak),
            LimitsAK = D(limitsAk),
            ReserveAK = D(reserveAk),
            NK = D(nk),
            StrainNK = D(nk),
            LimitsNK = D(limitsNk),
            ReserveNK = D(reserveNk),
            N3 = D(n3),
            StrainN3 = D(n3),
            LimitsN3 = 0,
            ReserveN3 = 0,
        };

        private static AdditionalSectionCharacteristics MapAdditionalCharacteristics(aisReportValues_StGb.Table14 table) => new()
        {
            DistanceCombined = D(table.Z),
            DistanceSteel = D(table.Zst_stb),
            CombinedSectionStaticMoment = D(table.Sshr),
            VerticalPlateAreaSteelBeam = D(table.Awt),
            HorizontalPlateAreaBottomFlange = D(table.As1_t),
            Zb1 = D(table.Zb1_stb),
            S = D(table.St),
        };

        private static ConcreteStress MapConcreteStress(double stressInConcrete, double stressInArmature) => new()
        {
            StressInConcrete = D(stressInConcrete),
            StressInArmature = D(stressInArmature),
        };

        private static double GetConcreteSlabInertia(ais7StGbCrossSection cs)
        {
            var csConcrete = cs.Clone();
            csConcrete.Items.RemoveAll(item => item.Material == ais7stGbCSItemMaterial.Steel);
            foreach (var rect in csConcrete.Items)
            {
                rect.Height -= rect.DHeight;
                rect.DHeight = 0;
                rect.Ar = 0;
            }

            csConcrete.Corners.Clear();
            return csConcrete.Ib_aa + csConcrete.Ib_co - csConcrete.Sb_aa / csConcrete.Ab * csConcrete.Sb_aa;
        }

        private static bool IsThinPlateMethodApplicable(ais7CheckPoint_StGb checkPoint, double concreteSlabInertia) =>
            checkPoint.Eb * concreteSlabInertia < 0.2 * checkPoint.Est * checkPoint.CS.Is;

        private static decimal D(double value) => MathExtensions.ToDecimal(value);

        private static decimal MmToM(double mm) => (decimal)(mm / 1000.0);

        private static decimal TonMeterToMNm(double tonMeter) => (decimal)(tonMeter * Gravity / 1_000_000.0);

        private static decimal KNmToMNm(double kNm) => (decimal)(kNm / 1000.0);

        private static string Format(double value) =>
            value.ToString("G", CultureInfo.InvariantCulture);
    }
}
