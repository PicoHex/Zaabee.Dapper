using System.Linq.Expressions;

namespace Zaabee.Dapper.Lambda.Resolver.ExpressionTree
{
    internal class SingleOperationNode : Node
    {
        public ExpressionType Operator { get; set; }
        public Node Child { get; set; }
    }
}
