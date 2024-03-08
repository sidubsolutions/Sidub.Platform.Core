//namespace Sidub.Platform.Core.Entity
//{
//    public static class EntityExtensions
//    {
//        public static bool EqualsByEntityKeys<TEntity>(this TEntity? left, TEntity? right) where TEntity : IEntity
//        {
//            if (left is null || right is null)
//                return left is null && right is null;

//            if (left.GetType() != right.GetType())
//                return false;

//            var leftKeys = EntityTypeHelper.GetEntityKeyValues(left);
//            var rightKeys = EntityTypeHelper.GetEntityKeyValues(right);

//            foreach (var key in leftKeys.Keys)
//            {
//                if (rightKeys.ContainsKey(key) && leftKeys[key] == rightKeys[key])
//                    rightKeys.Remove(key);
//                else
//                    return false;
//            }

//            return rightKeys.Count == 0;
//        }

//    }
//}
