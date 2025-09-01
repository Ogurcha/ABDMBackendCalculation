using System;
using System.Threading;
using Abdm.Calculation.Models;

namespace Abdm.Calculation.BusinessLogic
{
    /// <summary>
    /// Менеджер, который хранит в себе инфу по ИССО.
    /// </summary>
    public class RoadRulesManager : IRoadRulesManager
    {
        public static TimeSpan DATA_LIFESPAN = TimeSpan.FromMinutes(1);

        public RoadRules RefreshRoadRules(long issoId, NagruzkaTypeEnum nagruzkaType)
        {
            if (IssoId != issoId ||
                NagruzkaType != nagruzkaType ||
                DateTime.Now - DateTimeUpdated < DATA_LIFESPAN)
            {
                _lock.EnterWriteLock();
                try
                {
                    var nagruzkaGroupType = GetNagruzkaGroupType(nagruzkaType);
                    RoadRules value = new RoadRules();
                    RoadRules valueSecondary = new RoadRules();
                    HasSecondaryRule = false;
                    switch (nagruzkaGroupType)
                    {
                        case NagruzkaGroupTypeEnum.Common when nagruzkaType == NagruzkaTypeEnum.EN3:
                            value = RoadRulesStatic.RR1_1;
                            valueSecondary = RoadRulesStatic.RR2_1;
                            HasSecondaryRule = true;
                            break;
                        case NagruzkaGroupTypeEnum.AClass:
                        case NagruzkaGroupTypeEnum.Common:
                            value = RoadRulesStatic.RR1;
                            valueSecondary = RoadRulesStatic.RR2;
                            HasSecondaryRule = true;
                            break;
                        case NagruzkaGroupTypeEnum.Single:
                        case NagruzkaGroupTypeEnum.NClass:
                        case NagruzkaGroupTypeEnum.Track:
                            value = RoadRulesStatic.RR3;
                            break;
                        case NagruzkaGroupTypeEnum.AB:
                            value = RoadRulesStatic.RR4;
                            break;
                        default:
                            break;
                    }

                    roadRule = value;
                    secondaryRoadRule = valueSecondary;
                    IssoId = issoId;
                    NagruzkaType = nagruzkaType;
                    DateTimeUpdated = DateTime.Now;
                }
                finally { _lock.ExitWriteLock(); }
            }
            return RoadRule;
        }

        public long IssoId { get; private set; }

        public DateTime DateTimeUpdated { get; private set; }

        public NagruzkaTypeEnum NagruzkaType { get; private set; }

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private RoadRules roadRule;

        public RoadRules RoadRule
        {
            get
            {
                _lock.EnterReadLock();
                try { return roadRule; }
                finally { _lock.ExitReadLock(); }
            }
        }

        public bool HasSecondaryRule { get; private set; }

        private RoadRules secondaryRoadRule;

        public RoadRules SecondaryRoadRule
        {
            get
            {
                _lock.EnterReadLock();
                try { return secondaryRoadRule; }
                finally { _lock.ExitReadLock(); }
            }
        }

        public static NagruzkaGroupTypeEnum GetNagruzkaGroupType(NagruzkaTypeEnum nagruzkaType)
        {
            switch (nagruzkaType)
            {
                case NagruzkaTypeEnum.A8:
                case NagruzkaTypeEnum.A11:
                case NagruzkaTypeEnum.A14:
                    return NagruzkaGroupTypeEnum.AClass;
                case NagruzkaTypeEnum.N11:
                case NagruzkaTypeEnum.N14:
                    return NagruzkaGroupTypeEnum.NClass;
                case NagruzkaTypeEnum.AB51:
                case NagruzkaTypeEnum.AB74:
                case NagruzkaTypeEnum.AB151:
                    return NagruzkaGroupTypeEnum.AB;
                case NagruzkaTypeEnum.NG60:
                case NagruzkaTypeEnum.NG30:
                case NagruzkaTypeEnum.T60:
                case NagruzkaTypeEnum.T25:
                    return NagruzkaGroupTypeEnum.Track;
                case NagruzkaTypeEnum.N_10:
                case NagruzkaTypeEnum.N_13:
                case NagruzkaTypeEnum.N_18:
                case NagruzkaTypeEnum.N_30:
                case NagruzkaTypeEnum.EN3:
                default:
                    return NagruzkaGroupTypeEnum.Common;
            }
        }
    }
}
