﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    public class ProfilesEntity : IEntityBase
    {
        public virtual string ProfileName { get; set; }
    }
}