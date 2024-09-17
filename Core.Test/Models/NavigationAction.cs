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

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity.Relations;

namespace Sidub.Platform.Core.Test.Models
{

    [Entity("NavigationAction")]
    public class NavigationAction : INavigationItem
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [EntityRecordRelation<INavigationActionTarget>("Target", EntityRelationshipType.Composition)]
        public EntityReference<INavigationActionTarget> Target { get; set; }

        public bool IsRetrievedFromStorage { get; set; }

        public NavigationAction()
        {
            Id = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            Target = EntityReference<INavigationActionTarget>.Null;
        }

        public NavigationAction(string id, string title, INavigationActionTarget target)
        {
            Id = id;
            Title = title;
            Description = string.Empty;
            Target = EntityReference.Create(target);
        }



    }

}
