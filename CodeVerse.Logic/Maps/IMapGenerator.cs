using CodeVerse.Common;
using CodeVerse.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Logic.Maps
{
    public interface IMapGenerator
    {
        List<Entity> Generate();
    }
}
