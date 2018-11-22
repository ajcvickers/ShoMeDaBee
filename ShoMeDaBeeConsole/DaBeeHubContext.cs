using ShoMeDaBee.Internal;

namespace ShoMeDaBee
{
    public class DaBeeHubContext : DaBeeHub
    {
        public override IDaBeeSession Session => DaBeeSessionManager.Current;
    }
}