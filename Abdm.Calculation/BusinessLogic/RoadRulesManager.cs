using System;
using System.Threading;
using Abdm.Calculation.Models;
using Newtonsoft.Json.Linq;

namespace Abdm.Calculation.BusinessLogic
{
    /// <summary>
    /// Менеджер, который хранит в себе инфу по ИССО.
    /// </summary>
    public class RoadRulesManager
    {
        public static TimeSpan DATA_LIFESPAN = TimeSpan.FromMinutes(1);

        

        public RoadRules RefreshRoadRules(long issoId, NagruzkaTypeEnum nagruzkaType)
        {
            if (IssoId != issoId || 
                NagruzkaType != nagruzkaType ||
                DateTime.Now - DateTimeUpdated < DATA_LIFESPAN)
            {
                _lock.EnterWriteLock();
                try { 
                    var nagruzkaGroupType = GetNagruzkaGroupType(nagruzkaType);
                    RoadRules value = new RoadRules();
                    switch (nagruzkaGroupType)
                    {
                        case NagruzkaGroupTypeEnum.Common:

                            break;
                        case NagruzkaGroupTypeEnum.AClass:
                            break;


                        case NagruzkaGroupTypeEnum.Single:
                            break;
                        case NagruzkaGroupTypeEnum.NClass:
                            break;
                        
                        case NagruzkaGroupTypeEnum.Track:
                            break;

                        case NagruzkaGroupTypeEnum.AB:
                            break;
                        default:
                            break;
                    }



                    roadRules = value;
                    IssoId = issoId;
                    NagruzkaType = nagruzkaType;
                    DateTimeUpdated = DateTime.Now;
                }
                finally { _lock.ExitWriteLock(); }
            }
            return RoadRules;
        }

        public long IssoId { get; private set; }

        public DateTime DateTimeUpdated { get; private set; }

        public NagruzkaTypeEnum NagruzkaType { get; private set; }

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private RoadRules roadRules;

        public RoadRules RoadRules
        {
            get
            {
                _lock.EnterReadLock();
                try { return roadRules; }
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
