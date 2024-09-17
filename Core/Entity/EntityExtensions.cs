/*
 * Sidub Platform - Core
 * Copyright (C) 2024 Sidub Inc.
 * All rights reserved.
 *
 * This file is part of Sidub Platform - Core (the "Product").
 *
 * The Product is dual-licensed under:
 * 1. The GNU Affero General Public License version 3 (AGPLv3)
 * 2. Sidub Inc.'s Proprietary Software License Agreement (PSLA)
 *
 * You may choose to use, redistribute, and/or modify the Product under
 * the terms of either license.
 *
 * The Product is provided "AS IS" and "AS AVAILABLE," without any
 * warranties or conditions of any kind, either express or implied, including
 * but not limited to implied warranties or conditions of merchantability and
 * fitness for a particular purpose. See the applicable license for more
 * details.
 *
 * See the LICENSE.txt file for detailed license terms and conditions or
 * visit https://sidub.ca/licensing for a copy of the license texts.
 */

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
