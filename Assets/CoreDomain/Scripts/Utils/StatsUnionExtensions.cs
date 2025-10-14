using SpacetimeDB.Types;

namespace CoreDomain.Scripts.Utils
{
    public static  class StatsUnionExtensions
    {
        public static T TryGet<T>(this StatsUnion statsUnion) where T : class
        {
            return statsUnion switch
            {
                StatsUnion.Hero1(var s) when s is T casted => casted,
                StatsUnion.Hero2(var s) when s is T casted => casted,
                _ => null
            };
        }
    }
}