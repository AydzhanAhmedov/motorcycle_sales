﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motorcycle_sales.Core.Entities;

public class BaseEntity
{
    public virtual int Id { get; protected set; }
}