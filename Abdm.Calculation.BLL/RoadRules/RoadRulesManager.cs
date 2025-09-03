using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.RoadRules
{
    /// <summary>
    /// Менеджер, который хранит в себе инфу по ИССО.
    /// </summary>
    public class RoadRulesManager : IRoadRulesManager 
    {
        public static TimeSpan dataLifespan;

        public RoadRulesManager(DataLifeSpanSettings settings)
        {
            dataLifespan = TimeSpan.FromMinutes(settings.DataLifeSpanMinutes);
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

                    var ladingGroupType = GetLadingGroupType(ladingId);
                    RoadRules value = new RoadRules();
                    RoadRules valueSecondary = new RoadRules();
                    HasSecondaryRule = false;
                    switch (ladingGroupType)
                    {
                        case LadingGroupTypeEnum.Common when ladingId == LadingEnum.EN3:
                            value = RoadRulesStatic.RR1_1;
                            valueSecondary = RoadRulesStatic.RR2_1;
                            HasSecondaryRule = true;
                            break;
                        case LadingGroupTypeEnum.AClass:
                        case LadingGroupTypeEnum.Common:
                            value = RoadRulesStatic.RR1;
                            valueSecondary = RoadRulesStatic.RR2;
                            HasSecondaryRule = true;
                            break;
                        case LadingGroupTypeEnum.Single:
                        case LadingGroupTypeEnum.NClass:
                        case LadingGroupTypeEnum.Track:
                            value = RoadRulesStatic.RR3;
                            break;
                        case LadingGroupTypeEnum.AB:
                            value = RoadRulesStatic.RR4;
                            break;
                        default:
                            break;
                    }


                    secondaryRoadRule = valueSecondary;
                    if (HasSecondaryRule)
                    {
                        roadRule = Merge(value, valueSecondary);
                    }
                    else
                    {
                        roadRule = value;
                    }

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

        public static LadingGroupTypeEnum GetLadingGroupType(LadingEnum ladingId)
        {
            switch (ladingId)
            {
                case LadingEnum.A8:
                case LadingEnum.A11:
                case LadingEnum.A14:
                    return LadingGroupTypeEnum.AClass;
                case LadingEnum.N11:
                case LadingEnum.N14:
                    return LadingGroupTypeEnum.NClass;
                case LadingEnum.AB51:
                case LadingEnum.AB74:
                case LadingEnum.AB151:
                    return LadingGroupTypeEnum.AB;
                case LadingEnum.NG60:
                case LadingEnum.NG30:
                case LadingEnum.T60:
                case LadingEnum.T25:
                    return LadingGroupTypeEnum.Track;
                case LadingEnum.N_10:
                case LadingEnum.N_13:
                case LadingEnum.N_18:
                case LadingEnum.N_30:
                case LadingEnum.EN3:
                default:
                    return LadingGroupTypeEnum.Common;
            }
        }

        private RoadRules Merge(RoadRules value, RoadRules valueSecondary)
        {
            return new RoadRules
            {
                IsPedestrianAllowed = value.IsPedestrianAllowed || valueSecondary.IsPedestrianAllowed,
                IsDynamicMovement = value.IsDynamicMovement || valueSecondary.IsDynamicMovement,
                HasSafetyLine = value.HasSafetyLine && valueSecondary.HasSafetyLine,
                MaxAutoInColumn = Math.Max(value.MaxAutoInColumn, valueSecondary.MaxAutoInColumn),
                MaxColumnCount = Math.Max(value.MaxColumnCount, valueSecondary.MaxColumnCount),
                MinColumnDistance = Math.Min(value.MinColumnDistance, valueSecondary.MinColumnDistance)
            };
        }
    }
}
