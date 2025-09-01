using System;
using System.Threading;
using Abdm.Calculation.Models;
using Microsoft.Extensions.Configuration;

namespace Abdm.Calculation.BusinessLogic
{
    /// <summary>
    /// Менеджер, который хранит в себе инфу по ИССО.
    /// </summary>
    public class RoadRulesManager : IRoadRulesManager 
    {
        public static TimeSpan DATA_LIFESPAN;

        public RoadRulesManager(RoadRulesSettings settings)
        {
            DATA_LIFESPAN = TimeSpan.FromMinutes(settings.DataLifeSpanMinutes);
        }

        public RoadRules RefreshRoadRules(long issoId, NagruzkaEnum nagruzkaType)
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
                        case NagruzkaGroupTypeEnum.Common when nagruzkaType == NagruzkaEnum.EN3:
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

        public NagruzkaEnum NagruzkaType { get; private set; }

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

        public static NagruzkaGroupTypeEnum GetNagruzkaGroupType(NagruzkaEnum nagruzkaType)
        {
            switch (nagruzkaType)
            {
                case NagruzkaEnum.A8:
                case NagruzkaEnum.A11:
                case NagruzkaEnum.A14:
                    return NagruzkaGroupTypeEnum.AClass;
                case NagruzkaEnum.N11:
                case NagruzkaEnum.N14:
                    return NagruzkaGroupTypeEnum.NClass;
                case NagruzkaEnum.AB51:
                case NagruzkaEnum.AB74:
                case NagruzkaEnum.AB151:
                    return NagruzkaGroupTypeEnum.AB;
                case NagruzkaEnum.NG60:
                case NagruzkaEnum.NG30:
                case NagruzkaEnum.T60:
                case NagruzkaEnum.T25:
                    return NagruzkaGroupTypeEnum.Track;
                case NagruzkaEnum.N_10:
                case NagruzkaEnum.N_13:
                case NagruzkaEnum.N_18:
                case NagruzkaEnum.N_30:
                case NagruzkaEnum.EN3:
                default:
                    return NagruzkaGroupTypeEnum.Common;
            }
        }
    }
}
