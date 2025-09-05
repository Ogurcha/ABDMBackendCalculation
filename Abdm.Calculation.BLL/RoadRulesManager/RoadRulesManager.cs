using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.RoadRulesManager.RoadRulesStrategy;
using Abdm.Calculation.BLL.Settings;

namespace Abdm.Calculation.BLL.RoadRulesManager
{
    /// <summary>
    /// Менеджер, который хранит в себе инфу по ИССО.
    /// </summary>
    public class RoadRulesManager : IRoadRulesManager 
    {
        public readonly TimeSpan dataLifespan;

        public long IssoId { get; private set; }

        public DateTime DateTimeUpdated { get; private set; }

        public LadingEnum LadingType { get; private set; }

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

        private readonly Dictionary<(LadingGroupTypeEnum, LadingEnum?), BaseRRStrategy> strategies;
        private static readonly Dictionary<LadingEnum, LadingGroupTypeEnum> ladingGroupTypeMap =
            new()
            {
                [LadingEnum.A8] = LadingGroupTypeEnum.AClass,
                [LadingEnum.A11] = LadingGroupTypeEnum.AClass,
                [LadingEnum.A14] = LadingGroupTypeEnum.AClass,
                [LadingEnum.N11] = LadingGroupTypeEnum.NClass,
                [LadingEnum.N14] = LadingGroupTypeEnum.NClass,
                [LadingEnum.AB51] = LadingGroupTypeEnum.AB,
                [LadingEnum.AB74] = LadingGroupTypeEnum.AB,
                [LadingEnum.AB151] = LadingGroupTypeEnum.AB,
                [LadingEnum.NG60] = LadingGroupTypeEnum.Track,
                [LadingEnum.NG30] = LadingGroupTypeEnum.Track,
                [LadingEnum.T60] = LadingGroupTypeEnum.Track,
                [LadingEnum.T25] = LadingGroupTypeEnum.Track,
                [LadingEnum.N_10] = LadingGroupTypeEnum.Common,
                [LadingEnum.N_13] = LadingGroupTypeEnum.Common,
                [LadingEnum.N_18] = LadingGroupTypeEnum.Common,
                [LadingEnum.N_30] = LadingGroupTypeEnum.Common,
                [LadingEnum.EN3] = LadingGroupTypeEnum.Common,
            };


        public RoadRulesManager(DataLifeSpan settings)
        {
            dataLifespan = TimeSpan.FromMinutes(settings.Minutes);

            RegisterStrategy((LadingGroupTypeEnum.Common, LadingEnum.EN3), new EN3Strategy());
            RegisterStrategy((LadingGroupTypeEnum.AClass, null), new AClassCommonStrategy());
            RegisterStrategy((LadingGroupTypeEnum.Common, null), new AClassCommonStrategy());
            RegisterStrategy((LadingGroupTypeEnum.Single, null), new HeavyStrategy());
            RegisterStrategy((LadingGroupTypeEnum.NClass, null), new HeavyStrategy());
            RegisterStrategy((LadingGroupTypeEnum.Track, null), new HeavyStrategy());
            RegisterStrategy((LadingGroupTypeEnum.AB, null), new AbStrategy());
        }

        public RoadRules RefreshRoadRules(long issoId, LadingEnum ladingId)
        {
            if (getRefrshCondition(issoId, ladingId))
            {
                _lock.EnterWriteLock();
                try
                {
                    if (!getRefrshCondition(issoId, ladingId))
                    {
                        return RoadRule;
                    }

                    var ladingGroupType = ladingGroupTypeMap[ladingId];

                    roadRule = ProcessRRStrategy(ladingGroupType, ladingId);

                    IssoId = issoId;
                    LadingType = ladingId;
                    DateTimeUpdated = DateTime.Now;
                }
                finally { _lock.ExitWriteLock(); }
            }
            return RoadRule;

            bool getRefrshCondition(long issoId, LadingEnum ladingId)
            {
                return IssoId != issoId ||
                    LadingType != ladingId ||
                    DateTime.Now - DateTimeUpdated < dataLifespan;
            }
        }

        private void RegisterStrategy((LadingGroupTypeEnum, LadingEnum?) key, BaseRRStrategy strategy)
        {
            strategies[key] = strategy;
        }

        public RoadRules ProcessRRStrategy(LadingGroupTypeEnum ladingGroupTypeEnum, LadingEnum ladingEnum)
        {
            if (strategies.TryGetValue((ladingGroupTypeEnum, ladingEnum), out var strategy))
            {
                return strategy.GetRoadRules();
            }
            else if (strategies.TryGetValue((ladingGroupTypeEnum, null), out var strategySecond))
            {
                return strategySecond.GetRoadRules();
            }
            else
            {
                throw new Exception("Invalid RRStrategy method");
            }
        }
    }
}
