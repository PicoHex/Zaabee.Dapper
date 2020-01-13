using Zaabee.Dapper.Lambda.ValueObjects;

namespace Zaabee.Dapper.Lambda.Resolver.ExpressionTree
{
    internal class LikeNode : Node
    {
        public LikeMethod Method { get; set; }
        public MemberNode MemberNode { get; set; }
        public string Value { get; set; }
    }
}
