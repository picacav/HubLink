﻿//******************************************************************************
// <copyright file="license.md" company="Wlog project  (https://github.com/arduosoft/wlog)">
// Copyright (c) 2016 Wlog project  (https://github.com/arduosoft/wlog)
// Wlog project is released under LGPL terms, see license.md file.
// </copyright>
// <author>Daniele Fontani, Emanuele Bucaelli</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    public class ApplicationRoleEntity : IEntityBase
    {
        [BsonId]
        public override Guid Id { get; set; }
        public virtual Guid ApplicationId { get; set; }
        public virtual Guid RoleId{ get; set; }
    }

    
}